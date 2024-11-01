using AutoMapper;
using Azure.Core;
using STEM_ROBOT.BLL.Mapper;
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
    public class CompetitionSvc
    {
        private readonly CompetitionRepo _competitionRepo;
        private readonly IMapper _mapper;
        private readonly TeamRepo _teamRepo;
        private readonly TeamTableRepo _teamTableRepo;
        private readonly StageRepo _stageRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly MatchRepo _matchRepo;
        private readonly TeamMatchRepo _teamMatchRepo;
        public CompetitionSvc(CompetitionRepo competitionRepo, IMapper mapper, TeamRepo teamRepo, TeamTableRepo teamTableRepo, StageRepo stageRepo, TableGroupRepo tableGroupRepo, MatchRepo matchRepo, TeamMatchRepo teamMatchRepo)
        {
            _competitionRepo = competitionRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _teamTableRepo = teamTableRepo;
            _stageRepo = stageRepo;
            _tableGroupRepo = tableGroupRepo;
            _matchRepo = matchRepo;
            _teamMatchRepo = teamMatchRepo;
        }

        public async Task<MutipleRsp> GetListCompetitions()
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _competitionRepo.getListCompetition();
                if (list == null || !list.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var mappedList = _mapper.Map<IEnumerable<CompetitionRep>>(list);
                res.SetData("OK", mappedList);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public async Task<MutipleRsp> GetIDCompetitions(int id)
        {
            var res = new MutipleRsp();
            try
            {
                var competition = await _competitionRepo.getListCompetitionbyID(id);
                if (competition == null)
                {
                    res.SetError("No Data");
                    return res;
                }
                var mappedCompetition = _mapper.Map<CompetitionRep>(competition);
                res.SetData("OK", mappedCompetition);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> getListScoreCompetion(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                
                var list_score = await _competitionRepo.getListScoreCompetition(competitionId);
                if (list_score == null) throw new Exception("No data");


                //var mapper = _mapper.Map<List<CompetionCore>>(list_score);
                res.SetData("data", list_score);
            }
            catch (Exception ex)
            {
                throw new Exception("No data");
            }
            return res;
        }
        //listteam play
        public async Task<MutipleRsp> getlistTeamplay(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var competitons = await _competitionRepo.getListPlayer(competitionId);

                if (competitons == null) throw new Exception("No data");



                res.SetData("data", competitons);
            }
            catch (Exception ex)
            {
                throw new Exception("No data");
            }
            return res;
        }
        public async Task<SingleRsp> GetCompetitionInfor(int id)
        {
            var res = new SingleRsp();
            try
            {
                var competition = await _competitionRepo.getCompetition(id);

                if (competition == null)
                {
                    res.SetError("404", "Competition not found with the provided ID.");
                    return res;
                }

                if (competition.Tournament == null || competition.Genre == null)
                {
                    res.SetError("400", "Competition is missing related Tournament or Genre information.");
                    return res;
                }
              

                var competitionRsp = _mapper.Map<CompetitionInforRsp>(competition);

                res.setData("data", competitionRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", "Failed to retrieve competition data.");
                throw new Exception("Failed to retrieve competition data", ex);
            }
            return res;
        }
        public async Task<MutipleRsp> getCompetitionWithIDTournament(int IdTournament)
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _competitionRepo.getListCompetitionGener(IdTournament);
                if (list == null) throw new Exception("no data");

                var mapper = _mapper.Map<List<ListCompetiton>>(list);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception("No data");
            }
            return res;
        }
        public SingleRsp AddCompetion(CompetitionReq request)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _mapper.Map<Competition>(request);
                if (competition == null)
                {
                    res.SetError("Please check again!");
                    return res;
                }
                _competitionRepo.Add(competition);
                res.setData("OK", competition);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateCompetition(int id, CompetitionReq request)
        {
            var res = new SingleRsp();
            try
            {
                var competion = _competitionRepo.GetById(id);


                if (competion == null)
                {
                    res.SetError("Please check ID again!");
                    return res;
                }
                var mapper = _mapper.Map(request, competion);
                _competitionRepo.Update(mapper);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp DeleteCompetition(int id)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.GetById(id);
                if (competition == null)
                {
                    res.SetError("Please check ID again!");
                    return res;
                }
                _competitionRepo.Delete(competition.Id);
                res.setData("OK", competition);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        //update competitionformatconfig
        public async Task<SingleRsp> UpdateCompetitionConfig(CompetitionConfigReq request)
        {
            var res = new SingleRsp();
            try
            {

                var id = _competitionRepo.GetById(request.Id);

                if (id == null)
                {
                    res.SetError("No ID");
                }
                var map = _mapper.Map(request, id);
                _competitionRepo.Update(map);
                if (request.FormatId == 2)
                {
                    int teamCount = (int)request.NumberTeam;
                    var checkBool = await StateSetup(teamCount, request.Id);

                    if (checkBool == false)
                    {
                        throw new Exception("Tạo vòng đấu thất bại !!");
                    }

                }

                res.Setmessage("Ok");


            }
            catch (Exception ex)
            {
                throw new Exception("Fail Loại trực tiếp");
            }
            return res;

        }

        public SingleRsp CreateCompetitionFormatTable(int competitionId, CompetitionFormatTableReq request)

        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.Find(c => c.Id == competitionId).FirstOrDefault();
                if (competition == null)
                {
                    res.SetError("Competition not found with the provided ID.");
                    return res;
                }
                var competitionFormat = _mapper.Map(request, competition);
                _competitionRepo.Update(competitionFormat);
                if (competitionFormat.FormatId == 2)
                {
                    // Create teams
                    CreateTeams(competition.Id, request.NumberTeam);

                    // Create stages
                    List<string> stages = CalculateStages(request.NumberTeam, request.NumberTeamNextRound);
                    CreateStages(competition.Id, stages);

                    // Get stage table(Vòng bảng)
                    var stageTable = GetStageTableInCompetition(competition.Id);
                    if (stageTable == null)
                    {
                        res.SetError("Stage table not found for the given competition ID.");
                        return res;
                    }

                    // Create tables
                    CreateTables(stageTable.Id, request.NumberTable);

                    // Create matches
                    CreateMatches(competition.Id, request.NumberTeam, request.NumberTable, request.NumberTeamNextRound);

                }

                res.setData("200", "Success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        private List<string> CalculateStages(int numberTeam, int numberTeamNextRound)
        {
            var stages = new List<string> { "Vòng bảng" };
            if (numberTeamNextRound >= 64) stages.Add("Vòng 1/32");
            if (numberTeamNextRound >= 32) stages.Add("Vòng 1/16");
            if (numberTeamNextRound >= 16) stages.Add("Vòng 1/8");
            if (numberTeamNextRound >= 8) stages.Add("Tứ kết");
            if (numberTeamNextRound >= 4) stages.Add("Bán kết");
            if (numberTeamNextRound >= 2) stages.Add("Chung kết");
            return stages;
        }
        // Hàm tính toán số lượng trận đấu cần có trong giải đấu
        public SingleRsp CalculateTotalMatches(int numberOfTeams, int numberOfGroups, int numberTeamsNextRound)
        {
            var res = new SingleRsp();
            try
            {
                int totalMatches = 0;

                // Step 1: Calculate matches for each group (group stage)
                int teamsPerGroup = numberOfTeams / numberOfGroups;
                int extraTeams = numberOfTeams % numberOfGroups; // To handle cases where teams don't divide evenly

                for (int i = 0; i < numberOfGroups; i++)
                {
                    // If there are extra teams, add one more team to this group
                    int teamsInThisGroup = (i < extraTeams) ? teamsPerGroup + 1 : teamsPerGroup;

                    // Calculate round-robin matches for this group
                    int groupMatches = (teamsInThisGroup * (teamsInThisGroup - 1)) / 2;
                    totalMatches += groupMatches;
                }

                // Step 2: Calculate matches for the knockout stage
                // In knockout rounds, we need (numberTeamsNextRound - 1) matches to determine a winner
                int knockoutMatches = numberTeamsNextRound - 1;
                totalMatches += knockoutMatches;
                res.setData("200", totalMatches);
            }
            catch (Exception ex)
            {
                res.SetError("400", ex.Message);
            }
            return res;
        }

        public MutipleRsp CreateStages(int competitionId, List<string> stageNames)
        {
            var res = new MutipleRsp();
            try
            {
                var stages = stageNames.Select(stageName => new Stage
                {
                    CompetitionId = competitionId,
                    Name = stageName,
                    Status = "Pending",
                    StartDate = null,
                    EndDate = null
                }).ToList();

                foreach (var stage in stages)
                {
                    _stageRepo.Add(stage);
                }
                res.SetData("200", stages);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }

        private async Task<bool> StateSetup(int number, int competitionId)
        {
            if (number <= 0)
            {
                throw new Exception("Number must be greater than 0");
            }

            // Thêm đội vào giải đấu
            for (int i = 1; i <= number; i++)
            {
                var team = new Team()
                {
                    CompetitionId = competitionId,
                    Name = $"Đội #{i}",
                    Image = "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png",
                };
                _teamRepo.Add(team);
            }

            List<Team> teams = await _competitionRepo.GetTeamsByCompetitionId(competitionId);
            if (teams == null || teams.Count == 0) throw new Exception("No team found for competition.");

            teams = teams.OrderBy(t => t.Id).ToList();


            bool isPowerOf2 = (number & (number - 1)) == 0;
            int round = (int)Math.Ceiling(Math.Log2(number));
            int closestPowerOf2 = (int)Math.Pow(2, round);

            int extraTeams = number - (closestPowerOf2 / 2);

            List<Team> winningTeamsFromExtraRound = new List<Team>();


            if (!isPowerOf2 && extraTeams > 0)
            {
                string roundName = $"Vòng phụ cho vòng 1/{Math.Pow(2, round)}";
                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(2),
                    Status = "Vòng phụ"
                };
                _stageRepo.Add(stage);


                var teamsForExtraRound = teams.Take(extraTeams * 2).ToList();


                for (int i = 0; i < teamsForExtraRound.Count; i += 2)
                {
                    var match = new Match
                    {
                        StageId = stage.Id,
                        TableId = null,
                        StartDate = DateTime.Now,
                        Status = "Đấu phụ",
                        TimeIn = TimeSpan.Zero,
                        TimeOut = TimeSpan.Zero
                    };

                    _matchRepo.Add(match);


                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teamsForExtraRound[i].Id, NameDefault = teamsForExtraRound[i].Name });
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teamsForExtraRound[i + 1].Id, NameDefault = teamsForExtraRound[i + 1].Name });


                    winningTeamsFromExtraRound.Add(new Team
                    {
                        Id = teamsForExtraRound[i].Id,
                        Name = $"W#{i / 2 + 1} {roundName}"
                    });
                }


                teams = teams.Except(teamsForExtraRound).ToList();
            }


            teams.AddRange(winningTeamsFromExtraRound);


            for (int i = round; i > 0; i--)
            {
                string roundName;
                if (i == 1)
                    roundName = "Chung Kết";
                else if (i == 2)
                    roundName = "Bán Kết";
                else if (i == 3)
                    roundName = "Tứ Kết";
                else
                    roundName = $"Vòng 1/{Math.Pow(2, i-1)}";

                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    StageCheck = "Vòng chính",
                    StageMode = "Knockout"
                    
                };
                _stageRepo.Add(stage);


                List<Team> teamsInCurrentRound = new List<Team>();
                for (int j = 0; j < teams.Count - 1; j += 2)
                {
                    var match = new Match
                    {
                        StageId = stage.Id,
                        TableId = null,
                        StartDate = DateTime.Now,
                        Status = "Loại trực tiếp",
                        TimeIn = TimeSpan.Zero,
                        TimeOut = TimeSpan.Zero
                    };

                    _matchRepo.Add(match);


                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teams[j].Id, NameDefault = teams[j].Name });
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teams[j + 1].Id, NameDefault = teams[j + 1].Name });


                    teamsInCurrentRound.Add(new Team
                    {
                        Id = teams[j].Id,
                        Name = $"W#{j / 2 + 1} {roundName}"
                    });
                }


                teams = teamsInCurrentRound.ToList();
            }

            return true;
        }

        public int CalculateNumberStage(int numberTeam, int numberTable)
        {
            return (int)Math.Log2(numberTeam / numberTable) + 2;
        }
        public StageRep GetStageTableInCompetition(int competitionId)
        {
            var stage = _stageRepo.All().FirstOrDefault(s => s.CompetitionId == competitionId && s.Name == "Vòng bảng");
            if (stage == null)
            {
                return null;
            }

            return _mapper.Map<StageRep>(stage);
        }

        public MutipleRsp CreateTables(int stageId, int numberTable)
        {
            var res = new MutipleRsp();
            try
            {
                var createdTables = new List<TableGroup>();
                for (int i = 0; i < numberTable; i++)
                {
                    var table = new TableGroup
                    {
                        StageId = stageId,
                        Name = ((char)('A' + i)).ToString(),
                        IsAsign = false
                    };
                    _tableGroupRepo.Add(table);
                    createdTables.Add(table);
                }
                res.SetData("200", createdTables);
                return res;
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }


        private void CreateTeams(int competitionId, int numberOfTeams)
        {
            for (int i = 0; i < numberOfTeams; i++)
            {
                var team = new Team
                {
                    CompetitionId = competitionId,
                    Name = "Team " + (i + 1),
                    Image = "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png",
                };
                _teamRepo.Add(team);
            }
        }
        public int CalculateTotalMatches(int competitionId, int numberTeams, int numberTables, int numberTeamsNextRound)
        {
            int totalMatches = 0;

            // Group Stage Matches
            int totalGroupStageMatches = 0;
            var tables = _tableGroupRepo.All().Where(t => t.StageId == GetStageTableInCompetition(competitionId).Id).ToList();
            foreach (var table in tables)
            {
                var teamsInTable = _teamTableRepo.All().Where(tt => tt.TableGroupId == table.Id).ToList();
                int numberTeamsInTable = teamsInTable.Count;
                int numberMatchesInTable = numberTeamsInTable * (numberTeamsInTable - 1) / 2;
                totalGroupStageMatches += numberMatchesInTable;
            }
            totalMatches += totalGroupStageMatches;

            // Knockout Stage Matches
            int totalKnockoutMatches = numberTeamsNextRound - 1;
            totalMatches += totalKnockoutMatches;

            return totalMatches;
        }

        public void CreateMatches(int competitionId, int numberTeams, int numberTables, int numberTeamsNextRound)
        {
            try
            {
                int totalMatches = 0; // To keep track of the total number of matches

                // Step 1: Group Stage Matches
                var groupStage = GetStageTableInCompetition(competitionId);
                if (groupStage == null)
                {
                    throw new Exception("Không tìm thấy vòng bảng.");
                }

                var tables = _tableGroupRepo.All().Where(t => t.StageId == groupStage.Id).ToList();
                if (tables == null || !tables.Any())
                {
                    throw new Exception("Không tìm thấy bảng đấu.");
                }

                int teamsPerTable = numberTeams / numberTables;
                int extraTeams = numberTeams % numberTables; // For uneven distribution

                foreach (var table in tables)
                {
                    // Calculate the number of teams in the current table
                    int teamsInThisTable = teamsPerTable + (extraTeams-- > 0 ? 1 : 0);

                    // Round-robin matches for this table
                    int numberMatchesInTable = teamsInThisTable * (teamsInThisTable - 1) / 2;
                    totalMatches += numberMatchesInTable;

                    // Create the round-robin matches for this table
                    for (int i = 0; i < numberMatchesInTable; i++)
                    {
                        TimeSpan timeIn = new TimeSpan(9, 0, 0).Add(TimeSpan.FromMinutes(i * 30));

                        var match = new Match
                        {
                            StageId = groupStage.Id,
                            TableId = table.Id,
                            StartDate = DateTime.Now,
                            Status = "Pending",
                            TimeIn = timeIn
                        };

                        _matchRepo.Add(match);
                    }
                }

                // Step 2: Knockout Stage Matches
                var knockoutStages = _stageRepo.All()
                    .Where(s => s.CompetitionId == competitionId && s.Name != "Vòng bảng")
                    .OrderBy(s => s.Id)
                    .ToList();

                int remainingTeams = numberTeamsNextRound;

                foreach (var stage in knockoutStages)
                {
                    int matchesInThisStage = remainingTeams / 2;
                    if (matchesInThisStage <= 0)
                        break;

                    totalMatches += matchesInThisStage;

                    // Create matches for the knockout stage
                    for (int i = 0; i < matchesInThisStage; i++)
                    {
                        var match = new Match
                        {
                            StageId = stage.Id,
                            TableId = null,
                            StartDate = DateTime.Now.AddDays(1),
                            Status = "Pending"
                        };
                        _matchRepo.Add(match);
                    }

                    remainingTeams /= 2; // Halve the number of teams as they advance to the next stage
                }

                // Output the total matches for verification
                Console.WriteLine($"Total Matches Created: {totalMatches}");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo các trận đấu: " + ex.Message);
            }
        }




    }
}

