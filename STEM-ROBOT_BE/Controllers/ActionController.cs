using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.DAL.Repo;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly ActionSvc _actionSvc;

        public ActionController(ActionSvc actionSvc)
        {
            _actionSvc = actionSvc;
        }

        [HttpPut("confirm-action/{actionId}")]
        public async Task<IActionResult> ConfirmAction(int actionId, string status,int scheduleId)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = await _actionSvc.ConfirmAction(actionId, status, scheduleId, userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
        [HttpPost("send-action")]
        public async Task<IActionResult> SendAction(ActionReq req)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = await _actionSvc.NewAction(req,(int) req.ScheduleId, userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
        [HttpGet("referee-sup-action/{scheduleId}")]
        public async Task<IActionResult> GetActionSchedule(int scheduleId)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = await _actionSvc.GetActionSchedule(scheduleId, userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
    }
}
