using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class MatchPointSvc
    {
        private readonly MatchHaflRepo _matchHaflRepo;
        public MatchPointSvc(MatchHaflRepo matchHaflRepo)
        {
            _matchHaflRepo = matchHaflRepo;

        }
        public async Task<SingleRsp> ListActionPointID(int matchID)
        {
            var res = new SingleRsp();
            try
            {
                var list = await _matchHaflRepo.ListHaftMatch(matchID);
                if (list == null) throw new Exception("No data");
                res.setData("data", list);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
    }
}
