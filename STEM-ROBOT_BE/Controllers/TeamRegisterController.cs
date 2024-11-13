using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/teams-register")]
    [ApiController]

    public class TeamRegisterSvc : ControllerBase
    {
        private readonly TeamRegisterSvc _teamRegisterSvc;
        public TeamRegisterSvc(TeamRegisterSvc teamRegisterSvc)
        {
            _teamRegisterSvc = teamRegisterSvc;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetListTeamMatch(int competitionId, List<TeamMatchConfigCompetition> teamsMatchs)
        {
            
            return Ok();
        }
        [HttpPost("")]
        public async Task<IActionResult> RegisterTeamCompetion(int competitionId, List<TeamMatchConfigCompetition> teamsMatchs)
        {

            return Ok();
        }
    }

}
