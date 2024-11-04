using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System.Threading.Tasks;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly CompetitionSvc _competionSvc;
        public CompetitionController(CompetitionSvc competitionSvc)
        {
            _competionSvc = competitionSvc;
        }

        [HttpGet]
        public async Task<IActionResult> GetListCompetition()
        {
            var res = await _competionSvc.GetListCompetitions();
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetByIdCompetition(int id)
        {
            var res = await _competionSvc.GetIDCompetitions(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }

        //nội dung thi đấu của giải
        [HttpGet("tournament")]
        public async Task<IActionResult> GetToutnamentID(int id)
        {
            var res = await _competionSvc.getCompetitionWithIDTournament(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        //luật tính điểm của nội dung thi đấu
        [HttpGet("score-competition")]
        public async Task<IActionResult> GetListScore(int competitionID)
        {
            var res = await _competionSvc.getListScoreCompetion(competitionID);
            if (!res.Success)
            {
                throw new Exception("Please check again");
            }
            return Ok(res);
        }
        // danh sách đội của nội dung thi đấu
        [HttpGet("team-competition")]
        public async Task<IActionResult> GetListTeamPlay(int competitionId)
        {
            var res = await _competionSvc.getlistTeamplay(competitionId);
            if (!res.Success)
            {
                throw new Exception("Please check again");
            }
            return Ok(res);
        }
        [HttpGet("total-matches")]
        public IActionResult GetTotalMatches(int numberOfTeams, int numberOfGroups, int numberTeamsNextRound)
        {
            var res = _competionSvc.CalculateTotalMatches(numberOfTeams, numberOfGroups, numberTeamsNextRound);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res.Data);
        }

        //cấu hình hình thức thi đấu của nội dung thi đấu
        [HttpPut("format-config")]
        public async Task<IActionResult> UpdateCompetitionFormat(int competitionId, CompetitionConfigFormatReq request)
        {
         
            if (request.FormatId == 1)
            {
                var res = await _competionSvc.UpdateCompetitionConfig(competitionId,request);
                if (!res.Success)
                {
                    res.SetError("400", res.Message);


                }
                return Ok(res);
            }else if (request.FormatId == 2)
            {
                var res = _competionSvc.UpdateCompetitionFormatTable(competitionId, request);
                if (!res.Success)
                {
                    res.SetError("400", res.Message);

                }
                return Ok(res);
            }
              return Ok();
           
        }
 
        //thông tin cấu hình đăng kí thí sinh tham gia nội dung
        [HttpGet("config-register")]
        public async Task<IActionResult> GetGenerCompetitionID(int competitionID)
        {
            var res = await _competionSvc.getGenerCompetitionID(competitionID);
            if (!res.Success)
            {
                throw new Exception("Please check input");
            }
            return Ok(res.Data);
        }
        //xem lịch trình thi đấu của nội dung
        [HttpGet("match-schedule-view")]
        public async Task<IActionResult> MatchScheduleCompetition(int competitionId)
        {
            var res = await _competionSvc.matchScheduleCompetition(competitionId);
            if (!res.Success)
            {
                throw new Exception("Please check input");
            }
            return Ok(res.Data);
        }
        //xem lịch trình thi đấu vong bang của nội dung
        [HttpGet("match-group-stage-view")]
        public async Task<IActionResult> MatchGroupStageCompetition(int competitionId)
        {
            var res = await _competionSvc.matchScheduleCompetition(competitionId);
            if (!res.Success)
            {
                throw new Exception("Please check input");
            }
            return Ok(res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompetition(CompetitionReq request)
        {
            var res = _competionSvc.AddCompetion(request);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }


        [HttpPut("id")]
        public async Task<IActionResult> UpdateCompetition(int id, CompetitionReq request)
        {
            var res = _competionSvc.UpdateCompetition(id, request);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }


        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            var res = _competionSvc.DeleteCompetition(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }



        [HttpGet("Infor")]
        public async Task<IActionResult> GetInfor(int id)
        {
            var res = await _competionSvc.GetCompetitionInfor(id);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }


        [HttpPost("addRegulation/{competitionId}")]
        public async Task<IActionResult> AddRegulation(string filerule, int competitionId)
        {
            var res = await _competionSvc.AddRule(filerule, competitionId);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }

        [HttpPost("config-teamtable-stagetable")]
        public async Task<IActionResult> ConfigTeamTableStageTable(int competitionId, TableAssignmentReq tableAssignments)
        {
            var res = await _competionSvc.AssignTeamsToTables(competitionId, tableAssignments);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
        [HttpGet("data-to-assing/{competitionId}")]
        public async Task<IActionResult> GetDataToAssign(int competitionId)
        {
            var res = await _competionSvc.GetDataToAssign(competitionId);
            if (!res.Success)
            {
                res.SetError("400", res.Message);
            }
            return Ok(res);
        }
    }
}

