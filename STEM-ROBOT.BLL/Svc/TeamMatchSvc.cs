using AutoMapper;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class TeamMatchSvc
    {
        private readonly TeamMatchRepo _teamMatchRepo;
        private readonly IMapper _mapper;
        private readonly TeamRepo _teamRepo;
        private readonly MatchRepo _matchRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly TeamTableRepo _teamTableRepo;
        private readonly StageRepo _stageRepo;
        public TeamMatchSvc(TeamMatchRepo teamMatchRepo, IMapper mapper, TeamRepo teamRepo, MatchRepo matchRepo, TableGroupRepo tableGroupRepo, TeamTableRepo teamTableRepo, StageRepo stageRepo)
        {
            _teamMatchRepo = teamMatchRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
            _tableGroupRepo = tableGroupRepo;
            _teamTableRepo = teamTableRepo;
            _stageRepo = stageRepo;
        }
        public MutipleRsp GetListTeamMatch()
        {
            var res = new MutipleRsp();
            try
            {
                var teamMatch = _teamMatchRepo.All();
                if (teamMatch == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<TeamMatch>>(teamMatch);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp GetIdTeamMatch(int id)
        {
            var res = new SingleRsp();
            try
            {
                var teamMatch = _teamMatchRepo.GetById(id);
                if (teamMatch == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<TeamMatch>(teamMatch);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp AssignTeamsToMatchesInStageTable(int competitionId, List<AssignTeamsToMatchesInStageTableReq> matchAssignments)
        {
            var res = new SingleRsp();
            try
            {
                // Iterate over the match assignments from the frontend
                foreach (var assignment in matchAssignments)
                {
                    if (assignment.Teams == null || assignment.Teams.Count < 2)
                    {
                        throw new Exception($"Không đủ đội để tạo trận đấu cho match {assignment.MatchId}");
                    }

                    // Assign each team to the specified match
                    foreach (var team in assignment.Teams)
                    {
                        // Add team information to the match in TeamMatch table
                        AddTeamToMatch(team.TeamId, assignment.MatchId, team.IsHome);
                    }
                }

                res.SetMessage("OK");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xếp các đội vào trận đấu: {ex.Message}");
            }
            return res;
        }

        // Updated AddTeamToMatch to handle additional fields in TeamMatch table
        public void AddTeamToMatch(int teamId, int matchId, bool isHome)
        {
            var teamMatch = new TeamMatch
            {
                TeamId = teamId,
                MatchId = matchId,
                NameDefault = "Team Match Default Name", // Placeholder or can be set based on requirements
                IsPlay = false,
                ResultPlay = "Pending", // Initial result status
                TotalScore = 0,
                IsHome = isHome,
                IsSetup = true, // Assuming it is set to true when a match is assigned
                MatchWinCode = "Match_" + matchId // Example match code
            };

            _teamMatchRepo.Add(teamMatch);
        }

        private List<Team> GetQualifiedTeamsForNextRound(List<TableGroup> tables, int numberTeamsNextRound)
        {
            var qualifiedTeams = new List<Team>();
            var secondPlaceTeams = new List<Team>();
            var thirdPlaceTeams = new List<Team>();

            // Lấy các đội đạt hạng nhất, nhì và ba từ mỗi bảng đấu
            foreach (var table in tables)
            {
                var teamsInTable = _teamTableRepo.All()
                    .Where(tt => tt.TableGroupId == table.Id)
                    .Join(_teamRepo.All(), tt => tt.TeamId, t => t.Id, (tt, t) => t)
                    .ToList();

                if (teamsInTable.Any())
                {
                    // Thêm đội hạng nhất vào danh sách các đội vượt qua vòng bảng
                    qualifiedTeams.Add(teamsInTable[0]);

                    // Nếu có nhiều hơn một đội, thêm đội hạng nhì vào danh sách dự bị
                    if (teamsInTable.Count > 1)
                    {
                        secondPlaceTeams.Add(teamsInTable[1]);
                    }

                    // Nếu có nhiều hơn hai đội, thêm đội hạng ba vào danh sách dự bị
                    if (teamsInTable.Count > 2)
                    {
                        thirdPlaceTeams.Add(teamsInTable[2]);
                    }
                }
            }

            // Nếu số đội vượt qua vòng bảng chưa đủ, thêm các đội hạng nhì từ danh sách dự bị
            var random = new Random();
            while (qualifiedTeams.Count < numberTeamsNextRound && secondPlaceTeams.Any())
            {
                var randomIndex = random.Next(secondPlaceTeams.Count);
                qualifiedTeams.Add(secondPlaceTeams[randomIndex]);
                secondPlaceTeams.RemoveAt(randomIndex);
            }

            // Nếu số đội vượt qua vòng bảng vẫn chưa đủ, thêm các đội hạng ba từ danh sách dự bị
            while (qualifiedTeams.Count < numberTeamsNextRound && thirdPlaceTeams.Any())
            {
                var randomIndex = random.Next(thirdPlaceTeams.Count);
                qualifiedTeams.Add(thirdPlaceTeams[randomIndex]);
                thirdPlaceTeams.RemoveAt(randomIndex);
            }

            return qualifiedTeams;
        }


        public SingleRsp AssignTeamsToNextRoundMatches(int competitionId, int numberTeamsNextRound)
        {
            var res = new SingleRsp();
            try
            {
                // Lấy danh sách các bảng đấu trong vòng bảng
                var stageTable = _stageRepo.All(s => s.CompetitionId == competitionId && s.Name == "Vòng bảng").FirstOrDefault();
                // Get the list of tables in the group stage
                var tables = _tableGroupRepo.All().Where(t => t.StageId == stageTable.Id).ToList();
                if (tables == null || !tables.Any())
                {
                    throw new Exception("Không tìm thấy bảng đấu.");
                }

                // Sort tables alphabetically (assuming they are named A, B, C, etc.)
                tables = tables.OrderBy(t => t.Name).ToList();

                // Collect the first and second place teams separately
                var qualifiedTeams = new List<string>();
                var secondPlaceTeams = new List<string>();

                foreach (var table in tables)
                {
                    qualifiedTeams.Add($"W#1 {table.Name}");  // Winner of each table
                    secondPlaceTeams.Add($"W#2 {table.Name}"); // Runner-up of each table
                }

                // Adjust qualified teams based on the number needed for the next round
                if (qualifiedTeams.Count < numberTeamsNextRound)
                {
                    // Add runners-up if more teams are needed
                    int additionalTeamsNeeded = numberTeamsNextRound - qualifiedTeams.Count;
                    qualifiedTeams.AddRange(secondPlaceTeams.Take(additionalTeamsNeeded));
                }
                else if (qualifiedTeams.Count > numberTeamsNextRound)
                {
                    // Trim to match the required number of teams
                    qualifiedTeams = qualifiedTeams.Take(numberTeamsNextRound).ToList();
                }

                if (qualifiedTeams.Count != numberTeamsNextRound)
                {
                    throw new Exception("Số lượng đội vào vòng tiếp theo không đúng.");
                }

                // Calculate the number of knockout stages needed
                int numberOfStages = (int)Math.Ceiling(Math.Log2(qualifiedTeams.Count));

                // Get knockout stages sorted in order
                var knockoutStages = _stageRepo.All()
                    .Where(s => s.CompetitionId == competitionId && s.Name != "Vòng bảng")
                    .OrderBy(s => s.Id)
                    .Take(numberOfStages)
                    .ToList();

                if (knockoutStages == null || knockoutStages.Count < numberOfStages)
                {
                    throw new Exception("Không đủ các vòng đấu tiếp theo để sắp xếp trận đấu.");
                }

                // Assign matches for knockout rounds
                int remainingTeams = qualifiedTeams.Count;
                for (int stageIndex = 0; stageIndex < knockoutStages.Count && remainingTeams > 1; stageIndex++)
                {
                    var stage = knockoutStages[stageIndex];
                    int matchesInThisStage = remainingTeams / 2;

                    // Get matches in the current stage
                    var matches = _matchRepo.All().Where(m => m.StageId == stage.Id).OrderBy(m => m.Id).Take(matchesInThisStage).ToList();
                    if (matches.Count < matchesInThisStage)
                    {
                        throw new Exception($"Không đủ trận đấu trong {stage.Name}.");
                    }

                    for (int i = 0; i < matchesInThisStage; i++)
                    {
                        if (stageIndex == 0)
                        {
                            // Pair winners from different tables for the first round
                            AddTeamMatchWithPlaceholder(matches[i].Id, qualifiedTeams[i * 2]);
                            AddTeamMatchWithPlaceholder(matches[i].Id, qualifiedTeams[i * 2 + 1]);
                        }
                        else if (stageIndex == knockoutStages.Count - 1)
                        {
                            // Final match (championship round)
                            AddTeamMatchWithPlaceholder(matches[i].Id, "W#1 BÁN KẾT");
                            AddTeamMatchWithPlaceholder(matches[i].Id, "W#2 BÁN KẾT");
                        }
                        else
                        {
                            // Intermediate knockout rounds (e.g., quarterfinals, semifinals)
                            AddTeamMatchWithPlaceholder(matches[i].Id, $"W# {matchesInThisStage * stageIndex - matchesInThisStage + (i * 2) + 1}");
                            AddTeamMatchWithPlaceholder(matches[i].Id, $"W# {matchesInThisStage * stageIndex - matchesInThisStage + (i * 2) + 2}");
                        }
                    }

                    remainingTeams /= 2;
                }
                res.SetMessage("OK");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xếp các đội vào vòng tiếp theo: {ex.Message}");
            }
            return res;
        }



        // Hàm thêm thông tin đội vào TeamMatch với teamId null và nameDefault được chỉ định
        private void AddTeamMatchWithPlaceholder(int matchId, string nameDefault)
        {
            var teamMatch = new TeamMatch
            {
                MatchId = matchId,
                TeamId = null,
                ResultPlay = "Pending", // Trận đấu ban đầu chưa có kết quả
                NameDefault = nameDefault
            };
            _teamMatchRepo.Add(teamMatch);
        }
    }
}
