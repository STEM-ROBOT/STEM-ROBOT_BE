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
using Google.Api.Gax;
using System.Text.RegularExpressions;


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
        private readonly ActionRepo _actionRepo;
        public StemHub(IMapper mapper, TeamMatchRepo teamMatchRepo, NotificationRepo notificationRepo, IServiceProvider serviceProvider, MatchHaflRepo matchHaflRepo, MatchRepo matchRepo, ActionRepo actionRepo)
        {
            _mapper = mapper;
            _teamMatchRepo = teamMatchRepo;
            _notificationRepo = notificationRepo;
            _serviceProvider = serviceProvider;
            _matchHaflRepo = matchHaflRepo;
            _matchRepo = matchRepo;
            _actionRepo = actionRepo;
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
                    totalTime = totalTime.Add(TimeSpan.FromMinutes(5));

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
                    totalTime = totalTime.Add(TimeSpan.FromMinutes(5));
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
                    totalTime = totalTime.Add(TimeSpan.FromMinutes(5));
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


        //realtime team-adhesion-actions 
        public async Task<SingleRsp> TeamAdhesionListActionClient(int teamMatchId, DateTime time)
        {
            var cancellationToken = new CancellationToken();
            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    var matchInfo = await _matchRepo.MatchTimeOut(teamMatchId);
                               
                    TimeSpan totalTime = (TimeSpan)(matchInfo.TimeOut - time.TimeOfDay);
                    totalTime = totalTime.Add(TimeSpan.FromMinutes(5));
                    if (totalTime.TotalMinutes > 0)
                    {
                        linkedCts.CancelAfter(TimeSpan.FromMinutes(totalTime.TotalMinutes));
                        while (!linkedCts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                var listPoints = await _matchRepo.TeamAdhesionListAction(teamMatchId);

                                await hubContext.Clients.All.SendAsync("team-adhesion-actions/" + listPoints.TeamId.ToString(), listPoints, linkedCts.IsCancellationRequested);
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
        public async Task<SingleRsp> AverageScoreActionClient(int teamId, int matchId, DateTime time)
        {
            var cancellationToken = new CancellationToken();
            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    var matchInfo =  _matchRepo.GetById(matchId);
                    TimeSpan LengthData = (TimeSpan)matchInfo.TimeOut - (TimeSpan)matchInfo.TimeIn;
                    int totalMinutes = (int)LengthData.TotalMinutes;
                    int[] minuteArray = Enumerable.Range(1, totalMinutes).ToArray();
                    TimeSpan totalTime = (TimeSpan)(matchInfo.TimeOut - time.TimeOfDay);
                   
                    if (totalTime.TotalMinutes > 0)
                    {
                        linkedCts.CancelAfter(TimeSpan.FromMinutes(totalTime.TotalMinutes));
                        while (!linkedCts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                TimeSpan totalTimeCheck = (TimeSpan)(time.TimeOfDay - matchInfo.TimeIn);
                                int totalTimeScore = (int)totalTimeCheck.TotalMinutes;
                                var match = await _teamMatchRepo.getAverageScore(teamId, matchId);
                                var bonusAction = CalculateAverageScoresPerMinute(match.Where(a => a.ScoreCategory.Type == "Điểm cộng").ToList(), "bonus");
                                var MinusAction = CalculateAverageScoresPerMinute(match.Where(a => a.ScoreCategory.Type == "Điểm trừ").ToList(), "minus");
                                var rpsBosnusAction = Enumerable.Repeat(0.0, totalTimeScore).ToArray();
                                foreach (var action in bonusAction)
                                {

                                    rpsBosnusAction[action.Minute] = action.AverageScore;

                                }
                                var rpsMinusAction = Enumerable.Repeat(0.0, totalTimeScore).ToArray();
                                foreach (var action in MinusAction)
                                {

                                    rpsMinusAction[action.Minute] = action.AverageScore;

                                }
                                var rpsAverageScore = new
                                {
                                    time = minuteArray,
                                    bonus = rpsBosnusAction,
                                    minus = rpsMinusAction
                                };
                                
                                // danh sach trung binh diem cong theo phut
                                await hubContext.Clients.All.SendAsync("team-average-score/" + teamId.ToString(), rpsAverageScore, linkedCts.IsCancellationRequested);
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
        public class AveragedAction
        {
            public int Minute { get; set; }
            public double AverageScore { get; set; }
        }
        public static List<AveragedAction> CalculateAverageScoresPerMinute(List<DAL.Models.Action> actions, string type)
        {
            // Kiểm tra loại điểm để xác định hệ số nhân
            int multiplier = type == "bonus" ? 1 : -1;

            // Nhóm các hành động theo phút và tính điểm trung bình
            var averagedActions = actions
                .GroupBy(a => a.EventTime.Value.Minutes)
                .Select(g => new AveragedAction
                {
                    Minute = g.Key,
                    AverageScore = multiplier * (double)g.Average(a => a.ScoreCategory.Point)
                })
                .ToList();

            return averagedActions;
        }
        //realtime action refereeCompetitions
        public async Task<SingleRsp> ActionByRefereeSupClient(int matchId, int refereeCompetitionId,int schduleId, DateTime time)
        {
            var cancellationToken = new CancellationToken();
            var res = new SingleRsp();
            using (var scope = _serviceProvider.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<StemHub>>();

                // Create a CancellationTokenSource with a 30-minute timeout
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {

                    var timeMinute = _matchRepo.GetById(matchId);
                    TimeSpan totalTime = (TimeSpan)(timeMinute.TimeOut - time.TimeOfDay);
                    totalTime = totalTime.Add(TimeSpan.FromMinutes(5));
                    if (totalTime.TotalMinutes > 0)
                    {
                        linkedCts.CancelAfter(TimeSpan.FromMinutes(totalTime.TotalMinutes));
                        while (!linkedCts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                var listAction = await _actionRepo.ActionByRefereeSup(matchId, refereeCompetitionId);

                                await hubContext.Clients.All.SendAsync("list-action-refere-sub/" + schduleId.ToString(), listAction, linkedCts.IsCancellationRequested);
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
