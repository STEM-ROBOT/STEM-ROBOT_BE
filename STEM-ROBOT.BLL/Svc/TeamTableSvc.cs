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
    public class TeamTableSvc
    {
        private readonly TeamTableRepo _teamTableRepo;
        private readonly IMapper _mapper;
        private readonly TeamRepo _teamRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        public TeamTableSvc(TeamTableRepo teamTableRepo, IMapper mapper, TeamRepo teamRepo, TableGroupRepo tableGroupRepo)
        {
            _teamTableRepo = teamTableRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _tableGroupRepo = tableGroupRepo;
        }
        public MutipleRsp GetListTeamTable()
        {
            var res = new MutipleRsp();
            try
            {
                var teamTable = _teamTableRepo.All();
                if (teamTable == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<TeamTable>>(teamTable);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp GetIdTeamTable(int id)
        {
            var res = new SingleRsp();
            try
            {
                var teamTable = _teamTableRepo.GetById(id);
                if (teamTable == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<TeamTable>(teamTable);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        
        public SingleRsp Update(TeamTableReq request)
        {
            var res = new SingleRsp();
            try
            {

            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        
        
    }
}
