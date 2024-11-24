using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;

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
        [HttpGet("byCompetitionId/{competitionId}")]
        public async Task<IActionResult> GetTeamRegister(int competitionId)
        {
            var res = await _teamRegisterSvc.getListTeamRegister(competitionId);
            return Ok(res);
        }
        [HttpGet("by-tournament/{tournamentId}")]
        public async Task<IActionResult> GetTeamRegisterTournament(int tournamentId)
        {
            var res = await _teamRegisterSvc.getListTeamRegisterTournament(tournamentId);
            return Ok(res.Data);
        }
        [HttpPut("{id}/bycompetitionId/{competitionId}")]
        public IActionResult UpdateStatusTeamRegister(int id,int competitionId, [FromBody] TeamRegisterStatusRsp teamRegisterStatusRsp)
        {
            var res =  _teamRegisterSvc.updateStatusTeamRegister(id, competitionId, teamRegisterStatusRsp);
            return Ok(res);
        }
    }

}
