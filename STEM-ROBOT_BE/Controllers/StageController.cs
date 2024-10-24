using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/stages")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly StageSvc _stageSvc;
        public StageController(StageSvc stageSvc)
        {
            _stageSvc = stageSvc;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var res = _stageSvc.GetListStage();
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpGet("id")]
        public IActionResult GetByID(int id)
        {
            var res = _stageSvc.GetIDStage(id);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpPost]
        public IActionResult AddStage(StageReq request)
        {
            var res = _stageSvc.AddStage(request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        
        [HttpPut("id")]
        public IActionResult UpdateStage(int id, StageReq request)
        {
            var res = _stageSvc.UpdateStage(id, request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpDelete("id")]
        public IActionResult DeleteStage(int id)
        {

            var res = _stageSvc.DeleteStage(id);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
    }
}
