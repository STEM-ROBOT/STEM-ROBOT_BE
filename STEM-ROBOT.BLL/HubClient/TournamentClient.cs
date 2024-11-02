using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.HubClient
{
    public class TournamentClient : Hub
    {
        public async Task SendCountdown(int number)
        {
            await Clients.All.SendAsync("Number", number);
        }
    }
}
