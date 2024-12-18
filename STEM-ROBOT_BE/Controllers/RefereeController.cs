﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.DAL.Models;

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
        [HttpGet("referee-tournament")]
        public async Task<IActionResult> ListRefereeTournament()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please Login ");
            int userID = int.Parse(user.Value);
            var res = await _refereeSvc.ListRefereeTournament(userID);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("referee-sup-tournament")]
        public async Task<IActionResult> ListSupRefereeTournament()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please Login ");
            int userId = int.Parse(user.Value);
            var res = await _refereeSvc.ListSupRefereeTournament(userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("byTournamnet/{tournamentId}")]
        public IActionResult GetRefereeByTournamentId(int tournamentId)
        {
            var res = _refereeSvc.GetListRefereeByTournament(tournamentId);
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
        public async Task<IActionResult> AddListReferee([FromBody] List<RefereeReq> referees)
        {
            var res = await _refereeSvc.AddListReferee(referees);
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

        [HttpGet("free-referee")]
        public IActionResult GetListFreeRefereeInTournamentId(int tournamentId, int competitionId)
        {
            var res = _refereeSvc.GetListRefereeAvailable(tournamentId, competitionId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }

        [HttpPost("{competitionId}/assign-referees")]
        public IActionResult AssignReferees(int competitionId, [FromBody] List<AssginRefereeReq> referees, int numberTeamReferee, int numberSubReferee)
        {
            var res = _refereeSvc.AssignRefereeInCompetition(competitionId, referees, numberTeamReferee, numberSubReferee);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }

    }
}
