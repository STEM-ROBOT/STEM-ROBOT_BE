using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.HubClient;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/realtime")]
    [ApiController]
    public class RealtimeController : ControllerBase
    {
        private readonly IStemHub _stemHub;
        public RealtimeController(StemHub realTimeSvc)
        {
            _stemHub = realTimeSvc;
        }
        [HttpGet]
        public async Task<IActionResult> getRealTime(string key)
        {
            var userID = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (userID == null) return BadRequest("Please Login");
            int userId = int.Parse(userID.Value);
           
            var res = await _stemHub.NotificationClient( key, userId);
            return Ok("Ok");
        }
    }
}
