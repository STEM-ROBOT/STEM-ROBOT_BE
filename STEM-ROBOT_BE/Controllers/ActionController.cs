﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet("list-action")]
        public IActionResult GetActions()
        {
            var res = _actionSvc.GetActions();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetActionById(int id)
        {
            var res = _actionSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(404, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost("add-action")]
        public IActionResult CreateAction([FromBody] ActionReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _actionSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("update-action")]
        public IActionResult UpdateAction([FromBody] ActionReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _actionSvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("delete-action")]
        public IActionResult DeleteAction(int id)
        {
            var res = _actionSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
        [HttpPut("confirm-action")]
        public async Task<IActionResult> ConfirmAction(int actionId, string status,int scheduleId)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userId = int.Parse(user.Value);
            var point = await _actionSvc.ConfirmAction(actionId, status, scheduleId, userId);
            return Ok(point.Message);
        }
        [HttpPost("send-action")]
        public async Task<IActionResult> SendAction(ActionReq req, int scheduleId)
        {

            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userId = int.Parse(user.Value);
            var point = await _actionSvc.NewAction(req, scheduleId, userId);
            return Ok(point.Message);
        }

    }
}
