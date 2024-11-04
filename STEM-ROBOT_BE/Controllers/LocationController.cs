using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT_BE.Controllers
{
    [Route("api/locations")]
    [ApiController]
    //[Authorize(Roles = "1,2")]
    public class LocationController : ControllerBase
    {
        private readonly LocationSvc _locationSvc;

        public LocationController(LocationSvc locationSvc)
        {
            _locationSvc = locationSvc;
        }

        [HttpGet()]
        public IActionResult GetLocations()
        {
            var res = _locationSvc.GetLocations();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }
        [HttpGet("{id}")]
        public IActionResult GetLocationById(int id)
        {
            var res = _locationSvc.GetById(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPost()]
        public IActionResult CreateLocation([FromBody] LocationReq req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _locationSvc.CreateLocation(req);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateLocation([FromBody] LocationReq req, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _locationSvc.UpdateLocation(req, id);
            if (!res.Success)
            {
                StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteLocation(int id)
        {
            var res = _locationSvc.DeleteLocation(id);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Message);
        }

        [HttpGet("competitionId")]
        public IActionResult GetLocationByCompetition(int competitionId)
        {
            var res = _locationSvc.GetLocationsByCompetition(competitionId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("available/{competitionId}")]
        public IActionResult GetAvailableLocation(int competitionId)
        {
            var res = _locationSvc.GetAvailableLocations(competitionId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPost("list-location")]
        public IActionResult AddListLoaction(int competitionId, [FromBody] List<LocationReq> request)
        {
            var res = _locationSvc.AddListLocation(request ,competitionId);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
    }
}
