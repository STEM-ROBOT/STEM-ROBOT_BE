using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamTableController : ControllerBase
    {
        private readonly TeamTableSvc _teamTableSvc;
        public TeamTableController(TeamTableSvc teamTableSvc)
        {
            _teamTableSvc = teamTableSvc;
        }

        [HttpGet]
        public IActionResult GetListTeamTable()
        {
            var res = _teamTableSvc.GetListTeamTable();
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult GetIdTeamTable(int id)
        {
            var res = _teamTableSvc.GetIdTeamTable(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        
    }
}
