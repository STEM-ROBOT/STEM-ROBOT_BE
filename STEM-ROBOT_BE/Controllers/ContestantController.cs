using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

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
