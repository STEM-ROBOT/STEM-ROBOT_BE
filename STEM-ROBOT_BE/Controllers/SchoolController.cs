using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolSvc _schooSvc;
        public SchoolController(SchoolSvc schooSvc)
        {
            _schooSvc = schooSvc;
        }
        [HttpPost]
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
    }
}
