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
    public class TeamSvc
    {
        private readonly TeamRepo _teamRepo;
        private readonly IMapper _mapper;
        private readonly CompetitionRepo _competitionRepo;
 
        public TeamSvc(TeamRepo teamRepo, IMapper mapper, CompetitionRepo competitionRepo)
        {
            _teamRepo = teamRepo;
            _mapper = mapper;
            _competitionRepo = competitionRepo;
        }

        public MutipleRsp GetTeams()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _teamRepo.All();
                if (lst != null)
                {
                    var lstRes = _mapper.Map<List<Common.Rsp.TeamRsp>>(lst);
                    res.SetSuccess(lstRes, "200");
                }
                else
                {
                    res.SetError("404", "No data found");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var team = _teamRepo.GetById(id);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    var teamRes = _mapper.Map<Common.Rsp.TeamRsp>(team);
                    res.setData("200", teamRes);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(TeamReq req)
        {
            var res = new SingleRsp();
            try
            {
                var newTeam = _mapper.Map<Team>(req);
                _teamRepo.Add(newTeam);
                res.setData("200", newTeam);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Update(TeamReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var team = _teamRepo.GetById(id);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    _mapper.Map(req, team);
                    _teamRepo.Update(team);
                    res.setData("200", team);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Delete(int id)
        {
            var res = new SingleRsp();
            try
            {
                var team = _teamRepo.GetById(id);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    _teamRepo.Delete(id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetTeamsByCompetition(int id)
        {
            var res = new MutipleRsp();
            try
            {
                var teams = _teamRepo.GetTeamsByCompetition(id);

                if (teams != null)
                {
                    var lstRes = _mapper.Map<List<Common.Rsp.TeamRsp>>(teams);
                    res.SetSuccess(lstRes, "200");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        
    }
}
