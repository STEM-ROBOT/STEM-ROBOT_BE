using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/tournamentformats")]
    [ApiController]
    //[Authorize(Roles = "1")]
    public class TournamentFormatController : ControllerBase
    {
        private readonly TournamentFormatSvc _tournamentFormatSvc;

        public TournamentFormatController(TournamentFormatSvc tournamentFormatSvc)
        {
            _tournamentFormatSvc = tournamentFormatSvc;
        }

        [HttpGet()]
        public IActionResult GetTournamentFormats()
        {
            var res = _tournamentFormatSvc.GetFormats();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetTournamentFormatById(int id)
        {
            var res = _tournamentFormatSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost()]
        public IActionResult CreateTournamentFormat([FromBody] TournamentFormatReq req)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _tournamentFormatSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTournamentFormat([FromBody] TournamentFormatReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _tournamentFormatSvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTournamnetFormat(int id)
        {
            var res = _tournamentFormatSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
    }
}
