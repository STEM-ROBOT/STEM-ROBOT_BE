using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealtimeController : ControllerBase
    {
        private readonly RealTimeSvc _realTimeSvc;
        public RealtimeController(RealTimeSvc realTimeSvc)
        {
            _realTimeSvc = realTimeSvc;
        }
        [HttpGet]
        [Route("late")]
        public async Task<IActionResult> getRealTime()
        {
            var realtime =  _realTimeSvc.randomNumber;
            return Ok(realtime);
        }
    }
}
