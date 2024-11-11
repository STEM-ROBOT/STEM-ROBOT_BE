using STEM_ROBOT.Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.HubClient
{
    public interface IStemHub
    {
        Task<SingleRsp> NotificationClient(string key, int userid);
        Task<SingleRsp> MatchClient(int matchID, DateTime time);
    }
}
