using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolSvc _schooSvc;
        public SchoolController(SchoolSvc schooSvc)
        {
            _schooSvc = schooSvc;
        }
        [HttpPost("Import")]
        public async Task<IActionResult> ImportSchool(IFormFile file)
        {
            var res = await _schooSvc.ImportSchool(file);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetListSchool()
        {
            var res = _schooSvc.ListSchool();
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddSchool([FromBody] SchoolReq request)
        {
            var res = _schooSvc.AddSchool(request);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);

        }
        [HttpGet("id")]
        public async Task<IActionResult> GetIDSchool(int id)
        {

            var res = _schooSvc.ListIDSchool(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdateSchool(int id, SchoolReq request)
        {
            var res = _schooSvc.UpdateSchool(id, request);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var res = _schooSvc.DeleteSchool(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
    }
}
