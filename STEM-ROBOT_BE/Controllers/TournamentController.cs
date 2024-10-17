﻿using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;

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

        [HttpPost]
        public  IActionResult addTournament(TournamentReq request)
        {
            var user =  User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return BadRequest("Please Login!");
            }

            int userID = int.Parse(user.Value);
            SingleRsp res = _tournament.AddTournement(userID, request);
            if (res.Success)
            {
                return Ok(res.Data);
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
