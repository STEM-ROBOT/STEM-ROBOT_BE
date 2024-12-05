﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/matches")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchPointSvc _matchPointSvc;
        private readonly MatchSvc _matchSvc;
        public MatchController(MatchSvc matchSvc, MatchPointSvc matchPointSvc)
        {
            _matchSvc = matchSvc;
            _matchPointSvc = matchPointSvc;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var res = _matchSvc.GetListMatch();
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpGet("id")]
        public IActionResult GetByID(int id)
        {
            var res = _matchSvc.getByIDMatch(id);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }

        [HttpGet("match-config-time")]
        public async Task<IActionResult> getRoundTableGame(int CompetitionID)
        {

            var res = await _matchSvc.getListRound(CompetitionID);
            if (!res.Success) throw new Exception("Check again");
            return Ok(res);
        }

        //[HttpGet("match-knockout-config-time")]
        //public async Task<IActionResult> getRoundKnocOutGame(int CompetitionID)
        //{
        //    bool isFormatTable = true;
        //    var res = await _matchSvc.getListRound(CompetitionID, isFormatTable);
        //    if (!res.Success) throw new Exception("Check again");
        //    return Ok(res);
        //}


        [HttpGet("get-round-knockout")]
        public async Task<IActionResult> getRoundKnockoutgame(int CompetitionID)
        {
            var res = await _matchSvc.getListKnockOut(CompetitionID);
            if (!res.Success) throw new Exception("Check again");
            return Ok(res);
        }

        [HttpGet("get-round-knockout-late")]
        public async Task<IActionResult> getRoundKnockoutgameLate(int CompetitionID)
        {
            var res = await _matchSvc.getListKnockOutLate(CompetitionID);
            if (!res.Success) throw new Exception("Check again");
            return Ok(res);
        }

        [HttpGet("get-round-table")]
        public async Task<IActionResult> GetRoundParentTables(int competitionID)
        {
            var res = await _matchSvc.GetRoundParentTable(competitionID);
            if (!res.Success) throw new Exception("Check again");
            return Ok(res);
        }

        [HttpPost]
        public IActionResult AddStage(MatchReq request)
        {
            var res = _matchSvc.AddMatch(request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }

        [HttpPut("")]
        public async Task<IActionResult> ConFigTimeMtch(int competition, MatchConfigReq request)
        {
            var res = await _matchSvc.conFigTimeMtch(competition, request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpPut("id")]
        public IActionResult UpdateStage(int id, MatchReq request)
        {
            var res = _matchSvc.UpdateMatch(id, request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpDelete("id")]
        public IActionResult DeleteStage(int id)
        {

            var res = _matchSvc.DeleteMatch(id);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpGet("match-detail-action")]
        public async Task<IActionResult> MatchDetailAction(int matchId)
        {
            var list = await _matchSvc.CheckMatch(matchId);
            //if(list.Data == null) return Ok(list.Message);
            return Ok(list.Data);
        }
        [HttpGet("match-total-point")]
        public async Task<IActionResult> Teampoint(int matchId)
        {
            var point = await _matchSvc.teamPoint(matchId);
            return Ok(point.Data);
        }
        [HttpGet("match-action-referee")]
        public async Task<IActionResult> Listpoint(int teamMatchId, int scheduleId)
        {
            var point = await _matchSvc.ListPoint(teamMatchId, scheduleId);
            return Ok(point.Data);
        }
        [HttpGet("match-action-team")]
        public async Task<IActionResult> TeamAdhesionListAction(int matchId, int teamId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login");
            int userID = int.Parse(user.Value);
            var point = await _matchSvc.TeamAdhesionListActionSvc(matchId, teamId);
            return Ok(point.Data);
        }
        [HttpPut("confirm-point")]
        public async Task<IActionResult> ConfirmPoint(int actionId, string status)
        {
            var point = await _matchSvc.ConfirmPoint(actionId, status);
            return Ok(point.Message);
        }
    }
}
