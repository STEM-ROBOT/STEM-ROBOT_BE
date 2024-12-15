using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.BLL.HubClient;
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
    public class TeamMatchSvc
    {
        private readonly TeamMatchRepo _teamMatchRepo;
        private readonly IMapper _mapper;
        private readonly TeamRepo _teamRepo;
        private readonly MatchRepo _matchRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly TeamTableRepo _teamTableRepo;
        private readonly StageRepo _stageRepo;
        private readonly CompetitionRepo _competition;
        private readonly StemHub _stemHub;
        public TeamMatchSvc(TeamMatchRepo teamMatchRepo, IMapper mapper, CompetitionRepo competition, TeamRepo teamRepo, MatchRepo matchRepo, TableGroupRepo tableGroupRepo, TeamTableRepo teamTableRepo, StageRepo stageRepo, StemHub stemHub)
        {
            _teamMatchRepo = teamMatchRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
            _tableGroupRepo = tableGroupRepo;
            _teamTableRepo = teamTableRepo;
            _stageRepo = stageRepo;
            _competition = competition;
            _stemHub = stemHub;
        }
        public MutipleRsp GetListTeamMatch()
        {
            var res = new MutipleRsp();
            try
            {
                var teamMatch = _teamMatchRepo.All();
                if (teamMatch == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<TeamMatch>>(teamMatch);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp GetIdTeamMatch(int id)
        {
            var res = new SingleRsp();
            try
            {
                var teamMatch = _teamMatchRepo.GetById(id);
                if (teamMatch == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<TeamMatch>(teamMatch);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> UpdateTeamMatchConfig(List<TeamMatchConfigCompetition> request, int competitionId)
        {
            var res = new SingleRsp();
            var competition = await _teamMatchRepo.getCompetition(competitionId);
            if (request.Count < 1 && competition == null)
            {
                res.SetError("Kiểu dữ liệu không đúng");
                return res;
            }
            competition.IsTeamMacth = true;
            _competition.Update(competition);
            try
            {
                List<TeamMatch> teamMatches = new List<TeamMatch>();
                foreach (var teamMatch in request)
                {
                    TeamMatch team_match = new TeamMatch
                    {
                        Id = teamMatch.teamMatchId,
                        TeamId = teamMatch.teamId,
                        NameDefault = teamMatch.teamName,
                        MatchId = teamMatch.matchId,
                        TotalScore = 0,
                        ResultPlayTable = 0,
                        ResultPlay = "0",

                    };
                    teamMatches.Add(team_match);
                }
                _teamMatchRepo.UpdateRange(teamMatches);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
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

        public async Task<SingleRsp> TeamStatistical(int matchId, int teamId)
        {
            var res = new SingleRsp();
            try
            {
                var time = ConvertToVietnamTime(DateTime.Now);
                var timePlay = _matchRepo.GetById(matchId);
                var totalTime = timePlay.StartDate + timePlay.TimeIn;
                var checkDate = time < timePlay.StartDate;
                TimeSpan LengthData = (TimeSpan)timePlay.TimeOut- (TimeSpan)timePlay.TimeIn;
                int totalMinutes = (int)LengthData.TotalMinutes;
                int[] minuteArray = Enumerable.Range(1, totalMinutes).ToArray();
                TimeSpan checkTime = (DateTime)totalTime - time;
                if (time.Date < timePlay.StartDate.Value.Date || checkTime.TotalMinutes > 15)
                {
                    //res.SetMessage("Trận đấu chưa diễn ra");
                    res.setData("data", "notstarted");
                }
                else
                  if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes <= 15 && checkTime.TotalMinutes > 0)
                {
                    var data = new
                    {
                        TimeAwait = checkTime,
                        TimeInMatch = timePlay.TimeIn

                    };
                    res.setData("data", data);
                    return res;
                }
                else if (time.Date == timePlay.StartDate.Value.Date && checkTime.TotalMinutes < 0 && time.TimeOfDay <= timePlay.TimeOut)
                {

                    var data = await _stemHub.AverageScoreActionClient(teamId,matchId, ConvertToVietnamTime(DateTime.Now));
                    res.setData("data", data.Message);
                }
                else
                {
                    var match = await _teamMatchRepo.getAverageScore(teamId,matchId);
                    var bonusAction = CalculateAverageScoresPerMinute(match.Where(a => a.ScoreCategory.Type == "Điểm cộng").ToList(), "bonus");
                    var MinusAction = CalculateAverageScoresPerMinute(match.Where(a => a.ScoreCategory.Type == "Điểm trừ").ToList(), "minus");
                    // danh sach trung binh diem cong theo phut
                    var rpsBosnusAction = Enumerable.Repeat(0.0, totalMinutes).ToArray(); 
                    foreach (var action in bonusAction)
                    {

                        rpsBosnusAction[action.Minute] = action.AverageScore;
                          
                    }
                    var rpsMinusAction = Enumerable.Repeat(0.0, totalMinutes).ToArray(); 
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
                    res.setData("data", rpsAverageScore);
                }
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception("Get List Referee Fail");
            }
            return res;

        }

        public DateTime ConvertToVietnamTime(DateTime serverTime)
        {

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime.ToUniversalTime(), vietnamTimeZone);

            return vietnamTime;
        }
    }
}
