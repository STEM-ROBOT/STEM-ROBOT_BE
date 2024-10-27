using AutoMapper;
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
        public TeamMatchSvc(TeamMatchRepo teamMatchRepo, IMapper mapper, TeamRepo teamRepo, MatchRepo matchRepo)
        {
            _teamMatchRepo = teamMatchRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
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
    }
}
