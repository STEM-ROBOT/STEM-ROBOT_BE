using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.DAL.Models;
using System.Collections.Generic;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/contestants")]
    [ApiController]
    public class ContestantController : ControllerBase
    {
        private readonly ContestantSvc _contestantSvc;

        public ContestantController(ContestantSvc contestantSvc)
        {
            _contestantSvc = contestantSvc;
        }
        [HttpPost]
        public async Task<IActionResult> AddContestant(IFormFile formFile)
        {
            var res = await _contestantSvc.AddContestant(formFile);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }


        [HttpPost("list-contestant")]
        public IActionResult AddListContestantInTournament([FromBody] List<ContestantReq> contestants, int tournamentId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return BadRequest("Please Login!");
            }

            int userID = int.Parse(user.Value);
            var res = _contestantSvc.AddListContestantInTournament(contestants, userID, tournamentId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetListContestant()
        {
            var res = _contestantSvc.GetListContestants();
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }
        [HttpGet("available/tournamentId")]
        public IActionResult GetListAvailableContestantInTournamentId(int tournamentId)
        {
            var res = _contestantSvc.GetListAvailableContestantByTournament(tournamentId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }

        [HttpGet("accountId")]
        public IActionResult GetListContestantByAccount()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return BadRequest("Please Login!");
            }

            int userID = int.Parse(user.Value);
            var res = _contestantSvc.GetListContestantByAccount(userID);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }
        [HttpGet("tournamentId")]
        public IActionResult GetListContestantInTournamentId(int tournamentId)
        {
            var res = _contestantSvc.GetListContestantByTournament(tournamentId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }

        [HttpGet("id")]
        public async Task<IActionResult> GetIdContestant(int id)
        {
            var res = _contestantSvc.GetContestantID(id);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdateContestant(int id, ContestantReq request)
        {
            var res = _contestantSvc.UpdateContestant(id, request);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteContestant(int id)
        {
            var res = _contestantSvc.DeleteContestant(id);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }


        [HttpGet("teamId")]
        public IActionResult GetContestantInTeam(int teamId)
        {
            var res = _contestantSvc.GetContestantInTeam(teamId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }

        [HttpGet("available/competitionId")]
        public IActionResult GetAvailableContestantByCompetition(int competitionId)
        {
            var res = _contestantSvc.GetAvailableContestantByCompetition(competitionId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);
        }
        [HttpPost("contestant-to-team/{teamId}")]
        public IActionResult AddContestantToTeam(int teamId, [FromBody] List<ContestantTeamReq> req)
        {
            var res = _contestantSvc.AddContestantTeam(teamId, req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res);
        }
        [HttpGet("public-available-moderater")]
        public IActionResult GetListAvailableContestantByAccount(int tounamentId, int competitionId)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return Unauthorized(new { Message = "Please login" });
            }

            int userId = int.Parse(user.Value);
            var res = _contestantSvc.GetListAvailableContestantByAccount(userId, tounamentId, competitionId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res.Data);

        }

        [HttpPost("public-tournament")]
        public IActionResult AddContestantPublicTournament(int tournamentId, ContestantReq contestants)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            var userSchool = User.Claims.FirstOrDefault(x => x.Type == "SchoolName");
            if (user == null)
            {
                return Unauthorized("Please Login!");
            }
            var accountId = int.Parse(user.Value);
            var res = _contestantSvc.AddContestantPublic(tournamentId, accountId, contestants, userSchool.ToString());
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("public-tournament-moderator")]
        public async Task<IActionResult> GetContestantRegister(int tournamentId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return Unauthorized(new { Message = "Please login" });
            }

            int userId = int.Parse(user.Value);
            var res = await _contestantSvc.GetContestantRegister(tournamentId, userId);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res.Data);
        }
    }
}