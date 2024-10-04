using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthSvc _authSvc;

        public AuthController(AuthSvc authSvc)
        {
            _authSvc = authSvc;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginReq login)
        {
            SingleRsp response = _authSvc.Login(login);

            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return StatusCode(401, response.Message);  
            }
        }
    }
}
