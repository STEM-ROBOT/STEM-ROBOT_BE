using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
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
        public IActionResult AddListContestant([FromBody] List<ContestantReq> contestants)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null)
            {
                return BadRequest("Please Login!");
            }

            int userID = int.Parse(user.Value);
            var res =  _contestantSvc.AddListContestant(contestants,userID);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res.Data);
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
            var res =  _contestantSvc.GetContestantID(id);
            if (!res.Success)
            {
                res.SetError("500", res.Message);
            }
            return Ok(res);

        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdateContestant(int id,ContestantReq request)
        {
            var res =  _contestantSvc.UpdateContestant(id, request);
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
    }
}
