﻿using AutoMapper;
using Microsoft.Identity.Client;
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
        private readonly IMapper _mapper;

        public ActionSvc(ActionRepo actionRepo, TeamMatchRepo teamMatchRepo, ScoreCategoryRepo scoreCategoryRepo, IMapper mapper)
        {
            _actionRepo = actionRepo;
            _mapper = mapper;
            _teamMatchRepo = teamMatchRepo;
            _scoreCategoryRepo = scoreCategoryRepo;
        }

        public MutipleRsp GetActions()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _actionRepo.All();
                if (lst != null)
                {
                    var lstRes = _mapper.Map<List<ActionRsp>>(lst);
                    res.SetData("data", lstRes);
                }
                else
                {
                    res.SetError("404", "No data found");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var action = _actionRepo.GetById(id);
                if (action == null)
                {
                    res.SetError("404", "Action not found");
                }
                else
                {
                    var actionRes = _mapper.Map<ActionRsp>(action);
                    res.setData("data", actionRes);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(ActionReq req)
        {
            var res = new SingleRsp();
            try
            {
                var newAction = _mapper.Map<Action>(req);
                _actionRepo.Add(newAction);
                res.setData("data", newAction);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Update(ActionReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var action = _actionRepo.GetById(id);
                if (action == null)
                {
                    res.SetError("404", "Action not found");
                }
                else
                {
                    _mapper.Map(req, action);
                    _actionRepo.Update(action);
                    res.setData("data", action);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Delete(int id)
        {
            var res = new SingleRsp();
            try
            {
                var action = _actionRepo.GetById(id);
                if (action == null)
                {
                    res.SetError("404", "Action not found");
                }
                else
                {
                    _actionRepo.Delete(id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
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
                    if (score.Type.ToLower() == "điểm cộng")
                    {
                        teamMatch.TotalScore = teamMatch.TotalScore + score.Point;
                    }
                    else
                    {
                        teamMatch.TotalScore = teamMatch.TotalScore - score.Point;
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
                throw new Exception(ex.Message);
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
                    ScoreCategoryId= req.ScoreCategoryId,
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
                throw new Exception(ex.Message);
            }
            return res;
        }
    }
}
