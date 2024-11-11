using STEM_ROBOT.BLL.HubClient;
using STEM_ROBOT.Common.Rsp;
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
        public NotificationSvc(IStemHub stemHub)
        {

            _stemHub = stemHub;
        }

        public async Task<SingleRsp> NotificationAccount( int userID)
        {
            var res = new SingleRsp();
            try
            {
                var data = _stemHub.NotificationClient(userID);
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
