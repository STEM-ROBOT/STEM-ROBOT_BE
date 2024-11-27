using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    //[Authorize(Roles ="1")]

    public class AccountController : ControllerBase
    {
        private readonly AccountSvc _accountSvc;

        public AccountController(AccountSvc accountSvc)
        {
            _accountSvc = accountSvc;
        }


        [HttpGet("info")]
        public IActionResult GetAccountById()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please check User ");
            int userId = int.Parse(user.Value);
            var res = _accountSvc.GetInfoUser(userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("recent-package")]
        public IActionResult GetRecentPakage()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please check User ");
            int userId = int.Parse(user.Value);
            var res = _accountSvc.GetPackageUsed(userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost()]
        public IActionResult CreateAccount([FromBody] AccountReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _accountSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut()]
        public IActionResult UpdateAccount([FromBody] AccountReq req)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please check User ");
            int userId = int.Parse(user.Value);
            var res = _accountSvc.Update(req, userId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("forgot-password")]
        public async Task<IActionResult> ChangePass(ChangePass pass)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please check User ");
            int userId = int.Parse(user.Value);
            var passWord = _accountSvc.ChangePassword(userId, pass);
            return Ok("Success");
        }

        [HttpDelete()]
        public IActionResult DeleteAccount(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _accountSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }



    }
}