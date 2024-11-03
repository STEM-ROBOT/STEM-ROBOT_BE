using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMatchController : ControllerBase
    {
        private readonly TeamMatchSvc _teamMatchSvc;
        public TeamMatchController(TeamMatchSvc teamMatchSvc)
        {
            _teamMatchSvc = teamMatchSvc;
        }
        [HttpGet()]
        public IActionResult GetListTeamMatch()
        {
            var res = _teamMatchSvc.GetListTeamMatch();
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult GetIdTeamMatch(int id)
        {
            var res = _teamMatchSvc.GetIdTeamMatch(id);
            return Ok(res);
        }

        
    }
}
