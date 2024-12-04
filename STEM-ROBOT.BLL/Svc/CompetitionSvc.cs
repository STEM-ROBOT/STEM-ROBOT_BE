using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities.IO;
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
        private readonly StageTableRepo _stageTableRepo;
        public CompetitionSvc(CompetitionRepo competitionRepo, IMapper mapper, TeamRepo teamRepo, TeamTableRepo teamTableRepo, StageRepo stageRepo, TableGroupRepo tableGroupRepo, MatchRepo matchRepo, TeamMatchRepo teamMatchRepo, StageTableRepo stageTableRepo)
        {
            _stageTableRepo = stageTableRepo;
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
                if (list != null)
                {
                    var mapper = _mapper.Map<List<ListCompetiton>>(list);
                    res.SetData("data", mapper);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public async Task<SingleRsp> GetByToutnamentAdhesionId(int userId,int tournamentId)
        {
            var res = new SingleRsp();
            try
            {
                var list = await _competitionRepo.getListCompetitionAdhesion(userId, tournamentId);
                if (list != null)
                {
                    var mapper = _mapper.Map<List<ListCompetiton>>(list);
                    res.setData("data", mapper);
                }

                //if (competition == null)
                //{
                //    res.SetError("Please check again!");
                //    return res;
                //}
                //_competitionRepo.Add(competition);
                //res.setData("OK", competition);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> getlistTeamAdhesionplay(int useId,int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competitons = await _competitionRepo.getListPlayerAdhesion(useId,competitionId);

                if (competitons == null) throw new Exception("No data");



                res.setData("data", competitons);
            }
            catch (Exception ex)
            {
                throw new Exception("No data");
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
                var competition = await _competitionRepo.getGenerCompetitionID(CompetitionID);
                if (competition == null)
                {
                    throw new Exception("No data");
                }
                var competitionRps = new GenerCompetitonID
                {
                    id = CompetitionID,
                    image = competition.Genre.Image,
                    name = competition.Genre.Name,
                    numberContestantTeam = (int)competition.NumberContestantTeam,
                    registerTime = competition.Status == "Public" ? (DateTime)competition.RegisterTime : null,
                    EndTime = (DateTime)competition.EndTime,
                    StartTime = (DateTime)competition.StartTime,
                    status = competition.Status,
                    FormatId = competition.Format.Id,
                    AreaCode = competition.Tournament.AreaCode,
                    Introduce = competition.Tournament.Introduce,
                    ProvinceCode = competition.Tournament.ProvinceCode,
                    TournamentLevel = competition.Tournament.TournamentLevel,

                };


                res.setData("data", competitionRps);
            }
            catch (Exception ex)
            {
                throw new Exception("Fail getGenerCompetitionID ");
            }
            return res;
        }
        //update competitionformatconfig
        public async Task<SingleRsp> UpdateCompetitionConfig(int competitionId, CompetitionConfigFormatReq request)
        {
            var res = new SingleRsp();
            try
            {

                var competition_data = _competitionRepo.GetById(competitionId);

                if (competition_data == null)
                {
                    res.SetError("No ID");
                }
                _mapper.Map(request, competition_data);
                competition_data.IsFormat = true;

                _competitionRepo.Update(competition_data);

                if (request.FormatId == 1)
                {
                    int teamCount = (int)request.NumberTeam;

                    var create_team = await TeamsSetup(teamCount, competitionId);

                    if (create_team == true)
                    {
                        List<TeamMatch> winningTeamsFromExtraRound = new List<TeamMatch>();
                        for (int i = 0; i < teamCount; i++)
                        {
                            TeamMatch team = new TeamMatch();
                            team.TotalScore = 0;
                            winningTeamsFromExtraRound.Add(team);
                        }
                        var checkBool = await StateSetup(teamCount, competitionId, winningTeamsFromExtraRound);

                        if (checkBool == false)
                        {
                            throw new Exception("Tạo vòng đấu thất bại !!");
                        }
                        res.Setmessage("Ok");
                    }
                    else
                    {
                        throw new Exception("Tạo vòng đội thất bại !!");
                    }

                }
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Tạo vòng đội thất bại !!");
            }

        }
        public SingleRsp UpdateCompetitionFormatTable(int competitionId, CompetitionConfigFormatReq request)

        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                competition.IsFormat = true;
                if (competition == null)
                {
                    res.SetError("Competition not found with the provided ID.");
                    return res;
                }
                _mapper.Map(request, competition);
                _competitionRepo.Update(competition);

                if (competition.FormatId == 2)
                {

                    // Create teams
                    CreateTeams(competitionId, (int)request.NumberTeam);

                    // Create tables
                    CreateTables((int)request.NumberTable, competitionId);
                }

                res.setData("data", "Success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        private async Task<bool> TeamsSetup(int number, int competitionId)
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
                    1 => "CK",
                    2 => "BK",
                    3 => "TK",
                    _ => $"1/{Math.Pow(2, round - 1)}"
                };

                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    Status = "Vòng phụ"
                };
                _stageRepo.Add(stage);

                // so doi o vong dư
                var teamsForExtraRound = extraTeams * 2;

                // đánh số thứ tự trận win
                var winMatchNumber = 1;
                List<TeamMatch> teamNewList = new List<TeamMatch>();
                for (int i = 0; i < teamsForExtraRound; i += 2)
                {
                    Random randomCode = new Random();
                    int randomCodes = randomCode.Next(100000, 1000000);

                    var match = new Match
                    {
                        StageId = stage.Id,
                        Status = "Đấu phụ",
                        MatchCode = randomCodes.ToString()
                    };

                    _matchRepo.Add(match);

                    // Thêm các đội vào trận đấu
                    teamNewList.Add(new TeamMatch { MatchId = match.Id, TotalScore = 0 });
                    teamNewList.Add(new TeamMatch { MatchId = match.Id, TotalScore = 0 });

                    // Giả sử đội đầu tiên thắng (có thể cập nhật khi có thông tin thực tế)

                    TeamMatch teamNew = new TeamMatch
                    {
                        NameDefault = $"W#{winMatchNumber} {roundName}",
                        MatchWinCode = match.MatchCode,
                        TotalScore = 0
                    };


                    winningTeamsFromExtraRound.Add(teamNew);
                    winMatchNumber++;
                    index += 2;

                }
                round -= 1;
                _teamMatchRepo.AddRange(teamNewList);
            }
          
            // Main rounds
            for (int currentRound = round; currentRound > 0; currentRound--)
            {
                string roundName = currentRound switch
                {
                    1 => "CK",
                    2 => "BK",
                    3 => "TK",
                    _ => $"1/{Math.Pow(2, currentRound - 1)}"
                };

                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    StageCheck = "Vòng chính",
                    StageMode = "Knockout"
                };
                _stageRepo.Add(stage);

                // Prepare teams for the round
                // List<TeamMatch> teamsInCurrentRound = new List<TeamMatch>(winningTeamsFromExtraRound);

                var numbeMatchRounds = Math.Pow(2, currentRound);
                List<TeamMatch> teamNewMainList = new List<TeamMatch>();
                // Create matches in pairs
                for (int i = 0; i < numbeMatchRounds; i += 2)
                {
                    Random randomCode = new Random();

                    int matchCode = randomCode.Next(100000, 1000000);

                    var match = new Match
                    {
                        StageId = stage.Id,
                        Status = "Loại trực tiếp",
                        MatchCode = matchCode.ToString()
                    };
                    _matchRepo.Add(match);

                    if (currentRound == round && extraTeams == 0)
                    {
                        teamNewMainList.Add(new TeamMatch { MatchId = match.Id, TotalScore = 0 });
                        teamNewMainList.Add(new TeamMatch { MatchId = match.Id, TotalScore = 0 });
                    }
                    else
                    {
                        teamNewMainList.Add(new TeamMatch { MatchId = match.Id, TeamId = null, NameDefault = winningTeamsFromExtraRound[index].NameDefault, MatchWinCode = winningTeamsFromExtraRound[index].MatchWinCode, TotalScore = 0 });
                        teamNewMainList.Add(new TeamMatch { MatchId = match.Id, TeamId = null, NameDefault = winningTeamsFromExtraRound[index + 1].NameDefault, MatchWinCode = winningTeamsFromExtraRound[index + 1].MatchWinCode, TotalScore = 0 });

                    }

                    index += 2;
                    winningTeamsFromExtraRound.Add(new TeamMatch { NameDefault = $"W#{i / 2 + 1} {roundName}", MatchWinCode = match.MatchCode, TotalScore = 0 });
                }
                _teamMatchRepo.AddRange(teamNewMainList);
            }

            return true;
        }

        //danh sach tran dau vòng loạicua competition
        public async Task<SingleRsp> matchScheduleCompetition(int competitionId)
        {
            var res = new SingleRsp();
            var stage = await _stageRepo.GetAllStagesCompetition(competitionId);
            try
            {
                var matchSchedule = stage.Select(s => new MatchScheduleCompetition
                {
                    round = s.Name,
                    matches = s.Matches.Select(m => new MatchRoundViewRsp
                    {
                        matchId = m.Id,
                        //team tham gia

                        homeTeam = m.TeamMatches.Select(tm => tm.TeamId == null ? tm.NameDefault : tm.Team.Name).FirstOrDefault(),
                        awayTeam = m.TeamMatches.Select(tm => tm.TeamId == null ? tm.NameDefault : tm.Team.Name).LastOrDefault(),
                        homeTeamLogo = m.TeamMatches.Select(tm => tm.TeamId == null ? "https://firebasestorage.googleapis.com/v0/b/fine-acronym-438603-m5.firebasestorage.app/o/stem-sever%2Flogo-dask.png?alt=media&token=f1ac1eeb-4acc-402e-b11b-080f442d55bf" : tm.Team.Image).FirstOrDefault(),
                        awayTeamLogo = m.TeamMatches.Select(tm => tm.TeamId == null ? "https://firebasestorage.googleapis.com/v0/b/fine-acronym-438603-m5.firebasestorage.app/o/stem-sever%2Flogo-dask.png?alt=media&token=f1ac1eeb-4acc-402e-b11b-080f442d55bf" : tm.Team.Image).LastOrDefault(),
                        //ti so tran dau
                        homeScore = m.TeamMatches.Select(tm => tm.TotalScore).FirstOrDefault(),
                        awayScore = m.TeamMatches.Select(tm => tm.TotalScore).LastOrDefault(),
                        //thoi gian, dia diem   
                        //thoi gian, dia diem   
                        startTime = m.StartDate + m.TimeIn,
                        locationName = m.LocationId == null ? "" : m.Location.Address,
                    }).ToList()

                }).ToList();

                res.setData("data", matchSchedule);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //danh sach tran dau vòng bảng cua competition
        public async Task<SingleRsp> matchGroupStageCompetition(int competitionId)
        {
            var res = new SingleRsp();
            var group = await _stageRepo.GetAllGroupStageCompetition(competitionId);
            var locaions = await _stageRepo.GetAllLocationCompetition(competitionId);
            try
            {
                var matchGroup = group.Select(tb => new MatchGroupStageCompetition
                {
                    groupName = tb.Name,
                    round = tb.StageTables.Select(st => new MatchGroupStageRound
                    {
                        roundNumber = st.Stage.Name,
                        matches = st.Stage.Matches.Where(m => m.TableGroupId == tb.Id).Select(m => new MatchRoundViewRsp
                        {
                            matchId = m.Id,
                            //team tham gia

                            homeTeam = m.TeamMatches.Select(tm => tm.TeamId == null ? tm.NameDefault : tm.Team.Name).FirstOrDefault(),
                            awayTeam = m.TeamMatches.Select(tm => tm.TeamId == null ? tm.NameDefault : tm.Team.Name).LastOrDefault(),
                            homeTeamLogo = m.TeamMatches.Select(tm => tm.TeamId == null ? "https://firebasestorage.googleapis.com/v0/b/fine-acronym-438603-m5.firebasestorage.app/o/stem-sever%2Flogo-dask.png?alt=media&token=f1ac1eeb-4acc-402e-b11b-080f442d55bf" : tm.Team.Image).FirstOrDefault(),
                            awayTeamLogo = m.TeamMatches.Select(tm => tm.TeamId == null ? "https://firebasestorage.googleapis.com/v0/b/fine-acronym-438603-m5.firebasestorage.app/o/stem-sever%2Flogo-dask.png?alt=media&token=f1ac1eeb-4acc-402e-b11b-080f442d55bf" : tm.Team.Image).LastOrDefault(),
                            //ti so tran dau
                            homeScore = m.TeamMatches.Select(tm => tm.TotalScore).FirstOrDefault(),
                            awayScore = m.TeamMatches.Select(tm => tm.TotalScore).LastOrDefault(),
                            //thoi gian, dia diem   
                            startTime = m.StartDate + m.TimeIn,
                            locationName = m.LocationId == null ? "" : locaions.Where(l => l.Id == m.LocationId).FirstOrDefault().Address,
                        }).ToList()
                    }).ToList(),


                }).ToList();

                res.setData("data", matchGroup);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SingleRsp getActiveCompetition(int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.All(x => x.Id == competitionId).FirstOrDefault();
                if (competition != null)
                {
                    var resData = new ActiveCompetitionRsp();
                    resData.isFormat = competition.IsFormat;
                    resData.isLocation = competition.IsLocation;
                    resData.isReferee = competition.IsReferee;
                    resData.isTeam = competition.IsTeam;
                    resData.isTable = competition.IsTable;
                    resData.isMatch = competition.IsMacth;
                    resData.isTeamMatch = (bool)competition.IsTeamMacth;
                    resData.isSchedule = competition.IsSchedule;
                    resData.formatId = competition.FormatId;
                    res.setData("data", resData);
                }
                else
                {
                    res.SetError("Competition not found.");
                }
            }
            catch (Exception ex)
            {
                res.SetError($"An error occurred: {ex.Message}");
            }

            return res;
        }

        public async Task<SingleRsp> AssignTeamsToTables(int competitionId, TableAssignmentReq tableAssignments)
        {
            var res = new SingleRsp();
            try
            {
                // Validate that table assignments are provided
                if (tableAssignments == null || !tableAssignments.tableAssign.Any() || competitionId == 0)
                {
                    throw new Exception("Không có thông tin bảng đấu hoặc đội.");
                }
                // tong so vong dau vong dau bang
                int totalStage = 0;
                // Iterate through each table assignment provided by the frontend
                //2
                var tableGruoups = new List<TableGroup>();
                var teamTables = new List<TeamTable>();
                foreach (var assignment in tableAssignments.tableAssign)
                {
                    var tableGruoup = _tableGroupRepo.GetById(assignment.TableGroupId);
                    tableGruoup.TeamNextRoud = assignment.TeamNextRound;
                    tableGruoups.Add(tableGruoup);
                    //tổng trận trong bảng
                    var numMatchInTable = (assignment.Teams.Count * (assignment.Teams.Count - 1)) / 2;
                    // số trận trong vòng
                    var numberMatchInStage = Math.Ceiling((decimal)numMatchInTable / assignment.Teams.Count);
                    //số vòng mỗi bảng
                    var numberStageInTable = Math.Ceiling((decimal)numMatchInTable / numberMatchInStage);
                    if (numberStageInTable > totalStage)
                    {
                        totalStage = (int)numberStageInTable;
                    }
                    // Validate table ID and team list
                    if (assignment.TableGroupId <= 0 || assignment.Teams == null || !assignment.Teams.Any())
                    {
                        throw new Exception("Thông tin bảng hoặc danh sách đội không hợp lệ.");
                    }

                    // Add each team in the team list to the specified table
                    foreach (var teamId in assignment.Teams)
                    {
                        var teamTable = new TeamTable
                        {
                            TeamId = teamId,
                            TableGroupId = assignment.TableGroupId,
                            IsSetup = true // Assuming this should be true when teams are assigned
                        };
                        teamTables.Add(teamTable);
                    }
                }
                _teamTableRepo.AddRange(teamTables);
                _tableGroupRepo.UpdateRange(tableGruoups);
                var checksetup = await SetupStageTable(totalStage, competitionId, tableAssignments);

                var checkBool = await StateSetup((int)checksetup.Count, competitionId, checksetup);
                if (checkBool == false)
                {
                    throw new Exception("Tạo tran đấu thất bại !!");
                }
                var competition = _competitionRepo.GetById(competitionId);
                competition.IsTable = true;
                _competitionRepo.Update(competition);
                res.setData("data", "Success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<List<TeamMatch>> SetupStageTable(int totalStage, int competitionId, TableAssignmentReq tableAssignments)
        {

            var stages = new List<Stage>();
            // tao so vong dau bang
            for (int i = 0; i < totalStage; i++)
            {
                var stage = new Stage
                {
                    Name = $"{i + 1}",
                    Status = "Pending",
                    StartDate = null,
                    EndDate = null,
                    StageMode = "Vòng bảng",
                    CompetitionId = competitionId,
                };

                stages.Add(stage);
            }
            _stageRepo.AddRange(stages);


            //tao so vong dau cua moi bang 
            List<StageTable> tables = new List<StageTable>();
            List<TeamMatch> teamMatchs = new List<TeamMatch>();
            List<TeamMatch> winningTeamsFromExtraRound = new List<TeamMatch>();
            //2
            foreach (var assignment in tableAssignments.tableAssign)
            {
                //tổng trận trong bảng
                var numMatchInTable = (assignment.Teams.Count * (assignment.Teams.Count - 1)) / 2;
                // số trận trong vòng
                var numberMatchInStage = Math.Ceiling((decimal)numMatchInTable / assignment.Teams.Count);
                //số vòng mỗi bảng
                var numberStageInTable = Math.Ceiling((decimal)numMatchInTable / numberMatchInStage);
                //lien ket vong voi bang
                //5
                var stage_table = 1;
                var mactch_stage = 1;

                for (int i = 0; i < assignment.TeamNextRound; i++)
                {

                    TeamMatch team = new TeamMatch
                    {
                        NameDefault = $"Top#{i + 1} B#{assignment.TableGroupName}",
                        TotalScore = 0,
                        MatchWinCode = $"T#{i + 1}B#{assignment.TableGroupName}"
                    };
                    winningTeamsFromExtraRound.Add(team);
                }

                foreach (var stage in stages)
                {
                    if (stage_table <= numberStageInTable)
                    {
                        var stageTable = new StageTable
                        {
                            StageId = stage.Id,
                            TableGroupId = assignment.TableGroupId,
                        };
                        tables.Add(stageTable);
                        for (int j = 0; j < numberMatchInStage; j++)
                        {
                            if (mactch_stage <= numMatchInTable)
                            {
                                var match = new Match
                                {
                                    StageId = stage.Id,
                                    IsSetup = false,
                                    Status = stage.Name + assignment.TableGroupName,
                                    TableGroupId = assignment.TableGroupId,

                                };
                                _matchRepo.Add(match);
                                teamMatchs.Add(new TeamMatch { MatchId = match.Id, TotalScore = 0 });
                                teamMatchs.Add(new TeamMatch { MatchId = match.Id, TotalScore = 0 });
                                mactch_stage++;
                            }

                        }
                        stage_table++;
                    }

                }

            }
            _stageTableRepo.AddRange(tables);
            _teamMatchRepo.AddRange(teamMatchs);
            return winningTeamsFromExtraRound;
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
                res.setData("data", totalMatches);
            }
            catch (Exception ex)
            {
                res.SetError("400", ex.Message);
            }
            return res;
        }


        public MutipleRsp CreateTables(int numberTable, int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var createdTables = new List<TableGroup>();
                for (int i = 0; i < numberTable; i++)
                {
                    var table = new TableGroup
                    {
                        Name = ((char)('A' + i)).ToString(),
                        IsAsign = false,
                        CompetitionId = competitionId
                    };
                    _tableGroupRepo.Add(table);
                    createdTables.Add(table);
                }
                res.SetData("data", createdTables);
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
                competition.IsRule = true;
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

        public async Task<SingleRsp> GetDataToAssign(int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var lstTeamRsp = _teamRepo.All().Where(t => t.CompetitionId == competitionId).Select(t => new DataTeamRsp
                {
                    TeamId = t.Id,
                    TeamName = t.Name
                }).ToList();

                var competitition = _competitionRepo.GetById(competitionId);
                List<DataTableRsp> lstTableGroup;
                if (competitition.IsTable == false)
                {
                    lstTableGroup = _tableGroupRepo.All().Where(x => x.CompetitionId == competitionId).Select(x => new DataTableRsp
                    {
                        TableId = x.Id,
                        TableName = x.Name,
                        Teams = new List<DataTeamRsp>()
                    }).ToList();
                }
                else
                {
                    lstTableGroup = _tableGroupRepo.All().Where(x => x.CompetitionId == competitionId).Select(x => new DataTableRsp
                    {
                        TableId = x.Id,
                        TableName = x.Name,
                        IsTable = (bool)competitition.IsTable,
                        Teams = _teamTableRepo.All().Where(t => t.TableGroupId == x.Id).Select(t => new DataTeamRsp
                        {
                            TeamId = t.Id,
                            TeamName = t.Team.Name
                        }).ToList()
                    }).ToList();
                }
                var teamTableRsp = new DataAssignTeamTableRsp();
                teamTableRsp.Teams = lstTeamRsp;
                teamTableRsp.Tables = lstTableGroup;
                teamTableRsp.IsTable = (bool)competitition.IsTable;
                teamTableRsp.NumberTeamNextRound = competitition.NumberTeamNextRound;
                res.setData("data", teamTableRsp);
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return res;
        }

        public SingleRsp SetCompetitionActive(int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                if (competition == null)
                {
                    res.SetError("404", "Competition not found with the provided ID.");
                    return res;
                }
                if (competition.IsActive == true)
                {
                    res.SetError("400", "Competition is already active.");
                    return res;
                }
                if (competition.IsFormat == true && competition.IsMacth == true && competition.IsTeamMacth == true
                    && competition.IsReferee == true && competition.IsLocation == true && competition.IsSchedule == true)
                {
                    competition.IsActive = true;
                    _competitionRepo.Update(competition);
                    res.SetMessage("Competition is now active.");
                }
                else
                {
                    res.SetError("400", "Competition is missing some required information.");
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public async Task<SingleRsp> GetRuleCompetition(int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = await _competitionRepo.Rulecompetion(competitionId);

                if (competition == null)
                {
                    res.SetError("404", "Competition not found with the provided ID.");
                    return res;
                }
                var data = new
                {
                    regulation = competition.Regulation,
                    regulationExample = competition.Genre.HintRule
                };
                res.setData("file", data);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}