using AutoMapper;
using STEM_ROBOT.BLL.Mail;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class RefereeCompetitionSvc
    {
        private readonly RefereeCompetitionRepo _refereeCompetitionRepo;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public RefereeCompetitionSvc(RefereeCompetitionRepo refereeCompetitionRepo, IMapper mapper,IMailService mailService)
        {
            _refereeCompetitionRepo = refereeCompetitionRepo;
            _mapper = mapper;
            _mailService = mailService;
        }
        public async Task<MutipleRsp> ListRefeeCompetition()
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _refereeCompetitionRepo.ListRefereeCompetition();
                if (list == null) throw new Exception("No data");
                var mapper = _mapper.Map<List<RefereeCompetitionRsp>>(list);
                res.SetData("data", mapper);

            }catch(Exception ex)
            {
                throw new Exception("Get List Referee Fail");
            }
            return res;

        }
        
        
    }
}
