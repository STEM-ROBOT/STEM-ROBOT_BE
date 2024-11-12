using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private AreaSvc _areaSvc;
        public AreaController(AreaSvc areaSvc)
        {
            _areaSvc = areaSvc;
        }
        [HttpGet("area")]
        public async Task<IActionResult> ListArea()
        {
            var res = await _areaSvc.ListArea();
            return Ok(res.Data);
        }

        [HttpGet("province/areaId")]
        public IActionResult ListProvince(int areaId)
        {
            var res =  _areaSvc.GetProvinceByArea(areaId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("province")]
        public IActionResult ListAllProvince()
        {
            var res = _areaSvc.AllProvinceByArea();
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("district/provinceId")]
        public IActionResult ListDistrict(int provinceId)
        {
            var res = _areaSvc.GetDistrictByProvince(provinceId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("school/districtId")]
        public IActionResult ListSchool(int districtId)
        {
            var res = _areaSvc.GetSchoolByDistrict(districtId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
    }
}
