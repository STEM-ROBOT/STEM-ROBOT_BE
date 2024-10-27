using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly CompetitionSvc _competionSvc;
        public CompetitionController(CompetitionSvc competitionSvc)
        {
            _competionSvc = competitionSvc;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetListCompetition()
        {
            var res = await _competionSvc.GetListCompetitions();
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetByIdCompetition(int id)
        {
            var res = await _competionSvc.GetIDCompetitions(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }*/
        [HttpPost]
        public async Task<IActionResult> AddCompetition(CompetitionReq request)
        {
            var res = _competionSvc.AddCompetion(request);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdateCompetition(int id,CompetitionReq request)
        {
            var res = _competionSvc.UpdateCompetition(id, request);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res) ;
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            var res = _competionSvc.DeleteCompetition(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }

        [HttpPost("format-table")]
        public async Task<IActionResult> AddCompetitionFormatTable(CompetitionReq request)
        {
            var res = _competionSvc.CreateCompetitionFormatTable(request);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }

        

    }
}
