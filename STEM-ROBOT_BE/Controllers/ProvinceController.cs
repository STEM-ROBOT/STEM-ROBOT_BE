using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly ProvinceSvc _provinceSvc;
        public ProvinceController(ProvinceSvc provinceSvc)
        {
            _provinceSvc = provinceSvc;
        }
        
        [HttpPost("Import-excel")]
        public async Task<IActionResult> ImportProvinceExcel(IFormFile file)
        {
            var res = await _provinceSvc.ImportProvinceExcel(file);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        /*[HttpGet]
        public async Task<IActionResult> GetListProvince()
        {
            var res = _provinceSvc.ListProvince();
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddProvince([FromBody] ProvinceReq request)
        {
            var res = _provinceSvc.AddProvince(request);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);

        }
        [HttpGet("id")]
        public async Task<IActionResult> GetIDProvince(int id)
        {
            var res = _provinceSvc.GetIDProvince(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }*/
    }
}
