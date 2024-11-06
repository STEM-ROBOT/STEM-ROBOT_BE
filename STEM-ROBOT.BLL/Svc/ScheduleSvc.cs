using AutoMapper;
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

namespace STEM_ROBOT.BLL.Svc
{
    public class ScheduleSvc
    {
        private readonly ScheduleRepo _scheduleRepo;
        private readonly CompetitionRepo _competition;
        private readonly IMapper _mapper;


        private readonly IMailService _mailService;
        public ScheduleSvc(ScheduleRepo scheduleRepo, IMapper mapper, IMailService mailService, CompetitionRepo competition)
        {
            _scheduleRepo = scheduleRepo;
            _competition = competition;
            _mapper = mapper;
            _mailService = mailService;
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
                    res.SetSuccess(lstRes, "200");
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

        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = _scheduleRepo.GetById(id);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    var scheduleRes = _mapper.Map<ScheduleRsp>(schedule);
                    res.setData("200", scheduleRes);
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
                res.setData("200", newSchedule);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Update(ScheduleReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = _scheduleRepo.GetById(id);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    _mapper.Map(req, schedule);
                    _scheduleRepo.Update(schedule);
                    res.setData("200", schedule);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Delete(int id)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = _scheduleRepo.GetById(id);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    _scheduleRepo.Delete(id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> SendMail(int ScheduleID, int accountID)
        {
            var res = new SingleRsp();
            try
            {
                var checkSchedule = await _scheduleRepo.CheckRefereeCompetition(ScheduleID, accountID);
                if (checkSchedule == null) throw new Exception("No data");
                var email = checkSchedule.RefereeCompetition.Referee.Email;
                if (email == null) throw new Exception("No email");

                Random random = new Random();
                int randomCode = random.Next(10000, 100000);

                var emailbody = $@"
                        <div><h3>MÃ KÍCH HOẠT GIẢI ĐẤU CỦA BẠN</h3> 
                        <div>
                            
                            <span>Mã của bạn : </span> <strong>{randomCode}</strong><br>
                           
                        </div>
                       
                        <div>
                            <span>Mã có hiệu lực trong 120 giây</strong>
                        </div>
                           
                        <p>STem Xin trân trọng cảm ơn bạn đã sử dụng dịch vụ</p>
                    </div>
                    ";
                var mail = new MailReq
                {
                    ToEmail = email,
                    Subject = $"[STEM PLATFORM]",
                    Body = emailbody

                };
                checkSchedule.OptCode = randomCode.ToString();
                checkSchedule.TimeOut = DateTime.Now.AddSeconds(120);
                _scheduleRepo.Update(checkSchedule);
                await _mailService.SendEmailAsync(mail);

            }
            catch (Exception ex)
            {
                throw new Exception("Fail to sendmail");
            }
            return res;
        }
        public async Task<SingleRsp> CheckCodeSchedule(int scheduleID, int accountID, string code)
        {
            var res = new SingleRsp();
            try
            {
                var checkSchedule = await _scheduleRepo.CheckTimeoutCodeSchedule(scheduleID, accountID, code);
                if (checkSchedule == null) throw new Exception("No data");
                if (checkSchedule.TimeOut < DateTime.Now) res.SetMessage("Time out code");
                if (checkSchedule.OptCode.Contains(code))
                {
                    res.SetMessage("Success");
                }
                else
                {
                    res.SetMessage("Code not found");
                    res.SetError("Code not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fail check code");
            }
            return res;
        }
        public async Task<SingleRsp> UpdateBusy(int scheduleID, int accountID)
        {
            var res = new SingleRsp();
            try
            {
                var account = await _scheduleRepo.getEmail(scheduleID);
                var schedule = await _scheduleRepo.UpdateBusy(scheduleID, accountID);
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
        public async Task<SingleRsp> CancelBusy(int scheduleID, int accountID)
        {
            var res = new SingleRsp();
            try
            {
                var schedule = await _scheduleRepo.UpdateBusy(scheduleID, accountID);
                if (schedule == null) throw new Exception("No data");
                schedule.Status = false;
                

            } catch (Exception ex)
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
                var schedule = await _scheduleRepo.GetRoundGameAsync(competitionId);
                if (schedule == null)
                {
                    res.SetError("404", "Schedule not found");
                }
                else
                {
                    var data = new ScheduleConfigRsp
                    {
                        MatchReferees = schedule.Where(s => s.Role == "SRF").Select(cr => new SchedulSubRefereeRsp
                        {
                            Id = cr.Id,
                            Name = cr.Referee.Name,
                        }).ToList(),

                        Referees = schedule.Where(s => s.Role == "MRF").Select(cs => new SchedulMainRefereeRsp
                        {
                            Id = cs.Id,
                            Name = cs.Referee.Name,
                        }).ToList(),

                        Rounds = schedule.FirstOrDefault().Competition.Stages.Select(s => new SchedulRoundsRefereeRsp
                        {
                            RoundId = s.Id,
                            roundName = s.Name,
                            Matches = s.Matches.Select(m => new SchedulRoundsMatchsRefereeRsp
                            {
                                matchId = m.Id,
                                timeIn = (TimeSpan)m.TimeIn,
                                date = (DateTime)m.StartDate,
                                arena = m.Location.Address,
                            }).ToList(),
                        }).ToList(),
                    };
                    res.setData("data", schedule);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
