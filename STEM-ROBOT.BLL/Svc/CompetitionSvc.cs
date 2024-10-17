using AutoMapper;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class CompetitionSvc

    {
        private readonly CompetitionRepo _competitionRepo;
        private readonly IMapper _mapper;
        public CompetitionSvc(CompetitionRepo competitionRepo,IMapper mapper)
        {
            _competitionRepo = competitionRepo;
            _mapper = mapper;
        }

        public async Task<MutipleRsp> GetListCompetitions()
        {
            var res = new MutipleRsp();
            try {
                var list = await _competitionRepo.getListCompetition();
                if(list == null)
                {
                    res.SetError("No Data");
                }
                var mapper = _mapper.Map<IEnumerable<CompetitionRep>>(list);
                res.SetData("OK", mapper);
            }
            catch(Exception ex) 
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> GetIDCompetitions(int id)
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _competitionRepo.getListCompetitionbyID(id);
                if (list == null)
                {
                    res.SetError("No Data");
                }
                var mapper = _mapper.Map<CompetitionRep>(list);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
