using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public MatchSvc(MatchRepo repo, IMapper mapper, CompetitionRepo competition, TeamTableRepo teamTableRepo, TableGroupRepo tableGroupRepo, TeamRepo teamRepo, StageRepo stageRepo, StageSvc stageSvc)
        {
            _matchRepo = repo;
            _teamTableRepo = teamTableRepo;
            _mapper = mapper;
            _tableGroupRepo = tableGroupRepo;
            _teamRepo = teamRepo;
            _stageRepo = stageRepo;
            _stageSvc = stageSvc;
            _competition = competition;
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
        public async Task<SingleRsp> getListRound(int competitionId, bool isFormatTable)
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
                        startTime=competition.StartTime,
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
                                        locationId = md.LocationId
                                    }).ToList(),
                                }).ToList(),
                            }).ToList()

                        },
                        knockout = await getListRoundKnocOut(competition),
                        locations = competition.Locations.Select(l => new locationCompetitionConfig
                         {
                             locationId = l.Id,
                             locationName = l.Address

                         }).ToList(),
                    };
                    res.setData("data", roundParent);
                }
                else if(competition.FormatId == 1)
                {
                    var roundKnocOutParentConfig = new roundKnocOutParentConfig
                    {
                        startTime = competition.StartTime,
                        isMatch = (bool)competition.IsMacth,
                        knockout = await getListRoundKnocOut(competition) ,
                        locations= competition.Locations.Select(l=> new locationCompetitionConfig
                        {
                            locationId=l.Id,
                            locationName=l.Address

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
        public async Task<GroupRound> getListRoundKnocOut(Competition competition)
        {
            var knocout = new GroupRound
            {
                rounds = competition.Stages.Where(st => st.StageMode != "Vòng bảng").Select(s => new RoundGroupGame
                {
                    roundId = s.Id,
                    round = s.Name,
                    matchrounds = s.Matches.Select(ts => new RoundGroupGameMatch
                    {
                        tableName = "",
                        matches = s.Matches.Select(md => new TeamMatchRound
                        {
                            matchId = md.Id,
                            teamA = md.TeamMatches.FirstOrDefault().TeamId == null ? md.TeamMatches.FirstOrDefault().NameDefault : md.TeamMatches.FirstOrDefault().Team.Name,
                            teamB = md.TeamMatches.LastOrDefault().TeamId == null ? md.TeamMatches.LastOrDefault().NameDefault : md.TeamMatches.LastOrDefault().Team.Name,
                            date = md.StartDate,
                            locationId = md.LocationId
                        }).ToList(),
                    }).ToList(),
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
        public async Task<SingleRsp> GetRoundParentTable(int CompetitionId)
        {

            var res = new SingleRsp();
            try
            {
                var list = await _matchRepo.GetRoundParentTable(CompetitionId);
                if (list == null) throw new Exception("No data");
                RoundParentTable round_table = new RoundParentTable
                {
                    tableGroup = list.TableGroups.Select(tg => new tableGroup
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

        public async Task<SingleRsp> conFigTimeMtch(int competitionId , MatchConfigReq reqs)
        {
             var res = new SingleRsp();
            var competition_data = _competition.GetById(competitionId);
            if(competition_data == null ) {
                res.SetError("400");
                res.SetMessage("Nội dung thi đấu không tồn tại");
            }
            List<Match> matches = new List<Match>();
            DateTime endTime = DateTime.Now;    
            foreach( var match in reqs.matchs)
            {
                var matchUd = new Match
                {
                    Id= (int)match.id,
                    LocationId= (int)match.locationId,  
                    StartDate= match.startDate,
                    TimeIn = match.TimeIn,
                    TimeOut = match.TimeOut,    

                };
                matches.Add(matchUd);
                endTime = (DateTime)match.startDate;
            }
            competition_data.EndTime= endTime;  
            competition_data.IsMacth = true;    
            competition_data.TimeBreak= reqs.TimeBreak;
            //competition_data.TimeEndPlay = reqs.TimeEndPlay;
            //   competition_data.TimeStartPlay=reqs.TimeStartPlay;
            _matchRepo.UpdateRange(matches);

            return res;
        }
    }
}
