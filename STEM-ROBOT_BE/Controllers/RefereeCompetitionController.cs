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
        public async Task<IActionResult> ListRefereeCompetition()
        {
            var list = await _refereeCompetitionSvc.ListRefeeCompetition();
            return Ok(list.Data);
        }
    }
}
