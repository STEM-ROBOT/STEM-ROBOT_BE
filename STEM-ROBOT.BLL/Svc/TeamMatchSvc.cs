using AutoMapper;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class TeamMatchSvc
    {
        private readonly TeamMatchRepo _teamMatchRepo;
        private readonly IMapper _mapper;
        private readonly TeamRepo _teamRepo;
        private readonly MatchRepo _matchRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly TeamTableRepo _teamTableRepo;
        private readonly StageRepo _stageRepo;
        public TeamMatchSvc(TeamMatchRepo teamMatchRepo, IMapper mapper, TeamRepo teamRepo, MatchRepo matchRepo, TableGroupRepo tableGroupRepo, TeamTableRepo teamTableRepo, StageRepo stageRepo)
        {
            _teamMatchRepo = teamMatchRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
            _tableGroupRepo = tableGroupRepo;
            _teamTableRepo = teamTableRepo;
            _stageRepo = stageRepo;
        }
        public MutipleRsp GetListTeamMatch()
        {
            var res = new MutipleRsp();
            try
            {
                var teamMatch = _teamMatchRepo.All();
                if (teamMatch == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<TeamMatch>>(teamMatch);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp GetIdTeamMatch(int id)
        {
            var res = new SingleRsp();
            try
            {
                var teamMatch = _teamMatchRepo.GetById(id);
                if (teamMatch == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<TeamMatch>(teamMatch);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public async  Task<SingleRsp>  UpdateTeamMatchConfig(List<TeamMatchConfigCompetition> request,int competitionId)
        {
            var res = new SingleRsp();
            var competition = await _teamMatchRepo.getCompetition(competitionId);

            if (request.Count < 1 && competition == false) 
            {
                res.SetError("Kiểu dữ liệu không đúng");
                return res;
            }
            try
            {
               List<TeamMatch> teamMatches = new List<TeamMatch>();
                foreach (var teamMatch in request)
                {
                    var team_match = new TeamMatch
                    {
                        Id = teamMatch.teamMatchId,
                        TeamId = teamMatch.teamId,
                        NameDefault = teamMatch.teamName,
                    };
                    teamMatches.Add(team_match);
                }
                _teamMatchRepo.UpdateRange(teamMatches);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }

    }
}
