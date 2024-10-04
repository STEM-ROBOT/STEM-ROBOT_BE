using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountSvc _accountSvc;

        public AccountController(AccountSvc accountSvc)
        {
            _accountSvc = accountSvc;
        }

        [HttpGet()]
        public IActionResult GetAllAccount()
        {
            var res = _accountSvc.GetAll();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("id")]
        public IActionResult GetAccountById(int id)
        {
            var res = _accountSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message); 
            }
            return Ok(res.Data);
        }

        [HttpPost("id")]
        public IActionResult CreateAccount([FromBody] AccountReq req)
        {
            var res = _accountSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("id")]
        public IActionResult UpdateAccount([FromBody] AccountReq req, int id)
        {
            var res = _accountSvc.Update(req,id);
            if (!res.Success)
            {
                StatusCode(500, res.Message);
            }
            return StatusCode(500, res.Message);
        }

        [HttpDelete("id")]
        public IActionResult DeleteAccount(int id)
        {
            var res = _accountSvc.Delete(id);
            if (res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
    }
}
