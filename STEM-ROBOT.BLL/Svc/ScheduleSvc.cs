using AutoMapper;
using Google.Api.Gax;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Ocsp;
using STEM_ROBOT.BLL.Mail;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace STEM_ROBOT.BLL.Svc
{
    public class ScheduleSvc
    {
        private readonly ScheduleRepo _scheduleRepo;
        private readonly CompetitionRepo _competition;
        private readonly IMapper _mapper;
        private readonly MatchRepo _matchRepo;
        private readonly TeamMatchRepo _teamMatchRepo;

        private readonly IMailService _mailService;
        public ScheduleSvc(ScheduleRepo scheduleRepo, IMapper mapper, IMailService mailService, CompetitionRepo competition, MatchRepo matchRepo, TeamMatchRepo teamMatchRepo)
        {
            _scheduleRepo = scheduleRepo;
            _competition = competition;
            _mapper = mapper;
            _mailService = mailService;
            _matchRepo = matchRepo;
            _teamMatchRepo = teamMatchRepo;


        }

        public MutipleRsp GetSchedules()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _scheduleRepo.All();
                if (lst != null)
                {
                    var lstRes = _mapper.Map<List<ScheduleRsp>>(lst);
                    res.SetSuccess(lstRes, "data");
                }
                else
                {
                    res.SetError("404", "No data found");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetById(int Id)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = _scheduleRepo.GetById(Id);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    var scheduleRes = _mapper.Map<ScheduleRsp>(schedule);
                    res.setData("data", scheduleRes);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Create(ScheduleReq req)
        {
            var res = new SingleRsp();
            try
            {
                var newSchedule = _mapper.Map<Schedule>(req);
                _scheduleRepo.Add(newSchedule);
                res.setData("data", newSchedule);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Update(ScheduleReq req, int Id)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = _scheduleRepo.GetById(Id);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    _mapper.Map(req, schedule);
                    _scheduleRepo.Update(schedule);
                    res.setData("data", schedule);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Delete(int Id)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = _scheduleRepo.GetById(Id);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    _scheduleRepo.Delete(Id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> SendMail(int ScheduleId, int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var checkSchedule = await _scheduleRepo.CheckRefereeCompetition(ScheduleId, accountId);
                if (checkSchedule == null) throw new Exception("No data");
                var email = checkSchedule.RefereeCompetition.Referee.Email;
                if (email == null) throw new Exception("No email");

                Random random = new Random();
                int randomCode = random.Next(10000, 100000);

                var emailbody = $@"
                        <div><h3>MÃ XÁC THỰC LỊCH TRÌNH TRỌNG TÀI CHÍNH CỦA BẠN</h3> 
                        <div>
                            
                            <span>Mã xác thực : </span> <strong>{randomCode}</strong><br>
                           
                        </div>
                       
                        <div>
                            <span>Mã có hiệu lực trong 120 giây</strong>
                        </div>
                           
                        <p>Stem Xin trân trọng cảm ơn bạn đã sử dụng dịch vụ</p>
                    </div>
                    ";
                var mail = new MailReq
                {
                    ToEmail = email,
                    Subject = $"[STEM PLATFORM]",
                    Body = emailbody

                };
                checkSchedule.OptCode = randomCode.ToString();
                checkSchedule.TimeOut = ConvertToVietnamTime(DateTime.Now).AddSeconds(120);
                _scheduleRepo.Update(checkSchedule);
                await _mailService.SendEmailAsync(mail);

                var response = new ScheduleSecurityRsp
                {
                    timeOut = 120,
                    textView = checkSchedule.OptCode.Length
                };
                res.setData("data", response);
            }
            catch (Exception ex)
            {
                res.SetError("Gửi mã thất bại");
            }
            return res;
        }
        public async Task<SingleRsp> CheckCodeSchedule(int scheduleId, int accountId, string code)
        {
            var res = new SingleRsp();
            try
            {
                var checkSchedule = await _scheduleRepo.CheckTimeoutCodeSchedule(scheduleId, accountId);
                var checkUser = checkSchedule.RefereeCompetition.Referee.AccountId == accountId;
                if (!checkUser)
                {
                    res.SetMessage("Bạn không phải trọng tài của trận đấu này !");
                    return res;
                }
                else
                if (checkSchedule == null)
                {
                    res.SetMessage("Lịch trình không tồn tại !");
                    return res;
                }
                else
                if (checkSchedule.TimeOut < ConvertToVietnamTime(DateTime.Now))
                {
                    res.SetMessage("Mã hết hạn !");
                    return res;
                }
                else
                if (checkSchedule.OptCode.Equals(code))
                {
                    checkSchedule.IsJoin = true;
                    _scheduleRepo.Update(checkSchedule);
                    res.SetMessage("Success");

                }
                else
                {
                    res.SetMessage("Mã không đúng !");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Fail check code" + ex);
            }
            return res;
        }

        public async Task<SingleRsp> UpdateBusy(int scheduleId, int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var account = await _scheduleRepo.getEmail(scheduleId);
                var schedule = await _scheduleRepo.UpdateBusy(scheduleId, accountId);
                if (schedule == null) throw new Exception("No data");

                var email = schedule.RefereeCompetition?.Referee?.Email ?? "N/A";
                var emailModer = account?.Email ?? "N/A";
                var name = schedule.RefereeCompetition?.Referee?.Name ?? "N/A";

                DateTime startDate = (DateTime)(schedule.Match?.StartDate);
                TimeSpan timeIn = (TimeSpan)(schedule.Match?.TimeIn);
                TimeSpan timeOut = (TimeSpan)(schedule.Match?.TimeOut);

                schedule.Status = true;

                var emailbody = $@"
        <div style='font-family: Arial, sans-serif;'>
            <h3>THÔNG BÁO BẬN CỦA TRỌNG TÀI</h3> 

            <table style='width: 100%; border-collapse: collapse;'>
                <tr>
                    <td style='padding: 8px; border: 1px solid #ddd;'><strong>Trọng tài</strong></td>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{name}</td>
                </tr>
                <tr>
                    <td style='padding: 8px; border: 1px solid #ddd;'><strong>Email</strong></td>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{email}</td>
                </tr>
                <tr>
                    <td style='padding: 8px; border: 1px solid #ddd;'><strong>Ngày bắt đầu</strong></td>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{startDate}</td>
                </tr>
                <tr>
                    <td style='padding: 8px; border: 1px solid #ddd;'><strong>Thời gian vào sân</strong></td>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{timeIn}</td>
                </tr>
                <tr>
                    <td style='padding: 8px; border: 1px solid #ddd;'><strong>Thời gian ra sân</strong></td>
                    <td style='padding: 8px; border: 1px solid #ddd;'>{timeOut}</td>
                </tr>
            </table>

            <div style='margin-top: 16px;'>   
                <p>Bạn vui lòng bổ sung thêm trọng tài vào trận này nhé</p>
                <p>STEM xin trân trọng cảm ơn bạn đã sử dụng dịch vụ</p>
            </div>
        </div>
    ";

                var mail = new MailReq
                {
                    ToEmail = email,
                    Subject = "[STEM PLATFORM]",
                    Body = emailbody
                };

                await _mailService.SendEmailAsync(mail);
                _scheduleRepo.Update(schedule);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateBusy fail: " + ex.Message, ex);
            }
            return res;

        }
        public async Task<SingleRsp> CancelBusy(int scheduleId, int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = await _scheduleRepo.UpdateBusy(scheduleId, accountId);
                if (schedule == null) throw new Exception("No data");
                schedule.Status = false;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return res;
        }

        public async Task<SingleRsp> ScheduleCompetition(int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var scheduleMatch = await _scheduleRepo.GetRefereeGameAsync(competitionId);



                var schedule = await _scheduleRepo.GetRoundGameAsync(competitionId);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {

                    var data = new ScheduleConfigRsp
                    {
                        IsSchedule = schedule.IsSchedule,
                        MatchReferees = schedule.RefereeCompetitions.Where(s => s.Role == "SRF").Select(cr => new SchedulSubRefereeRsp
                        {
                            Id = cr.Id,
                            Name = cr.Referee.Name,
                        }).ToList(),

                        Referees = schedule.RefereeCompetitions.Where(s => s.Role == "MRF").Select(cs => new SchedulMainRefereeRsp
                        {
                            Id = cs.Id,
                            Name = cs.Referee.Name,
                        }).ToList(),

                        Rounds = schedule.Stages.Select(s => new SchedulRoundsRefereeRsp
                        {
                            RoundId = s.Id,
                            roundName = s.Name,
                            Matches = s.Matches.Select(m => new SchedulRoundsMatchsRefereeRsp
                            {
                                matchId = m.Id,
                                timeIn = (TimeSpan)m.TimeIn,
                                date = (DateTime)m.StartDate,
                                arena = m.Location.Address,
                                mainReferee = scheduleMatch.Count > 0 ? (int)scheduleMatch.Where(sc => sc.RefereeCompetition.Role == "MRF" && sc.MatchId == m.Id).FirstOrDefault().RefereeCompetitionId : 0,
                                mainRefereeName = scheduleMatch.Count > 0 ? scheduleMatch.Where(sc => sc.RefereeCompetition.Role == "MRF" && sc.MatchId == m.Id).FirstOrDefault().RefereeCompetition.Referee.Name : "",
                                matchRefereesdata = scheduleMatch.Count > 0 ? scheduleMatch.Where(sc => sc.RefereeCompetition.Role == "SRF" && sc.MatchId == m.Id).ToList().Select(rs => new SchedulMainMatchRefereeRsp
                                {
                                    SubRefereeId = rs.RefereeCompetition.Id,
                                    SubRefereeName = rs.RefereeCompetition.Referee.Name,
                                }).ToList() : new List<SchedulMainMatchRefereeRsp>(),
                            }).ToList(),
                        }).ToList(),
                    };
                    res.setData("data", data);

                }
            }

            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public async Task<SingleRsp> updateScheduleConfigCompetition(int competitionId, List<ScheduleReq> reques)
        {
            var res = new SingleRsp();
            var competition = _competition.GetById(competitionId);
            competition.IsSchedule = true;
            var list_schedule = _mapper.Map<List<Schedule>>(reques);
            _competition.Update(competition);
            _scheduleRepo.AddRange(list_schedule);
            return res;
        }


        //check schedule

        public async Task<SingleRsp> checkTimeSchedule(int scheDuleId, int accountId)
        {
            var res = new SingleRsp();
            var date = ConvertToVietnamTime(DateTime.Now);
            try
            {
                var scheduledata = await _scheduleRepo.checkTimeschedule(scheDuleId, accountId);
                if (scheduledata != null)
                {
                    var schedule = scheduledata.Match.TeamMatches.Select(a => new CheckTimeSchedule
                    {
                        matchId = (int)scheduledata.MatchId,
                        teamMatchId = a.Id
                    }).ToList();
                    var matchID = scheduledata.MatchId;

                    var matchCheck = _matchRepo.GetById(matchID);

                    var totalTime = matchCheck.StartDate + matchCheck.TimeIn;

                    var checkDate = date < matchCheck.StartDate;

                    TimeSpan checkTime = (DateTime)totalTime - date;

                    if (date.Date != matchCheck.StartDate.Value.Date || checkTime.TotalMinutes > 15)
                    {

                        res.setData("data", "error");

                    }
                    else if (checkTime.TotalMinutes > 15)
                    {

                        res.setData("data", "error");

                    }
                    else
                    if (checkTime.TotalMinutes <= 15 || checkTime.TotalMinutes <= 0 && date.TimeOfDay <= matchCheck.TimeOut)
                    {
                        var timeAwait = checkTime.TotalMinutes < 0 ? TimeSpan.Zero : checkTime;
                        var data = new
                        {
                            TimeAwait = timeAwait,
                            TimeInMatch = matchCheck.TimeIn,
                            TimeOutMatch = matchCheck.TimeOut,
                            numberHaft = matchCheck.NumberHaft,
                            breakTimeHaft = matchCheck.BreakTimeHaft,
                            Isjoin = scheduledata.IsJoin,
                            matchId = scheduledata.MatchId,
                            scheduleData = schedule
                        };
                        res.setData("data", data);
                        return res;
                    }
                }
                else
                {
                    res.setData("data", "error");
                }





            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> ScheduleSupReferee(int refereeCompetitionId, int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var refe = await _scheduleRepo.CheckSupRefereeCompetition(refereeCompetitionId, accountId);
                if (refe == null)
                {
                    res.SetError("data", "Ban khong phai trong tai vien cua tran dau");
                }
                else
                {
                    var data = await _scheduleRepo.ScheduleSupRefereeCompetition(refereeCompetitionId);
                    if (data == null)
                    {
                        throw new Exception("Not found");
                    }
                    int competitionId = (int)data[0].RefereeCompetition.CompetitionId;
                    var locatons = await _scheduleRepo.CheckLocationCompetition(competitionId);
                    var dataRes = new
                    {
                        scheduleReferee = data.Select(sc => new ScheduleSupReferee
                        {
                            Id = sc.Id,
                            location = locatons.Where(l => l.Id == sc.Match.LocationId).FirstOrDefault().Address,
                            matchId = (int)sc.MatchId,
                            StartTime = sc.Match.StartDate.Value.Add(sc.Match.TimeIn.Value).ToString("yyyy-MM-ddTHH:mm:ss"),
                            EndTime = sc.Match.StartDate.Value.Add(sc.Match.TimeOut.Value).ToString("yyyy-MM-ddTHH:mm:ss"),
                            status = CheckMatchStatus(sc.Match.StartDate.Value.Add(sc.Match.TimeIn.Value), sc.Match.StartDate.Value.Add(sc.Match.TimeOut.Value)),
                            teamMatch = sc.Match.TeamMatches.Select(tm => new TeamMatchReferee
                            {
                                teamId = tm.Id,
                                teamLogo = tm.TeamId != null ? tm.Team.Image : "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png",
                                teamType = tm.TeamId != null ? tm.Team.Name : tm.NameDefault,
                            }).ToList(),

                        }).ToList(),
                    };
                    res.setData("data", dataRes);
                }

            }
            catch (Exception ex)
            {
                res.setData("data", "error" + ex);
            }
            return res;
        }
        public async Task<SingleRsp> ScheduleSupRefereeMatchInfo(int scheduleId, int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = await _scheduleRepo.SupRefereeCheck(scheduleId);
                int matchId = (int)schedule.MatchId;
                int competitionId = (int)schedule.RefereeCompetition.CompetitionId;
                var matchData = await _scheduleRepo.SupRefereeCompetitionMatchInfo(matchId);
                var scoreData = await _scheduleRepo.CompetitionScore(competitionId);
                var dataRes = new
                {
                    score = scoreData.Select(sc => new ScheduleMatchScoreRsp
                    {
                        Description = sc.Description,
                        Point = sc.Point.ToString(),
                        Type = sc.Type,
                        scoreId = sc.Id

                    }).ToList(),
                    matchInfo = new ScheduleMatchInfoRsp
                    {
                        matchId = matchData.Id,
                        breakHaftTime = matchData.BreakTimeHaft.ToString(),
                        durationHaft = matchData.TimeOfHaft.Value.ToString(),
                        endTime = matchData.TimeOut.Value.ToString(),
                        startTime = matchData.TimeIn.Value.ToString(),
                        startDate = matchData.StartDate.Value.ToString(),
                        haftMatch = matchData.MatchHalves.Select(h => new ScheduleMatchHaftRsp
                        {
                            HaftId = h.Id,
                            HaftName = h.HalfName

                        }).ToList(),
                        teamMatch = matchData.TeamMatches.Select(tm => new ScheduleMatchTeamMatchRsp
                        {
                            teamLogo = tm.TeamId != null ? tm.Team.Image : "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png",
                            teamMatchId = tm.Id,
                            teamName = tm.TeamId != null ? tm.Team.Name : tm.NameDefault,

                        }).ToList()
                    }
                };
                res.setData("data", dataRes);
            }
            catch (Exception ex)
            {
                res.setData("data", "error" + ex);
            }
            return res;
        }
        public DateTime ConvertToVietnamTime(DateTime serverTime)
        {
            // Lấy thông tin múi giờ Việt Nam (UTC+7)
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển đổi từ thời gian server sang thời gian Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime.ToUniversalTime(), vietnamTimeZone);

            return vietnamTime;
        }
        public bool CheckMatchStatus(DateTime startTime, DateTime endTime)
        {
            DateTime currentTime = ConvertToVietnamTime(DateTime.Now);

            // Kiểm tra nếu thời gian hiện tại nằm trong khoảng 15 phút trước StartTime hoặc trong thời gian trận đấu
            if (currentTime >= startTime.AddMinutes(-15) && currentTime <= endTime)
            {
                return true;
            }
            return false;
        }
        public class TeamResult
        {
            public int TeamId { get; set; }
            public string TableName { get; set; }
            public string MatchWinCode { get; set; }
            public int Score { get; set; }
        }
        public async Task<SingleRsp> ConfirmMatchRandom(int scheduleID, int accountId, ScheduleRandomReq req)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = await _scheduleRepo.confirmSchedule(scheduleID, accountId);
                if (schedule == null)
                {
                    res.Setmessage("lich trinh khong ton tai");
                }
                else
                {
                    var teamUpdate = new List<TeamMatch>();
                    var teamWin = _teamMatchRepo.GetById(req.teamMatchWinId);
                    teamWin.TeamId = req.teamId;
                    teamUpdate.Add(teamWin);
                    foreach (var teamMatch in req.TeamMatchs)
                    {
                        var teamItem = _teamMatchRepo.GetById(teamMatch.Id);
                        if (teamMatch.Id == req.teamMatchRandomId)
                        {
                            teamItem.ResultPlay = "Win";
                            teamItem.IsPlay = true;
                        }
                        else
                        {
                            teamItem.ResultPlay = "Lose";
                            teamItem.IsPlay = true;
                        }
                        teamItem.HitCount = teamMatch.HitCount;
                        teamUpdate.Add(teamItem);

                    }
                    _teamMatchRepo.UpdateRange(teamUpdate);
                    res.SetMessage("success");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
        //confirm schedule
        public async Task<SingleRsp> ConfirmSchedule(int scheduleID, int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = await _scheduleRepo.confirmSchedule(scheduleID, accountId);

                if (schedule == null)
                {
                    res.Setmessage("lich trinh khong ton tai");
                }
                else if (schedule.Match.Stage.StageMode == "Vòng bảng")
                {
                    var competition = _competition.GetById(schedule.Match.Stage.CompetitionId);
                    var team1 = schedule.Match.TeamMatches.FirstOrDefault();
                    var team2 = schedule.Match.TeamMatches.LastOrDefault();

                    if (team1.TotalScore > team2.TotalScore)
                    {

                        var listTeam = new List<TeamMatch>();
                        team1.ResultPlayTable = competition.WinScore;
                        team1.IsPlay = true;
                        team1.ResultPlay = "Win";
                        listTeam.Add(team1);
                        team2.ResultPlayTable = competition.LoseScore;
                        team2.IsPlay = true;
                        team2.ResultPlay = "Lose";
                        listTeam.Add(team2);
                        _teamMatchRepo.UpdateRange(listTeam);
                    }
                    else if (team1.TotalScore == team2.TotalScore)
                    {
                        var listTeam = new List<TeamMatch>();
                        team1.IsPlay = true;
                        team1.ResultPlay = "Draw";
                        team1.ResultPlayTable = competition.TieScore;
                        listTeam.Add(team1);
                        //capj nhat team 2
                        team2.IsPlay = true;
                        team2.ResultPlay = "Draw";
                        team2.ResultPlayTable = competition.TieScore;
                        listTeam.Add(team2);
                        _teamMatchRepo.UpdateRange(listTeam);
                    }
                    else
                    {
                        var listTeam = new List<TeamMatch>();
                        team1.ResultPlayTable = competition.LoseScore;
                        team1.IsPlay = true;
                        team1.ResultPlay = "Lose";
                        listTeam.Add(team1);
                        team2.ResultPlayTable = competition.WinScore;
                        team2.IsPlay = true;
                        team2.ResultPlay = "Win";
                        listTeam.Add(team2);
                        _teamMatchRepo.UpdateRange(listTeam);
                    }


                    //kiem tra co phai tran dau cuoi cung cua bang khong
                    var matchLast = await _scheduleRepo.checkMatchLast((int)schedule.Match.TableGroupId);
                    var matchLastcheck = matchLast.LastOrDefault();
                    if ((int)schedule.MatchId == matchLastcheck.Id)
                    {
                        var matchtable = await _scheduleRepo.checkTableMatch((int)schedule.MatchId);
                        if (matchLast != null)
                        {
                            int teamNextRound = (int)matchtable.TeamNextRoud;
                       
                            // lay danh sach team trong bangr
                            var listTeamTable = matchtable.TeamTables;

                            //sap xep theo thu tu diem giamr dan
                            var topTeams = listTeamTable
                            .OrderByDescending(t => t.Team.TeamMatches.Sum(tm => tm.ResultPlayTable ?? 0))                     
                            .ToList();

                            // mang xu li 
                            var listBackup = topTeams;

                            for (int i = 0; i< teamNextRound; i++)
                            {
                                var topSame = listBackup[i];

                                int sameScoreCount = topTeams
                                    .Count(t => t.Team.TeamMatches.Sum(tm => tm.ResultPlayTable ?? 0) == topSame.Team.TeamMatches.Sum(tm => tm.ResultPlayTable ?? 0));
                                if( sameScoreCount > 1 )
                                {

                                }
                                else {
                                    var MatchCode = $"T#{i+1}B#{matchtable.Name}";
                                    var teamMatchWin = await _scheduleRepo.matchWinSchedule(MatchCode);
                                    teamMatchWin.TeamId = topSame.TeamId;
                                    _teamMatchRepo.Update(teamMatchWin);
                                }
                            }
                            
   
                        }

                    }
                    res.SetMessage("success");

                }
                else
                {
                    var teamMatchWin = await _scheduleRepo.matchWinSchedule(schedule.Match.MatchCode);
                    var team1 = schedule.Match.TeamMatches.FirstOrDefault();
                    var team2 = schedule.Match.TeamMatches.LastOrDefault();
                    var listTeamMatch = new List<TeamMatch>();
                    if (team1.TotalScore == team2.TotalScore)
                    {
                        // Tính trung bình cộng của listBonusTeam
                        var listBonusTeam1 = team1.Actions.Where(a => a.ScoreCategory.Type.ToLower() == "điểm cộng").ToList();
                        var listBonusTeam2 = team2.Actions.Where(a => a.ScoreCategory.Type.ToLower() == "điểm cộng").ToList();

                        team1.AverageBonus = listBonusTeam1.Count > 0 ? listBonusTeam1.Average(a => a.ScoreCategory.Point ?? 0) : 0;
                        team2.AverageBonus = listBonusTeam2.Count > 0 ? listBonusTeam2.Average(a => a.ScoreCategory.Point ?? 0) : 0;

                        // Tính trung bình cộng của listMinusTeam
                        var listMinusTeam1 = team1.Actions.Where(a => a.ScoreCategory.Type.ToLower() == "điểm trừ").ToList();
                        var listMinusTeam2 = team2.Actions.Where(a => a.ScoreCategory.Type.ToLower() == "điểm trừ").ToList();

                        team1.AverageMinus = listMinusTeam1.Count > 0 ? listMinusTeam1.Average(a => a.ScoreCategory.Point ?? 0) : 0;
                        team2.AverageMinus = listMinusTeam2.Count > 0 ? listMinusTeam2.Average(a => a.ScoreCategory.Point ?? 0) : 0;


                        if (team1.AverageBonus > team2.AverageBonus)
                        {
                            team1.IsPlay = true;
                            team1.ResultPlay = "Win";
                            //capj nhat team 2
                            team2.IsPlay = true;
                            team2.ResultPlay = "Lose";
                            teamMatchWin.TeamId = team1.TeamId;
                            listTeamMatch.Add(teamMatchWin);
                            listTeamMatch.Add(team1);
                            listTeamMatch.Add(team2);
                            _teamMatchRepo.UpdateRange(listTeamMatch);
                            res.SetMessage("success");
                        }
                        else if (team1.AverageBonus < team2.AverageBonus)
                        {
                            team1.IsPlay = true;
                            team1.ResultPlay = "Lose";
                            //capj nhat team 2
                            team2.IsPlay = true;
                            team2.ResultPlay = "Win";
                            teamMatchWin.TeamId = team2.TeamId;
                            listTeamMatch.Add(teamMatchWin);
                            listTeamMatch.Add(team1);
                            listTeamMatch.Add(team2);
                            _teamMatchRepo.UpdateRange(listTeamMatch);
                            res.SetMessage("success");
                        }
                        else if (team2.AverageBonus == team1.AverageBonus)
                        {

                            if (team1.AverageMinus < team2.AverageMinus)
                            {
                                team1.IsPlay = true;
                                team1.ResultPlay = "Win";
                                //capj nhat team 2
                                team2.IsPlay = true;
                                team2.ResultPlay = "Lose";
                                teamMatchWin.TeamId = team1.TeamId;
                                listTeamMatch.Add(teamMatchWin);
                                listTeamMatch.Add(team1);
                                listTeamMatch.Add(team2);
                                _teamMatchRepo.UpdateRange(listTeamMatch);
                                res.SetMessage("success");
                            }
                            else if (team1.AverageMinus > team2.AverageMinus)
                            {
                                team1.IsPlay = true;
                                team1.ResultPlay = "Lose";
                                //capj nhat team 2
                                team2.IsPlay = true;
                                team2.ResultPlay = "Win";
                                teamMatchWin.TeamId = team2.TeamId;
                                listTeamMatch.Add(teamMatchWin);
                                listTeamMatch.Add(team1);
                                listTeamMatch.Add(team2);
                                _teamMatchRepo.UpdateRange(listTeamMatch);
                                res.SetMessage("success");
                            }
                            else if (team1.AverageMinus == team2.AverageMinus)
                            {

                                var data = new MatchDataScheduleConfirm
                                {
                                    formatName = "Loại Trực Tiếp",
                                    formatType = "knock-out",
                                    teamMatchWinId = teamMatchWin.Id,
                                    teamRanDom = schedule.Match.TeamMatches.Select(ac => new RandomTeamWinRsp
                                    {
                                        averageMinus = (double)ac.AverageMinus,
                                        averageBonus = (double)ac.AverageBonus,
                                        tolalScore = (int)ac.TotalScore,
                                        teamImage = ac.Team.Image,
                                        teamMatchId = ac.Id,
                                        teamName = ac.Team.Name,
                                        teamId = (int)ac.TeamId
                                    }).ToList(),
                                };
                                _teamMatchRepo.UpdateRange(schedule.Match.TeamMatches);
                                res.setData("data", data);
                                res.SetMessage("randome");
                            }
                        }


                    }
                    else if (team1.TotalScore > team2.TotalScore)
                    {//capj nhat team 1
                        team1.IsPlay = true;
                        team1.ResultPlay = "Win";
                        //capj nhat team 2b
                        team2.IsPlay = true;
                        team2.ResultPlay = "Lose";
                        teamMatchWin.TeamId = team1.TeamId;
                        listTeamMatch.Add(teamMatchWin);
                        listTeamMatch.Add(team1);
                        listTeamMatch.Add(team2);
                        _teamMatchRepo.UpdateRange(listTeamMatch);
                        res.SetMessage("success");
                    }
                    else
                    {
                        team1.IsPlay = true;
                        team1.ResultPlay = "Lose";
                        //capj nhat team 2
                        team2.IsPlay = true;
                        team2.ResultPlay = "Win";
                        teamMatchWin.TeamId = team2.TeamId;
                        listTeamMatch.Add(teamMatchWin);
                        listTeamMatch.Add(team1);
                        listTeamMatch.Add(team2);
                        _teamMatchRepo.UpdateRange(listTeamMatch);
                        res.SetMessage("success");

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
    }
}
