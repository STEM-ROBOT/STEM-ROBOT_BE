using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/matches")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchSvc _matchSvc;
        public MatchController(MatchSvc matchSvc)
        {
            _matchSvc = matchSvc;
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

        [HttpGet("get-round")]
        public async Task<IActionResult> getRoundgame(int StageID)
        {
            var res = await _matchSvc.getListRound(StageID);
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
    }
}
