using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/genres")]
    [ApiController]
    //[Authorize(Roles = "1")]
    public class GenreController : ControllerBase
    {
        private readonly GenreSvc _genreSvc;

        public GenreController(GenreSvc genreSvc)
        {
            _genreSvc = genreSvc;
        }

        [HttpGet()]
        public IActionResult GetGenres()
        {
            var res = _genreSvc.GetGenres();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetGenreById(int id)
        {
            var res = _genreSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }


        [HttpPost()]
        public IActionResult CreateGenre([FromBody] GenreReq req)
        {
            var res = _genreSvc.Create(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre([FromBody] GenreReq req, int id)
        {
            var res = _genreSvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGenre(int id)
        {
            var res = _genreSvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
    }
}
