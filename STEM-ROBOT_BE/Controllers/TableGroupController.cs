using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/tables")]
    [ApiController]
    public class TableGroupController : ControllerBase
    {
        private readonly TableGroupSvc _tableGroupSvc;
        public TableGroupController(TableGroupSvc tableGroupSvc)
        {
            _tableGroupSvc = tableGroupSvc;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var res = _tableGroupSvc.GetListTable();
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpGet("id")]
        public IActionResult GetByID(int id)
        {
            var res = _tableGroupSvc.GetIdTable(id);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpPost]
        public IActionResult AddStage(TableGroupReq request)
        {
            var res = _tableGroupSvc.AddTable(request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpPut("id")]
        public IActionResult UpdateStage(int id, TableGroupReq request)
        {
            var res = _tableGroupSvc.UpdateTable(id, request);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
        [HttpDelete("id")]
        public IActionResult DeleteStage(int id)
        {

            var res = _tableGroupSvc.DeleteTable(id);
            if (!res.Success)
            {
                res.SetMessage(res.Message);
            }
            return Ok(res);
        }
    }
}
