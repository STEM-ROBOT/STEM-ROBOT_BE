using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamSvc _teamSvc;
        private readonly IMapper _mapper;
        public TeamController(TeamSvc svc, IMapper mapper)
        {
            _teamSvc = svc;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetTeams()
        {
            var res = _teamSvc.GetTeams();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeamById(int id)
        {
            var res = _teamSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(404, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost()]
        public IActionResult CreateTeam([FromBody] TeamReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _teamSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeam([FromBody] TeamReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _teamSvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var res = _teamSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
    }
}
