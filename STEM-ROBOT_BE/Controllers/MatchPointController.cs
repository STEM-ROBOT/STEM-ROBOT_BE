using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/matchpoints")]
    [ApiController]
    public class MatchPointController : ControllerBase
    {
        private readonly MatchPointSvc _matchPointSvc;
        public MatchPointController(MatchPointSvc matchPointSvc)
        {
            _matchPointSvc = matchPointSvc;
        }
        [HttpGet()]
        public async Task<IActionResult> ListPoint(int matchID)
        {
            var list = await _matchPointSvc.ListActionPointID(matchID);
            return Ok(list.Data);
        }
        
    }
}
