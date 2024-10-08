//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace STEM_ROBOT.BLL.HubClient
//{
//    public class TournamentClient : Hub
//    {
//        public async Task SendCountdown(DateTime endDate)
//        {
//            var timeLeft = endDate - DateTime.Now;
//            if(timeLeft.TotalSeconds > 0)
//            {
//                await Clients.All.SendAsync("ReceiveCountdown", timeLeft.TotalSeconds);
//            }
//            else
//            {
//                await Clients.All.SendAsync("RegistrationClosed");
//            }
//        }
//    }
//}
