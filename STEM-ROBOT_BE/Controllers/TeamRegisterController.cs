using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/teams-register")]
    [ApiController]

    public class TeamRegisterController : ControllerBase
    {
        private readonly TeamRegisterSvc _teamRegisterSvc;
        public TeamRegisterController(TeamRegisterSvc teamRegisterSvc)
        {
            _teamRegisterSvc = teamRegisterSvc;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetListTeamMatch(int competitionId, List<TeamMatchConfigCompetition> teamsMatchs)
        {
            
            return Ok();
        }
        [HttpPost()]
        public async Task<IActionResult> RegisterTeamCompetion(int competitionId, TeamRegisterReq teamRegister)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {             
                return Unauthorized(new { Message = "Please login" });
            }
             
            int userId = int.Parse(user.Value);
            var res = await _teamRegisterSvc.RegisterTeamCompetion(teamRegister, competitionId, userId);
            return Ok(res);
        }
    }

}
