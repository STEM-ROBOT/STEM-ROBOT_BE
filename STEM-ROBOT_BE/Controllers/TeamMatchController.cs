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
        [HttpGet("get-list-team-match")]
        public IActionResult GetListTeamMatch()
        {
            var res = _teamMatchSvc.GetListTeamMatch();
            return Ok(res);
        }
        [HttpGet("get-id-team-match")]
        public IActionResult GetIdTeamMatch(int id)
        {
            var res = _teamMatchSvc.GetIdTeamMatch(id);
            return Ok(res);
        }

        [HttpPost("assign-stage-table/{competitionId}")]
        public IActionResult AssignTeamsToMatchesInStageTable(int competitionId, List<AssignTeamsToMatchesInStageTableReq> tableAssignments)
        {
            var res = _teamMatchSvc.AssignTeamsToMatchesInStageTable(competitionId, tableAssignments);
            return Ok(res);
        }

        // test cái này
        [HttpPost("assign-stage-next/{competitionId}")]
        public IActionResult AssignTeamsToMatchesInStageNext(int competitionId, int numberTeamNextRound)
        {
            var res = _teamMatchSvc.AssignTeamsToNextRoundMatches(competitionId, numberTeamNextRound);
            return Ok(res);
        }
    }
}
