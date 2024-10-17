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
        public async Task<IActionResult> GetAccounts()
        {
            var res = await _accountSvc.GetAll();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var res = await _accountSvc.GetById(id);
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

        [HttpPut("{id}")]
        public IActionResult UpdateAccount([FromBody] AccountReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _accountSvc.Update(req, id);
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
    }
}