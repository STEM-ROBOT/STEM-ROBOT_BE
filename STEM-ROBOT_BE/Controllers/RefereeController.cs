﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/referees")]
    [ApiController]
    public class RefereeController : ControllerBase
    {
        private readonly RefereeSvc _refereeSvc;

        public RefereeController(RefereeSvc refereeSvc)
        {
            _refereeSvc = refereeSvc;
        }

        [HttpGet()]
        public IActionResult GetReferees()
        {
            var res = _refereeSvc.GetReferees();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetRefereeById(int id)
        {
            var res = _refereeSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost()]
        public IActionResult CreateReferee([FromBody] RefereeReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _refereeSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost("list-referee")]
        public IActionResult AddListReferee([FromBody] List<RefereeReq> referees)
        {
            var res = _refereeSvc.AddListReferee(referees);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReferee([FromBody] RefereeReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _refereeSvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReferee(int id)
        {
            var res = _refereeSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
        /*[HttpGet("free-referee")]
        public IActionResult GetListRefereeInTournamentId(int competitionId)
        {
            var res = _refereeSvc.GetListRefereeAvailable(competitionId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }*/
    }
}
