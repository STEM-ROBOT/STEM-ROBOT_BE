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
        [HttpPut("")]
        public async Task<IActionResult> GetListTeamMatch(int competitionId,List<TeamMatchConfigCompetition>  teamsMatchs)
        {
            var res = await _teamMatchSvc.UpdateTeamMatchConfig(teamsMatchs, competitionId);
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
