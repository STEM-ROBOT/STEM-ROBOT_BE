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

        [HttpGet()]
        public  IActionResult GetAccounts()
        {
            var res =  _accountSvc.GetAccounts();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("userId")]
        public IActionResult GetAccountById()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please check User ");
            int userID = int.Parse(user.Value);
            var res = _accountSvc.GetById(userID);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }


        [HttpPost("signup-moderator")]
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

        [HttpPut("update-account")]
        public IActionResult UpdateAccount([FromBody] AccountReq req)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please check User ");
            int userID = int.Parse(user.Value);
            var res = _accountSvc.Update(req, userID);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
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

        [HttpGet("recent-package")]
        public IActionResult GetRecentPakage(int id)
        {
            var res = _accountSvc.GetPackageUsed(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePass(ChangePass pass)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please check User ");
            int userID = int.Parse(user.Value);
            var passWord = _accountSvc.ChangePassword(userID,pass);
            return Ok("Success");
        }
        
    }
}