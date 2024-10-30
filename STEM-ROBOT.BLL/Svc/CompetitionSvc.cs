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

        public SingleRsp CreateCompetitionFormatTable(CompetitionReq request)

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

                if (request.FormatId == 1)
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

                    // Assign teams to tables
                    AssignTeamsToTables(competition.Id, stageTable.Id, request.NumberTeam, request.NumberTable);

                    // Create matches
                    CreateMatches(competition.Id, request.NumberTeam, request.NumberTable, request.NumberTeamNextRound);

                    // AssignTeamsToMatchesInStageTable
                    AssignTeamsToMatchesInStageTable(competition.Id, stageTable.Id);

                    // Assign qualified teams to next round
                    AssignTeamsToNextRoundMatches(competition.Id, request.NumberTeamNextRound);
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
        public int CalculateTotalMatches(int numberOfTeams, int numberOfTables, int numberOfQualifiedTeams)
        {
            // Số trận đấu vòng bảng
            int groupStageMatches = (numberOfTeams / numberOfTables) * (numberOfTeams / numberOfTables - 1) / 2 * numberOfTables;

            // Số trận đấu vòng loại trực tiếp
            int knockoutStageMatches = numberOfQualifiedTeams - 1;

            return groupStageMatches + knockoutStageMatches;
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

        public SingleRsp AssignTeamsToTables(int competitionId, int stageId, int numberTeams, int numberTables)
        {
            var res = new SingleRsp();
            // Lấy danh sách các đội thuộc competition
            var teams = _teamRepo.All().Where(t => t.CompetitionId == competitionId).ToList();
            if (teams == null || !teams.Any())
            {
                throw new Exception("Không tìm thấy đội nào trong cuộc thi.");
            }

            // Lấy danh sách các bảng đấu thuộc stage "Vòng bảng"
            var tableGroups = _tableGroupRepo.All().Where(b => b.StageId == stageId).ToList();
            if (tableGroups == null || !tableGroups.Any())
            {
                throw new Exception("Số lượng bảng đấu không phù hợp.");
            }

            int teamsPerTable = numberTeams / numberTables;
            int remainder = numberTeams % numberTables;

            int currentTeamIndex = 0;

            foreach (var table in tableGroups)
            {
                int currentTableTeamCount = teamsPerTable + (remainder > 0 ? 1 : 0);
                remainder--;

                for (int i = 0; i < currentTableTeamCount; i++)
                {
                    if (currentTeamIndex >= teams.Count) break;

                    var team = teams[currentTeamIndex++];
                    AddTeamToTable(team.Id, table.Id);
                }
            }
            res.setData("200", "Success");
            return res;
        }

        public void AddTeamToTable(int teamId, int tableGroupId)
        {
            var teamTable = new TeamTable
            {
                TeamId = teamId,
                TableGroupId = tableGroupId
            };
            _teamTableRepo.Add(teamTable);
        }

        private void CreateTeams(int competitionId, int numberOfTeams)
        {
            for (int i = 0; i < numberOfTeams; i++)
            {
                var team = new Team
                {
                    CompetitionId = competitionId,
                    Name = "Team " + (i + 1)
                };
                _teamRepo.Add(team);
            }
        }

        public void CreateMatches(int competitionId, int numberTeams, int numberTables, int numberTeamsNextRound)
        {
            try
            {
                // Lấy danh sách bảng đấu thuộc vòng bảng
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

                // Tính số trận vòng bảng
                foreach (var table in tables)
                {
                    var teamsInTable = _teamTableRepo.All().Where(tt => tt.TableGroupId == table.Id).ToList();
                    int numberTeamsInTable = teamsInTable.Count;

                    int numberMatchesInTable = numberTeamsInTable * (numberTeamsInTable - 1) / 2;
                    for (int i = 0; i < numberMatchesInTable; i++)
                    {
                        TimeSpan timeIn = new TimeSpan(9, 0, 0).Add(TimeSpan.FromMinutes(i * 30)); // 09:00 + 30 phút mỗi trận

                        var match = new Match
                        {
                            StageId = groupStage.Id,
                            TableId = table.Id,
                            StartDate = DateTime.Now, // Ngày giờ bắt đầu có thể thêm sau
                            Status = "Pending",
                            TimeIn = timeIn, // Gán giá trị TimeSpan
                            
                            
                        };

                        _matchRepo.Add(match);
                    }
                }


                // Tạo các trận đấu vòng loại trực tiếp
                var knockoutStages = _stageRepo.All().Where(s => s.CompetitionId == competitionId && s.Name != "Vòng bảng").OrderBy(s => s.Id).ToList();
                int knockoutStageIndex = 0;
                int remainingMatches = numberTeamsNextRound - 1;

                foreach (var stage in knockoutStages)
                {
                    int matchesInThisStage = Math.Min(remainingMatches, (int)Math.Pow(2, knockoutStages.Count - knockoutStageIndex - 1));
                    remainingMatches -= matchesInThisStage;

                    for (int i = 0; i < matchesInThisStage; i++)
                    {
                        var match = new Match
                        {
                            StageId = stage.Id,
                            TableId = null, // Vòng loại trực tiếp không có bảng
                            StartDate = DateTime.Now.AddDays(1),
                            Status = "Pending"
                        };
                        _matchRepo.Add(match);
                    }

                    knockoutStageIndex++;
                    if (remainingMatches <= 0) break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void AssignTeamsToMatchesInStageTable(int competitionId, int stageId)
        {
            try
            {
                // Lấy danh sách các bảng đấu thuộc stage "Vòng bảng"
                var tables = _tableGroupRepo.All().Where(t => t.StageId == stageId).ToList();
                if (tables == null || !tables.Any())
                {
                    throw new Exception("Không tìm thấy bảng đấu.");
                }

                // Lấy tất cả các trận đấu đã tạo cho vòng bảng
                var matches = _matchRepo.All().Where(m => m.StageId == stageId).OrderBy(m => m.Id).ToList();
                if (matches == null || matches.Count == 0)
                {
                    throw new Exception("Không tìm thấy trận đấu nào trong vòng đấu.");
                }

                int currentMatchIndex = 0;

                // Duyệt qua từng bảng đấu
                foreach (var table in tables)
                {
                    // Lấy danh sách các đội thuộc bảng đấu hiện tại
                    var teamsInTable = _teamTableRepo.All().Where(tt => tt.TableGroupId == table.Id).Select(tt => tt.TeamId).ToList();

                    if (teamsInTable.Count < 2)
                    {
                        throw new Exception("Không đủ đội để tạo trận đấu cho bảng " + table.Name);
                    }

                    // Đấu vòng tròn cho tất cả các đội trong bảng
                    for (int i = 0; i < teamsInTable.Count - 1; i++)
                    {
                        for (int j = i + 1; j < teamsInTable.Count; j++)
                        {
                            if (currentMatchIndex >= matches.Count)
                            {
                                throw new Exception("Không đủ số trận đấu đã tạo để xếp các đội.");
                            }

                            // Lấy trận đấu từ danh sách đã tạo
                            var match = matches[currentMatchIndex++];

                            // Thêm đội vào trận đấu
                            AddTeamToMatch((int)teamsInTable[i], match.Id);
                            AddTeamToMatch((int)teamsInTable[j], match.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xếp các đội vào trận đấu: {ex.Message}");
            }
        }
        // Hàm thêm đội vào trận đấu
        public void AddTeamToMatch(int teamId, int matchId)
        {
            var teamMatch = new TeamMatch
            {
                TeamId = teamId,
                MatchId = matchId,
                ResultPlay = "Pending", // Đội ban đầu chưa có kết quả

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
                    .OrderByDescending(t => t.Point)
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


        public void AssignTeamsToNextRoundMatches(int competitionId, int numberTeamsNextRound)
        {
            try
            {
                // Lấy danh sách các bảng đấu thuộc stage "Vòng bảng"
                var tables = _tableGroupRepo.All().Where(t => t.StageId == GetStageTableInCompetition(competitionId).Id).ToList();
                if (tables == null || !tables.Any())
                {
                    throw new Exception("Không tìm thấy bảng đấu.");
                }

                // Lấy các đội vượt qua vòng bảng
                var qualifiedTeams = GetQualifiedTeamsForNextRound(tables, numberTeamsNextRound);

                // Điều chỉnh số đội cho phù hợp với số lượng cần thiết (phải là số chẵn để tạo trận đấu)
                if (qualifiedTeams.Count % 2 != 0)
                {
                    qualifiedTeams.RemoveAt(qualifiedTeams.Count - 1); // Loại bỏ 1 đội nếu số lượng lẻ
                }

                // Tính toán số lượng vòng tiếp theo cần thiết
                int numberOfStages = (int)Math.Ceiling(Math.Log2(qualifiedTeams.Count));

                // Lấy danh sách các stage knockout được sắp xếp theo thứ tự
                var knockoutStages = _stageRepo.All()
                    .Where(s => s.CompetitionId == competitionId && s.Name != "Vòng bảng")
                    .OrderBy(s => s.Id)
                    .Take(numberOfStages)
                    .ToList();

                if (knockoutStages == null || knockoutStages.Count < numberOfStages)
                {
                    throw new Exception("Không đủ các vòng đấu tiếp theo để sắp xếp trận đấu.");
                }

                // Sắp xếp các trận đấu cho các vòng loại trực tiếp
                int remainingTeams = qualifiedTeams.Count;
                for (int stageIndex = 0; stageIndex < knockoutStages.Count && remainingTeams > 1; stageIndex++)
                {
                    var stage = knockoutStages[stageIndex];
                    int matchesInThisStage = remainingTeams / 2;

                    // Lấy danh sách các trận đấu của stage hiện tại
                    var matches = _matchRepo.All().Where(m => m.StageId == stage.Id).OrderBy(m => m.Id).Take(matchesInThisStage).ToList();
                    if (matches.Count < matchesInThisStage)
                    {
                        throw new Exception($"Không đủ trận đấu trong {stage.Name}.");
                    }

                    for (int i = 0; i < matchesInThisStage; i++)
                    {
                        if (stageIndex == 0)
                        {
                            // Vòng đầu tiên sau vòng bảng
                            if (i < qualifiedTeams.Count - 1)
                            {
                                AddTeamMatchWithPlaceholder(matches[i].Id, $"Nhất bảng {tables[i % tables.Count].Name}");
                                AddTeamMatchWithPlaceholder(matches[i].Id, $"Nhì bảng {tables[(i + 1) % tables.Count].Name}");
                            }
                        }
                        else
                        {
                            // Các vòng tiếp theo (bán kết, chung kết)
                            AddTeamMatchWithPlaceholder(matches[i].Id, $"Win trận {matchesInThisStage * stageIndex - matchesInThisStage + (i * 2) + 1}");
                            AddTeamMatchWithPlaceholder(matches[i].Id, $"Win trận {matchesInThisStage * stageIndex - matchesInThisStage + (i * 2) + 2}");
                        }
                    }

                    remainingTeams /= 2;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xếp các đội vào vòng tiếp theo: {ex.Message}");
            }
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

