using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.DAL.Models;

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
        public async Task<IActionResult> MatchDetailAction(int matchID,DateTime date) 
        {
            var list = await _matchSvc.CheckMatch(matchID, date);
            if(list.Data == null) return Ok(list.Message);
            return Ok(list.Data);
        }
    }
}
