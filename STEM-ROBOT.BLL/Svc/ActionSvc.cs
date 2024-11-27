using AutoMapper;
using Microsoft.Identity.Client;
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
using Action = STEM_ROBOT.DAL.Models.Action;

namespace STEM_ROBOT.BLL.Svc
{
    public class ActionSvc
    {
        private readonly ActionRepo _actionRepo;
        private readonly TeamMatchRepo _teamMatchRepo;
        private readonly ScoreCategoryRepo _scoreCategoryRepo;
        private readonly ScheduleRepo _scheduleRepo;
        private readonly MatchRepo _matchRepo;
        private readonly StemHub _stemHub;
        private readonly IMapper _mapper;

        public ActionSvc(ActionRepo actionRepo, TeamMatchRepo teamMatchRepo, ScoreCategoryRepo scoreCategoryRepo, IMapper mapper, StemHub stemHub, ScheduleRepo scheduleRepo, MatchRepo matchRepo)
        {
            _actionRepo = actionRepo;
            _mapper = mapper;
            _teamMatchRepo = teamMatchRepo;
            _scoreCategoryRepo = scoreCategoryRepo;
            _stemHub = stemHub;
            _scheduleRepo = scheduleRepo;
            _matchRepo = matchRepo;
        }


        public async Task<SingleRsp> ConfirmAction(int actionId, string status, int scheduleId, int accoutId)
        {
            var res = new SingleRsp();
            try
            {
                var check = await _actionRepo.checkRefereeschedule(scheduleId, accoutId);

                if (check != null)
                {
                    var action = _actionRepo.GetById(actionId);
                    var score = _scoreCategoryRepo.GetById(action.ScoreCategoryId);
                    var teamMatch = _teamMatchRepo.GetById(action.TeamMatchId);
                    if (status == "accept")
                    {
                        if (score.Type.ToLower() == "điểm trừ" && teamMatch.TotalScore > 0)
                        {
                            teamMatch.TotalScore -= score.Point;
                        }
                        else if(score.Type.ToLower() == "điểm cộng")
                        {
                            teamMatch.TotalScore += score.Point;
                        }
                    }
                    action.Status = status;
                    _teamMatchRepo.Update(teamMatch);
                    _actionRepo.Update(action);
                    res.SetMessage("Update success");
                }
                else
                {
                    res.SetMessage("Update fail");
                }

            }
            catch (Exception ex)
            {
                res.SetMessage("Update fail:"+ex);
            }
            return res;
        }
        public async Task<SingleRsp> NewAction(ActionReq req, int scheduleId, int userId)
        {
            var res = new SingleRsp();
            var schedule = await _actionRepo.checkRefereeschedule(scheduleId, userId);
            try
            {
                var data = new Action
                {
                    EventTime = req.EventTime,
                    MatchHalfId = req.MatchHalfId,
                    ScoreCategoryId = req.ScoreCategoryId,
                    RefereeCompetitionId = schedule.RefereeCompetitionId,
                    Status = "pending",
                    Score = 0,
                    TeamMatchId = req.TeamMatchId,
                };
                _actionRepo.Add(data);
                res.SetMessage("success");
            }
            catch (Exception ex)
            {
                res.SetMessage("send fail:"+ ex);
            }
            return res;
        }
        public async Task<SingleRsp> GetActionSchedule(int scheduleId, int accountId)
        {
            var time = ConvertToVietnamTime(DateTime.Now);
            var res = new SingleRsp();
            try
            {

                var schedule = await _actionRepo.checkRefereeschedule(scheduleId, accountId);

                var timePlay = _matchRepo.GetById(schedule.MatchId);
                var totalTime = timePlay.StartDate + timePlay.TimeIn;

                var checkDate = time < timePlay.StartDate;

                TimeSpan checkTime = (DateTime)totalTime - time;

                if (schedule.IsJoin != true)
                {

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

                    var data = await _stemHub.ActionByRefereeSupClient((int)schedule.MatchId, (int)schedule.RefereeCompetitionId, scheduleId, ConvertToVietnamTime(DateTime.Now));
                    res.setData("data", data.Message);
                }
                else
                {
                    var listAction = await _actionRepo.ActionByRefereeSup((int)schedule.MatchId, (int)schedule.RefereeCompetitionId);
                    res.setData("data", listAction);
                }
                return res;

            }
            catch (Exception ex)
            {
                res.SetMessage("get fail:" + ex);
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
