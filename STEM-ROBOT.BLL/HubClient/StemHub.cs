using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading;


namespace STEM_ROBOT.BLL.HubClient
{


    public class StemHub : Hub, IStemHub
    {
        private readonly IMapper _mapper;
        private readonly TeamMatchRepo _teamMatchRepo;
        private readonly NotificationRepo _notificationRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly MatchHaflRepo _matchHaflRepo;
        private readonly MatchRepo _matchRepo;
        public StemHub(IMapper mapper, TeamMatchRepo teamMatchRepo, NotificationRepo notificationRepo, IServiceProvider serviceProvider, MatchHaflRepo matchHaflRepo, MatchRepo matchRepo)
        {
            _mapper = mapper;
            _teamMatchRepo = teamMatchRepo;
            _notificationRepo = notificationRepo;
            _serviceProvider = serviceProvider;
            _matchHaflRepo = matchHaflRepo;
            _matchRepo = matchRepo;
        }

        public async Task<SingleRsp> MatchClient(int matchID, DateTime time)
        {
            var cancellationToken = new CancellationToken();

            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    var timeMinute = _matchRepo.GetById(matchID);
                    TimeSpan totalTime = (TimeSpan)(timeMinute.TimeOut - time.TimeOfDay);

                    linkedCts.CancelAfter(TimeSpan.FromMinutes(totalTime.TotalMinutes));

                    while (!linkedCts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var match = await _matchHaflRepo.ListHaftMatch(matchID);

                            await hubContext.Clients.All.SendAsync("match-deatail/" + matchID.ToString(), match, linkedCts.IsCancellationRequested);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending message: {ex.Message}");
                        }

                        // Delay to prevent continuous rapid execution, adjust as needed
                        await Task.Delay(TimeSpan.FromMilliseconds(3000));
                    }
                    res.SetMessage("timeout");
                }

            }

            return res;
        }

        public async Task<SingleRsp> NotificationClient(int userid)
        {
            var cancellationToken = new CancellationToken();
            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    linkedCts.CancelAfter(TimeSpan.FromMinutes(15));

                    while (!linkedCts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var notifications = await _notificationRepo.listNotifi(userid);
                            var mappedNotifications = _mapper.Map<List<NotificationRsp>>(notifications);
                            await hubContext.Clients.All.SendAsync("notification/" + userid.ToString(), mappedNotifications, linkedCts.IsCancellationRequested);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending message: {ex.Message}");
                        }

                        // Delay to prevent continuous rapid execution, adjust as needed
                        await Task.Delay(TimeSpan.FromMilliseconds(5000));
                    }
                }
            }

            res.SetMessage("timeout");
            return res;
        }


        // Realtime teampoint
        public async Task<SingleRsp> TeamPointClient(int matchID, DateTime time)
        {
            var cancellationToken = new CancellationToken();
            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    var timeMinute = _matchRepo.GetById(matchID);
                    TimeSpan totalTime = (TimeSpan)(timeMinute.TimeOut - time.TimeOfDay);
                    if (totalTime.TotalMinutes > 0)
                    {
                        linkedCts.CancelAfter(TimeSpan.FromMinutes(totalTime.TotalMinutes));

                        while (!linkedCts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                var match = await _matchRepo.TeamPoint(matchID);

                                await hubContext.Clients.All.SendAsync("team-match-result/" + matchID.ToString(), match, linkedCts.IsCancellationRequested);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error sending message: {ex.Message}");
                            }

                            // Delay to prevent continuous rapid execution, adjust as needed
                            await Task.Delay(TimeSpan.FromMilliseconds(3000));
                        }

                    }

                }
            }

            res.SetMessage("timeout");
            return res;
        }

        //realtime point 
        public async Task<SingleRsp> ListPointClient(int teamMatchID, DateTime time)
        {
            var cancellationToken = new CancellationToken();
            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    var listPoint = await _matchRepo.MatchListPoint(teamMatchID);
                    var matchID = listPoint.MatchId;
                    var match = _matchRepo.GetById(matchID);

                    TimeSpan totalTime = (TimeSpan)(match.TimeOut - time.TimeOfDay);
                  
                    if (totalTime.TotalMinutes > 0)
                    {
                        linkedCts.CancelAfter(TimeSpan.FromMinutes(totalTime.TotalMinutes));
                        while (!linkedCts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                var listPoints = await _matchRepo.MatchListPoint(teamMatchID);

                                await hubContext.Clients.All.SendAsync("list-point/" + teamMatchID.ToString(), listPoints, linkedCts.IsCancellationRequested);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error sending message: {ex.Message}");
                            }

                            // Delay to prevent continuous rapid execution, adjust as needed
                            await Task.Delay(TimeSpan.FromMilliseconds(4000));
                        }
                    }

                }
            }

            res.SetMessage("timeout");
            return res;

        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected: {connectionId}");


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client disconnected: {connectionId}");


            await base.OnDisconnectedAsync(exception);
        }
        public DateTime ConvertToVietnamTime(DateTime serverTime)
        {
            // Lấy thông tin múi giờ Việt Nam (UTC+7)
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển đổi từ thời gian server sang thời gian Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime.ToUniversalTime(), vietnamTimeZone);

            return vietnamTime;
        }
    }
}
