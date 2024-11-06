using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.DAL.Repo;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/refereecompetition")]
    [ApiController]
    public class RefereeCompetitionController : ControllerBase
    {
        private readonly RefereeCompetitionSvc _refereeCompetitionSvc;
        public RefereeCompetitionController(RefereeCompetitionSvc refereeCompetitionSvc)
        {
            _refereeCompetitionSvc = refereeCompetitionSvc;
        }
        [HttpGet("list-referee-competition")]
        public async Task<IActionResult> ListRefereeCompetition(int competitionID)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please Login ");
            int userID = int.Parse(user.Value);
            
            var list = await _refereeCompetitionSvc.ListRefeeCompetition(competitionID,userID);
            return Ok(list.Data);
        }
    }
}
