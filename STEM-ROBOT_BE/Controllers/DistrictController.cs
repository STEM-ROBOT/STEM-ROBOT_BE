using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly DistrictSvc _districtSvc;
        public DistrictController(DistrictSvc districtSvc)
        {
            _districtSvc = districtSvc;
        }

        [HttpPost("Import-excel")]
        public async Task<IActionResult> ImportDistrictExcel(IFormFile file)
        {
            var res = await _districtSvc.ImportDistrictExcel(file);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        /*[HttpGet]
        public async Task<IActionResult> GetListDistrict()
        {
            var res = _districtSvc.ListDistrict();
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddDistrict([FromBody] DistrictReq request)
        {
            var res = _districtSvc.AddDistrict(request);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);

        }
        [HttpGet("id")]
        public async Task<IActionResult> GetIDDistrict(int id)
        {
            var res = _districtSvc.GetIDDistrict(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }*/
    }
}
