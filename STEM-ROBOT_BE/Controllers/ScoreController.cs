using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/scores")]
    [ApiController]
    public class ScoreController : ControllerBase
    {

        private readonly ScoreCategorySvc _scoreCategorySvc;

        public ScoreController(ScoreCategorySvc scoreCategorySvc)
        {
            _scoreCategorySvc = scoreCategorySvc;
        }

        [HttpGet("byCompetitionId")]
        public IActionResult GetScoreCategories(int competitionId)
        {
            var res = _scoreCategorySvc.GetScoreCategories(competitionId);
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(404, res.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetScoreCategoryById(int id)
        {
            var res = _scoreCategorySvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(404, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost("{competitionId}")]
        public IActionResult CreateScoreCategory([FromBody] List<ScoreCategoryReq> req, int competitionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _scoreCategorySvc.Create(req, competitionId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateScoreCategory([FromBody] ScoreCategoryReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _scoreCategorySvc.Update(req, id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteScoreCategory(int id)
        {
            var res = _scoreCategorySvc.Delete(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }
        //[HttpGet("{bycompetitionId/{competitionId}")]
        //public IActionResult GetScoreByCompetition(int competitionId)
        //{
        //    var res = _scoreCategorySvc.GetScoreCategories(competitionId);
        //    if (!res.Success)
        //    {
        //        return StatusCode(500, res.Message);
        //    }
        //    return Ok(res.Message);
        //}
    }
}

