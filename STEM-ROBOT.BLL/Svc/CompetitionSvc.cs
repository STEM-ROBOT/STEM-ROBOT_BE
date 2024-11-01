﻿using AutoMapper;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        //competition gener
        public async Task<SingleRsp> getGenerCompetitionID(int CompetitionID)
        {
            var res = new SingleRsp();
            try
            {
                var genre = await _competitionRepo.getGenerCompetitionID(CompetitionID);
                if (genre == null)
                {
                    throw new Exception("No data");
                }
                res.setData("data", genre);
            }
            catch (Exception ex)
            {
                throw new Exception("Fail getGenerCompetitionID ");
            }
            return res;
        }
        //update competitionformatconfig
        public async Task<SingleRsp> UpdateCompetitionConfig(CompetitionConfigReq request)
        {
            var res = new SingleRsp();
            try
            {

                var competition_data = _competitionRepo.GetById(request.Id);

                if (competition_data == null)
                {
                    res.SetError("No ID");
                }
                competition_data.FormatId = request.FormatId;
                _competitionRepo.Update(competition_data);

                if (request.FormatId == 1)
                {
                    int teamCount = (int)request.NumberTeam;

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

        public SingleRsp CreateCompetitionFormatTable(CompetitionFormatTableReq request)

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
                throw new Exception("Fail Loại trực tiếp");
            }
      
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
                return false;
                throw new Exception("Number must be greater than 0");

            }
            Dictionary<int, string> originalTeamNames = new Dictionary<int, string>();
            for (int i = 1; i <= number; i++)
            {
                var team = new Team()
                {
                    CompetitionId = competitionId,
                    Name = $"Đội #{i}",
                    Image = "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png",
                };
                _teamRepo.Add(team);
                originalTeamNames[team.Id] = team.Name;
            }
            return true;
        }

        private async Task<bool> StateSetup(int number, int competitionId, List<TeamMatch> winningTeamsFromExtraRound)
        {
            // tính số vòng đấu đủ hay dư
            bool isPowerOf2 = (number & (number - 1)) == 0;
            int round = (int)Math.Ceiling(Math.Log2(number));
            int closestPowerOf2 = (int)Math.Pow(2, round);
            int extraTeams = number - (closestPowerOf2 / 2);

            // chuẩn bị danh sách team 
           
            var index = 0;

            // tạo vòng đấu cho số đội dư 
            if (!isPowerOf2 && extraTeams > 0)
            {
                string roundName = round switch
                {
                    1 => "Chung Kết",
                    2 => "Bán Kết",
                    3 => "Tứ Kết",
                    _ => $"Vòng 1/{Math.Pow(2, round - 1)}"
                };

                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(2),
                    Status = "Vòng phụ"
                };
                _stageRepo.Add(stage);

                // so doi o vong dư
                var teamsForExtraRound = extraTeams * 2;

                // đánh số thứ tự trận win
                var winMatchNumber = 1;

                for (int i = 0; i < teamsForExtraRound; i += 2)
                {
                    Random randomCode = new Random();
                    int randomCodes = randomCode.Next(100000, 1000000);

                    var match = new Match
                    {
                        StageId = stage.Id,
                        TableId = null,
                        StartDate = DateTime.Now,
                        Status = "Đấu phụ",
                        TimeIn = TimeSpan.Zero,
                        TimeOut = TimeSpan.Zero,
                        MatchCode = randomCodes.ToString()
                    };

                    _matchRepo.Add(match);

                    // Thêm các đội vào trận đấu
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id });
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id });

                    // Giả sử đội đầu tiên thắng (có thể cập nhật khi có thông tin thực tế)

                    TeamMatch teamNew = new TeamMatch
                    {
                        NameDefault = $"W#{winMatchNumber} {roundName}",
                        MatchWinCode = match.MatchCode
                    };


                    winningTeamsFromExtraRound.Add(teamNew);
                    winMatchNumber++;
                    index += 2;

                }
                round -= 1;
            }

            // Main rounds
            for (int currentRound = round; currentRound > 0; currentRound--)
            {
                string roundName = currentRound switch
                {
                    1 => "Chung Kết",
                    2 => "Bán Kết",
                    3 => "Tứ Kết",
                    _ => $"Vòng 1/{Math.Pow(2, currentRound - 1)}"
                };

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

                // Prepare teams for the round
               // List<TeamMatch> teamsInCurrentRound = new List<TeamMatch>(winningTeamsFromExtraRound);

                var numbeMatchRounds = Math.Pow(2, currentRound);
                // Create matches in pairs
                for (int i = 0; i < numbeMatchRounds; i += 2)
                {
                    Random randomCode = new Random();

                    int matchCode = randomCode.Next(100000, 1000000);

                    var match = new Match
                    {
                        StageId = stage.Id,
                        TableId = null,
                        StartDate = DateTime.Now,
                        Status = "Loại trực tiếp",
                        TimeIn = TimeSpan.Zero,
                        TimeOut = TimeSpan.Zero,
                        MatchCode = matchCode.ToString()
                    };
                    _matchRepo.Add(match);

                    if (currentRound == round && extraTeams == 0)
                    {
                        _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id });
                        _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id });
                    }
                    else
                    {
                        _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = null, NameDefault = winningTeamsFromExtraRound[index].NameDefault, MatchWinCode = winningTeamsFromExtraRound[index].MatchWinCode });
                        _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = null, NameDefault = winningTeamsFromExtraRound[index + 1].NameDefault, MatchWinCode = winningTeamsFromExtraRound[index + 1].MatchWinCode });

                    }

                    index += 2;  
                    winningTeamsFromExtraRound.Add(new TeamMatch { NameDefault = $"W#{i / 2 + 1} {roundName}", MatchWinCode = match.MatchCode });
                }
            }

            return true;
        }







        public SingleRsp CreateCompetitionFormatTable(CompetitionFormatTableReq request)

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
        // Add teams and save original names





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
                            TimeIn = timeIn, // Gán giá trị TimeSpan


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

        public async Task<SingleRsp> AddRule(string file, int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                if (competition == null)
                {
                    throw new Exception("Không tìm thấy competitionId");
                }

                competition.Regulation = file;
                _competitionRepo.Update(competition);

                res.setData("success", competition);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }


    }
}

