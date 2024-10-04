using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL;


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

        [HttpGet]
        public IActionResult GetAllAccount()
        {
            var res = _accountSvc.GetAll();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var res = _accountSvc.GetById(id);
            if (res.Success)
            {
                return Ok(res.Data);
            }return StatusCode(500, res.Message);
        }


    }
}
