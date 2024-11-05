using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleSvc _scheduleSvc;
        private readonly IMapper _mapper;
        public ScheduleController(ScheduleSvc scheduleSvc, IMapper mapper)
        {
            _scheduleSvc = scheduleSvc;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetSchedules()
        {
            var res = _scheduleSvc.GetSchedules();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetScheduleById(int id)
        {
            var res = _scheduleSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(404, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost()]
        public IActionResult CreateSchedule([FromBody] ScheduleReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _scheduleSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSchedule([FromBody] ScheduleReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _scheduleSvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSchedule(int id)
        {
            var res = _scheduleSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
        [HttpGet("schedule-sendmail")]
        public async Task<IActionResult> Sendmail(int scheduleId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userID = int.Parse(user.Value);
            var sendmail = await _scheduleSvc.SendMail(scheduleId, userID);
            return Ok("Send Success");
        }
        [HttpPost("schedule-sendcode")]
        public async Task<IActionResult> SendCode(int scheduleId,string code)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userID = int.Parse(user.Value);
            var sendmail = await _scheduleSvc.CheckCodeSchedule(scheduleId, userID,code);
            return Ok(sendmail);
        }
    }
}
