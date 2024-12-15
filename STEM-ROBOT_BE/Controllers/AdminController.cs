using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminSvc _adminSvc;
        public AdminController(AdminSvc adminSvc)
        {
            _adminSvc = adminSvc;
        }
        [HttpGet("list-account")]
        public async Task<IActionResult> GetListAccountAdmin()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = await _adminSvc.GetListAccountAdmin();
            return Ok(res);
        }
        [HttpGet("list-genre")]
        public async Task<IActionResult> GetListGenreAdmin()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = await _adminSvc.GetListGenre();
            return Ok(res.Data);
        }
        [HttpGet("list-order")]
        public async Task<IActionResult> GetListOrder(string? nameUser)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = await _adminSvc.GetListOrder(nameUser);
            return Ok(res.Data);
        }
        [HttpPost("add-genre")]
        public async Task<IActionResult> AddGenreAdmin(GenreReq genre)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return Unauthorized("Please login ");
            int userId = int.Parse(user.Value);
            var res = _adminSvc.AddGenre(genre);
            return Ok(res);
        }
    }
}
