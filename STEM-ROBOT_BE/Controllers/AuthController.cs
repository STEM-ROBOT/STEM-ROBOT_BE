using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthSvc _authSvc;

        public AuthController(AuthSvc authSvc)
        {
            _authSvc = authSvc;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq login)
        {
            var user = await _authSvc.Login(login);
            return Ok(user);
        }
    }
}
