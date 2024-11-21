using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using System.Drawing.Printing;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly TournamentSvc _tournament;

        public TournamentController(TournamentSvc tournamentSvc)
        {
            _tournament = tournamentSvc;
        }


        [HttpGet("get-status")]
        public async Task<IActionResult> getStatus(int id)
        {
            var res = await _tournament.getStatus(id);
            if (!res.Success) throw new Exception("Please check againt");
            return Ok(res);
        }
        [HttpGet("list-tournament")]
        public async Task<IActionResult> getListTournament(string? name = null, string? provinceCode = null, string? status = null, int? GenerId = null, int page = 1, int pageSize = 10)
        {
            var res = await _tournament.GetTournament(name, provinceCode, status, GenerId, page, pageSize);
            if (!res.Success) throw new Exception("Please check again");
            return Ok(res);
        }
        [HttpGet("list-tournament-moderator")]
        public async Task<IActionResult> getTournamentModerator()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return BadRequest("Please Login!");
            }

            int userID = int.Parse(user.Value);
            MutipleRsp res = await _tournament.getListTournamentModerator(userID);
            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return StatusCode(401, res.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getTournament(int id)
        {
            var res = await _tournament.GetById(id);
            if (!res.Success) throw new Exception("Please check again");
            return Ok(res);
        }

        [HttpGet("per-month")]
        public IActionResult GetTournamentPerMonth()
        {
            var res = _tournament.GetTournamentsPerMonth();
            if (!res.Success) throw new Exception("Please check again");
            return Ok(res);
        }
        [HttpPut("viewer")]
        public async Task<IActionResult> UpdateView(int tournamentId)
        {

          
            MutipleRsp res = await _tournament.UpdateViewer(tournamentId);
            if (res.Success)
            {
                return Ok(res.Message);
            }
            else
            {
                return StatusCode(401, res.Message);
            }
        }
        [HttpGet("check-register-moderator")]
        public async Task<IActionResult> CheckRegisterContestant(int tournamentId)
        {


            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return BadRequest("Please Login!");
            }

            int userID = int.Parse(user.Value);
            var res = await _tournament.CheckRegisterContestant(tournamentId, userID);
            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return StatusCode(401, res.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> addTournament(TournamentReq request)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return Unauthorized("Please Login!");
            }

            int userID = int.Parse(user.Value);
            var res = await _tournament.AddTournement(userID, request);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(401, res.Message);
            }
        }
        //[HttpGet("{tourNamentID}")]
        //public async Task<IActionResult> getTimeTournament(int tourNamentID)
        //{
        //    SingleRsp res = await _tournament.CountTimeTournament(tourNamentID);
        //    if (!res.Success)
        //    {
        //        return StatusCode(401, res.Message);
        //    }
        //    return Ok(res.Data);
        //}
        
    }
}
