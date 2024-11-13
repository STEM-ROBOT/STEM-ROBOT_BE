using AutoMapper;
using Org.BouncyCastle.Ocsp;
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

    public class TeamRegisterSvc
    {
        private readonly TeamRegisterRepo _teamRegisterRepo;
        private readonly CompetitionRepo _competitionRepo;
        private readonly ContestantTeamRepo _contestantTeamRepo;
        private readonly AccountRepo _accountRepo;
        private readonly IMapper _mapper;
        public TeamRegisterSvc(TeamRegisterRepo repo, AccountRepo accountRepo, CompetitionRepo competitionRepo, ContestantTeamRepo contestantTeamRepo, IMapper mapper)
        {
            _teamRegisterRepo = repo;
            _competitionRepo = competitionRepo;
            _contestantTeamRepo = contestantTeamRepo;
            _mapper = mapper;
            _accountRepo= accountRepo;
        }


        public async Task<SingleRsp> RegisterTeamCompetion(TeamRegisterReq teamRegister, int competitionId, int userId)
        {
            var res = new SingleRsp();
            var competition = _competitionRepo.GetById(competitionId);
            var account= _accountRepo.GetById(userId); 
            if (competition == null)
            {
                res.Setmessage("Nội dung thi đấu không tồn tại !");
            }
            else
            {
                try {
                    teamRegister.CompetitionId = competition.Id;
                    teamRegister.AccountId = userId;
                    var newTeamRegister = _mapper.Map<TeamRegister>(teamRegister);
                    _teamRegisterRepo.Add(newTeamRegister);
                    var listContestTants = _mapper.Map<List<ContestantTeam>>(teamRegister.Contestants);
                    foreach (var contestant in listContestTants)
                    {
                        contestant.TeamRegisterId = newTeamRegister.Id;
                    }
                    // Sau đó, bạn có thể lưu listContestants vào database nếu cần
                    _contestantTeamRepo.AddRange(listContestTants);
                    res.SetMessage("success");
                }
                catch (Exception ex) 
                {
                    res.SetError(ex.ToString());
                }
               
            }
            return res;
        }
    }
}
