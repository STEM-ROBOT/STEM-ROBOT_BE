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
        [HttpGet("province")]
        public async Task<IActionResult> ListProvince(int id)
        {
            var res = await _areaSvc.ListProvince(id);
            return Ok(res.Data);
        }
        [HttpGet("district")]
        public async Task<IActionResult> ListDistrict(int id)
        {
            var res = await _areaSvc.ListDistrict(id);
            return Ok(res.Data);
        }
    }
}
