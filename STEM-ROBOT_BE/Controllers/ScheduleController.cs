using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.DAL.Models;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleSvc _scheduleSvc;
        private readonly NotificationSvc _notificationSvc;

        private readonly IMapper _mapper;
        public ScheduleController(ScheduleSvc scheduleSvc, IMapper mapper, NotificationSvc notificationSvc)
        {
            _scheduleSvc = scheduleSvc;
            _mapper = mapper;
            _notificationSvc = notificationSvc;
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
        [HttpGet("schedule-referee-main-match")]
        public async Task<IActionResult> CheckSchedule(int scheduleID)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login");
            int userID = int.Parse(user.Value);
            var res = await _scheduleSvc.checkTimeSchedule(scheduleID, userID);
            
            return Ok(res.Data);
        }
        [HttpGet("schedule-referee-sup")]
        public async Task<IActionResult> ScheduleSupReferee(int refereeCompetitionId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login");
            int userID = int.Parse(user.Value);
            var res = await _scheduleSvc.ScheduleSupReferee(refereeCompetitionId, userID);

            return Ok(res.Data);
        }
        [HttpGet("schedule-referee-sup-match-info")]
        public async Task<IActionResult> ScheduleSupRefereeMatchInfo(int scheduleId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login");
            int userID = int.Parse(user.Value);
            var res = await _scheduleSvc.ScheduleSupRefereeMatchInfo(scheduleId, userID);

            return Ok(res.Data);
        }
        //[HttpPost()]
        //public IActionResult CreateSchedule([FromBody] ScheduleReq req)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var res = _scheduleSvc.Create(req);
        //    if (!res.Success)
        //    {
        //        return StatusCode(500, res.Message);
        //    }
        //    return Ok(res.Data);
        //}

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
        [HttpPost("schedule-sendmail")]
        public async Task<IActionResult> Sendmail(int scheduleId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userID = int.Parse(user.Value);
            var sendmail = await _scheduleSvc.SendMail(scheduleId, userID);
            if(sendmail.TestError != null)
            {
                return Ok(sendmail.Data);
            }
            else
            {
                return Ok(sendmail.TestError);
            }
           
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
        [HttpPut("schedule-confirm")]
        public async Task<IActionResult> ConfirmMatch(int scheduleId)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userID = int.Parse(user.Value);
            var response = await _scheduleSvc.ConfirmSchedule(scheduleId, userID);
            return Ok(response);
        }
        [HttpPut("schedule-confirm-random")]
        public async Task<IActionResult> ConfirmMatchRandom(int scheduleId, ScheduleRandomReq req)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userID = int.Parse(user.Value);
            var response = await _scheduleSvc.ConfirmMatchRandom(scheduleId, userID, req);
            return Ok(response);
        }

        [HttpGet("match-config-schedule")]
        public async Task<IActionResult>  GetScheduleConfigCompetition(int competitionId)
        {
            var res = await _scheduleSvc.ScheduleCompetition(competitionId);
            if (!res.Success)
            {
                return StatusCode(404, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost("")]
        public async Task<IActionResult> UpdateScheduleConfigCompetition(int competitionId , List<ScheduleReq> reqs)
        {
            var res =  await  _scheduleSvc.updateScheduleConfigCompetition(competitionId, reqs);

            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost("schedule-busy")]
        public async Task<IActionResult> UpdateBusy(int scheduleID)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userID = int.Parse(user.Value);
            var sendmail = await _scheduleSvc.UpdateBusy(scheduleID, userID);
            return Ok(sendmail);
        }
    }
}
