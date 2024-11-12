using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationSvc _notificationSvc;
        public NotificationController(NotificationSvc notificationSvc)
        {
            _notificationSvc = notificationSvc;
        }
        [HttpGet("notification")]
        public async Task<IActionResult> NotificationAccount()
        {

            var userID = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (userID == null) return BadRequest("Please Login");
            int userId = int.Parse(userID.Value);
            var res = await _notificationSvc.NotificationAccount(userId);
            return Ok(res.Data);
        }
    }
}
