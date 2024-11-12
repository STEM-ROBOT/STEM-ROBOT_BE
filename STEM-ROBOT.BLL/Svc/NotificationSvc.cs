using STEM_ROBOT.BLL.HubClient;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class NotificationSvc
    {
        private readonly IStemHub _stemHub;
        private readonly ScheduleRepo _scheduleRepo;
        private readonly MatchRepo _matchRepo;
        public NotificationSvc(IStemHub stemHub,ScheduleRepo scheduleRepo,MatchRepo matchRepo)
        {

            _stemHub = stemHub;
            _scheduleRepo = scheduleRepo;
            _matchRepo = matchRepo;
        }
        
        //check notification
        public async Task<SingleRsp> NotificationAccount( int userID)
        {
            var res = new SingleRsp();
            try
            {
                var data = await _stemHub.NotificationClient(userID);
                res.setData("data", data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
       
    }
}
