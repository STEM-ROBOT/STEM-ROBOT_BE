using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using STEM_ROBOT.BLL.HubClient;
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
    public class MatchSvc
    {
        private readonly MatchRepo _matchRepo;
        private readonly IMapper _mapper;
        private readonly TeamTableRepo _teamTableRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly TeamRepo _teamRepo;
        private readonly CompetitionRepo _competition;
        private readonly StageRepo _stageRepo;
        private readonly StageSvc _stageSvc;
        private readonly StemHub _stemHub;
        private readonly MatchHaflRepo _matchHaflRepo;
        private readonly ActionRepo _actionRepo;
        private readonly ScheduleRepo _scheduleRepo;
        public MatchSvc(MatchRepo repo, IMapper mapper, ScheduleRepo scheduleRepo, CompetitionRepo competition, TeamTableRepo teamTableRepo, TableGroupRepo tableGroupRepo, TeamRepo teamRepo, StageRepo stageRepo, StageSvc stageSvc, StemHub stemHub, MatchHaflRepo matchHaflRepo, ActionRepo actionRepo)
        {
            _matchRepo = repo;
            _teamTableRepo = teamTableRepo;
            _mapper = mapper;
            _tableGroupRepo = tableGroupRepo;
            _teamRepo = teamRepo;
            _stageRepo = stageRepo;
            _stageSvc = stageSvc;
            _competition = competition;
            _stemHub = stemHub;
            _matchHaflRepo = matchHaflRepo;
            _actionRepo = actionRepo;
            _scheduleRepo = scheduleRepo;
        }

        public MutipleRsp GetListMatch()
        {
            var res = new MutipleRsp();
            try
            {
                var list = _matchRepo.All();
                if (list == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<MatchRep>>(list);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp getByIDMatch(int id)

        {
            var res = new SingleRsp();
            try
            {
                var list = _matchRepo.GetById(id);
                if (list == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<MatchRep>(list);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp AddMatch(MatchReq req)
        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Match>(req);
                if (mapper == null)
                {
                    res.SetError("Please check data");
                }
                _matchRepo.Add(mapper);
                res.setData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateMatch(int id, MatchReq req)
        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Match>(req);
                if (mapper == null)
                {
                    res.SetError("Please check data");
                }
                _matchRepo.Add(mapper);
                res.setData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteMatch(int id)
        {
            var res = new SingleRsp();
            try
            {
                var match = _matchRepo.GetById(id);
                if (match == null)
                {
                    res.SetError("Please check data");
                }
                _matchRepo.Delete(match.Id);
                res.setData("Ok", match);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }


        //done
        public async Task<SingleRsp> getListRound(int competitionId)
        {
            var res = new SingleRsp();

            try
            {
                var competition = await _matchRepo.GetRoundGameAsync(competitionId);
                if (competition == null) throw new Exception("No data");

                if (competition.FormatId == 2)
                {
                    var roundParent = new roundTableParentConfig
                    {
                        isMatch = (bool)competition.IsMacth,
                        startTime = competition.StartTime,
                        group = new GroupRound
                        {
                            rounds = competition.Stages.Where(st => st.StageMode == "Vòng bảng").Select(s => new RoundGroupGame
                            {
                                roundId = s.Id,
                                round = s.Name,
                                matchrounds = s.StageTables.Select(ts => new RoundGroupGameMatch
                                {
                                    tableName = ts.TableGroup.Name,
                                    matches = ts.TableGroup.Matches.Where(m => m.StageId == s.Id).Select(md => new TeamMatchRound
                                    {
                                        matchId = md.Id,
                                        teamA = md.TeamMatches.FirstOrDefault().TeamId == null ? md.TeamMatches.FirstOrDefault().NameDefault : md.TeamMatches.FirstOrDefault().Team.Name,
                                        teamB = md.TeamMatches.LastOrDefault().TeamId == null ? md.TeamMatches.LastOrDefault().NameDefault : md.TeamMatches.LastOrDefault().Team.Name,
                                        date = md.StartDate,
                                        time = md.TimeIn,
                                        locationId = md.LocationId
                                    }).ToList(),
                                }).ToList(),
                            }).ToList()

                        },
                        knockout = await getListRoundKnocOut(competitionId),
                        locations = competition.Locations.Select(l => new locationCompetitionConfig
                        {
                            locationId = l.Id,
                            locationName = l.Address

                        }).ToList(),
                    };
                    res.setData("data", roundParent);
                }
                else if (competition.FormatId == 1)
                {
                    var roundKnocOutParentConfig = new roundKnocOutParentConfig
                    {
                        startTime = competition.StartTime,
                        isMatch = (bool)competition.IsMacth,
                        knockout = await getListRoundKnocOut(competitionId),
                        locations = competition.Locations.Select(l => new locationCompetitionConfig
                        {
                            locationId = l.Id,
                            locationName = l.Address

                        }).ToList(),
                    };
                    res.setData("data", roundKnocOutParentConfig);
                }




            }
            catch
            {
                throw new Exception("Fail");
            }
            return res;
        }

        //done
        public async Task<GroupRound> getListRoundKnocOut(int competitionId)
        {
            var competition = await _matchRepo.GetRoundKnocoutGameAsync(competitionId);
            var knocout = new GroupRound
            {
                IsTeamMatch = (bool)competition.IsTeamMacth,
                rounds = competition.Stages.Where(st => st.StageMode != "Vòng bảng").Select(s => new RoundGroupGame
                {
                    roundId = s.Id,
                    round = s.Name,
                    matchrounds = new List<RoundGroupGameMatch>
                    {
                        new RoundGroupGameMatch
                        {
                        tableName = "",
                        matches = s.Matches.Select(md => new TeamMatchRound
                        {
                            matchId = md.Id,
                            teamA = md.TeamMatches.FirstOrDefault().TeamId == null ? md.TeamMatches.FirstOrDefault().NameDefault : md.TeamMatches.FirstOrDefault().Team.Name,
                            teamB = md.TeamMatches.LastOrDefault().TeamId == null ? md.TeamMatches.LastOrDefault().NameDefault : md.TeamMatches.LastOrDefault().Team.Name,
                            date =md.StartDate,
                            time=md.TimeIn,
                            locationId = md.LocationId
                        }).ToList(),
                        },

                    }
                }).ToList()


            };
            return knocout;

        }



        public async Task<MutipleRsp> getListKnockOut(int CompetitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _matchRepo.getRoundGameKnockOut(CompetitionId);
                if (list == null) throw new Exception("No data");
                res.SetData("data", list);

            }
            catch
            {
                throw new Exception("Fail");
            }
            return res;
        }

        //done
        public async Task<MutipleRsp> getListKnockOutLate(int CompetitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _matchRepo.getRoundGameKnockOut(CompetitionId);
                if (list == null) throw new Exception("No data");
                res.SetData("data", list);

            }
            catch
            {
                throw new Exception("Fail");
            }
            return res;
        }
        //done
        public async Task<SingleRsp> GetRoundParentTable(int competitionId)
        {

            var res = new SingleRsp();
            try
            {
                var list = await _matchRepo.GetRoundParentTable(competitionId);
                var table_data = await _matchRepo.GetTableTeam(competitionId);
                if (list == null) throw new Exception("No data");
                var round_table = new RoundParentTable
                {
                    isTeamMatch = list.IsTeamMacth,
                    tableGroup = table_data.Select(tg => new tableGroup
                    {
                        team_tableId = tg.Id,
                        team_table = tg.TeamTables.Select(tb => new RoundTableTeam
                        {
                            teamId = tb.TeamId,
                            teamName = tb.Team.Name
                        }).ToList()
                    }).ToList(),
                    rounds = list.Stages.Select(s => new RoundGameTable
                    {
                        roundId = s.Id,
                        roundName = s.Name,
                        tables = s.StageTables.Select(ts => new RoundTable
                        {
                            tableId = ts.TableGroupId,
                            tableName = ts.TableGroup.Name,
                            matches = ts.TableGroup.Matches.Where(m => m.StageId == s.Id).Select(m => new RoundGameMatch
                            {
                                matchId = m.Id,
                                teamMatches = m.TeamMatches.Select(tm => new RoundGameTeamMatch
                                {
                                    teamId = tm.TeamId,
                                    teamMatchId = tm.Id,
                                    teamName = tm.NameDefault

                                }).ToList()
                            }).ToList(),



                        }
                        ).ToList()
                    }).ToList()


                };

                res.setData("data", round_table);

            }
            catch
            {
                throw new Exception("Fail");
            }
            return res;
        }

        public async Task<SingleRsp> conFigTimeMtch(int competitionId, MatchConfigReq reqs)
        {
            var res = new SingleRsp();
            var competition_data = await _competition.getCompetitionMatchUpdate(competitionId);
            if (competition_data == null)
            {
                res.SetError("400");
                res.SetMessage("Nội dung thi đấu không tồn tại");
            }


            List<Match> matches = new List<Match>();

            List<MatchHalf> matchesHalf = new List<MatchHalf>();

            var endTime = new DateTime();
            //var listMatch = competition_data.Stages.Select(m => new Match
            //{

            //})
            foreach (var stage in competition_data.Stages)
            {
                foreach (var match in stage.Matches)
                {
                    var check = reqs.matchs.Where(m => m.id == match.Id).FirstOrDefault();
                    match.LocationId = check.locationId;
                    match.TimeIn = check.TimeIn;
                    match.TimeOut = check.TimeOut;
                    match.StartDate = check.startDate;
                    match.TimeOfHaft = reqs.TimeOfHaft;
                    match.NumberHaft = reqs.NumberHaft;
                    match.BreakTimeHaft = reqs.BreakTimeHaft;
                    endTime = (DateTime)check.startDate;
                    matches.Add(match);
                    for (int i = 1; i <= reqs.NumberHaft; i++)
                    {
                        var matchHalf = new MatchHalf
                        {
                            MatchId = match.Id,
                            HalfName = i.ToString(),
                        };
                        matchesHalf.Add(matchHalf);
                    }
                }

            }
            competition_data.EndTime = endTime;
            competition_data.IsMacth = true;
            competition_data.TimeBreak = reqs.TimeBreak;
            competition_data.TimeOfMatch = reqs.TimeOfMatch;
            competition_data.TimeEndPlay = reqs.TimeEndPlay;
            competition_data.TimeStartPlay = reqs.TimeStartPlay;
            _competition.Update(competition_data);
            _matchRepo.UpdateRange(matches);
            _matchHaflRepo.AddRange(matchesHalf);
            return res;
        }

        //check date
        public async Task<SingleRsp> CheckMatch(int matchID)
        {
            var time = ConvertToVietnamTime(DateTime.Now);
            var res = new SingleRsp();
            try
            {
                var timePlay = _matchRepo.GetById(matchID);
                var totalTime = timePlay.StartDate + timePlay.TimeIn;

                var checkDate = time < timePlay.StartDate;

                TimeSpan checkTime = (DateTime)totalTime - time;

                if (time.Date < timePlay.StartDate.Value.Date || checkTime.TotalMinutes > 15)
                {
                    //res.SetMessage("Trận đấu chưa diễn ra");
                    res.setData("data", "notstarted");
                }
                else
                if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes <= 15 && checkTime.TotalMinutes > 0)
                {
                    var data = new
                    {
                        TimeAwait = checkTime,
                        TimeInMatch = timePlay.TimeIn

                    };
                    res.setData("data", data);
                    return res;
                }
                else if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes < 0 && time.TimeOfDay <= timePlay.TimeOut)
                {
                    var data = await _stemHub.MatchClient(matchID, time);
                    res.setData("data", data.Message);
                }
                else
                {
                    var list = await _matchHaflRepo.ListHaftMatch(matchID);
                    res.setData("data", list);
                }
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //convert datetime to timespan
        public TimeSpan ConvertDateTimeToTimeSpan(DateTime dateTime)
        {
            // Chọn thời gian gốc là DateTime.MinValue (01/01/0001 00:00:00)
            DateTime baseTime = DateTime.MinValue;

            // Tính TimeSpan từ thời gian gốc đến dateTime
            TimeSpan timeSpan = dateTime - baseTime;

            return timeSpan;
        }

        //realtime-teampoint 
        public async Task<SingleRsp> teamPoint(int matchID)
        {
            var time = ConvertToVietnamTime(DateTime.Now);
            var res = new SingleRsp();
            try
            {
                var timePlay = _matchRepo.GetById(matchID);
                var totalTime = timePlay.StartDate + timePlay.TimeIn;

                var checkDate = time < timePlay.StartDate;

                TimeSpan checkTime = (DateTime)totalTime - time;

                if (time.Date < timePlay.StartDate.Value.Date || checkTime.TotalMinutes > 15)
                {
                    //res.SetMessage("Trận đấu chưa diễn ra");
                    res.setData("data", "notstarted");
                }
                else
                if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes <= 15 && checkTime.TotalMinutes > 0)
                {
                    var data = new
                    {
                        TimeAwait = checkTime,
                        TimeInMatch = timePlay.TimeIn

                    };
                    res.setData("data", data);
                    return res;
                }
                else if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes < 0 && time.TimeOfDay <= timePlay.TimeOut)
                {

                    var data = await _stemHub.TeamPointClient(matchID, time);
                    res.setData("data", data.Message);
                }
                else
                {
                    var match = await _matchRepo.TeamPoint(matchID);
                    res.setData("data", match);
                }
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //realtime-listpoint
        public async Task<SingleRsp> ListPoint(int teamMatchId, int scheduleId)
        {
            var time = ConvertToVietnamTime(DateTime.Now);
            var res = new SingleRsp();
            try
            {
                //var listPoint = await _matchRepo.MatchListPoint(teamMatchId);
                //var matchID = listPoint.MatchId;
                var schedule = _scheduleRepo.GetById(scheduleId);

                var timePlay = _matchRepo.GetById(schedule.MatchId);
                var totalTime = timePlay.StartDate + timePlay.TimeIn;

                var checkDate = time < timePlay.StartDate;

                TimeSpan checkTime = (DateTime)totalTime - time;

                if (schedule.IsJoin != true) {

                    res.setData("data", "notjoin");
                }
                else if (time.Date < timePlay.StartDate.Value.Date || checkTime.TotalMinutes > 15)
                {
                    //res.SetMessage("Trận đấu chưa diễn ra");
                    res.setData("data", "notstarted");
                }
                else
                if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes <= 15 && checkTime.TotalMinutes > 0)
                {
                    var data = new
                    {
                        TimeAwait = checkTime,
                        TimeInMatch = timePlay.TimeIn

                    };
                    res.setData("data", data);
                    return res;
                }
                else if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes < 0 && time.TimeOfDay <= timePlay.TimeOut)
                {

                    var data = await _stemHub.ListPointClient(teamMatchId, ConvertToVietnamTime(DateTime.Now));
                    res.setData("data", data.Message);
                }
                else
                {
                    var match = await _matchRepo.MatchListPoint(teamMatchId);
                    res.setData("data", match);
                }
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        //confirm point

        public async Task<SingleRsp> ConfirmPoint(int actionID, string status)
        {
            var res = new SingleRsp();
            try
            {
                var point = _actionRepo.GetById(actionID);
                point.Status = status;
                _actionRepo.Update(point);
                res.SetMessage("Update success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
        public DateTime ConvertToVietnamTime(DateTime serverTime)
        {

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime.ToUniversalTime(), vietnamTimeZone);

            return vietnamTime;
        }
    }
}
