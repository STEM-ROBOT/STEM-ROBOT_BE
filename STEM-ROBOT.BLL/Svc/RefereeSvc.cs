﻿using AutoMapper;
using STEM_ROBOT.BLL.Mail;
using STEM_ROBOT.Common.BLL;
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
    public class RefereeSvc
    {

        private readonly RefereeRepo _refereeRepo;
        private readonly IMapper _mapper;
        private readonly TournamentRepo _tournamentRepo;
        private readonly RefereeCompetitionRepo _refereeCompetitionRepo;
        private readonly ScheduleRepo _scheduleRepo;
        private readonly CompetitionRepo _competitionRepo;
        private readonly IMailService _mailSerivce;
        private readonly AccountRepo _accountRepo;
        public RefereeSvc(RefereeRepo refereeRepo, IMapper mapper, RefereeCompetitionRepo refereeCompetitionRepo, ScheduleRepo scheduleRepo, CompetitionRepo competitionRepo, IMailService mailSerivce, AccountRepo accountRepo, TournamentRepo tournamentRepo)
        {
            _refereeRepo = refereeRepo;
            _mapper = mapper;
            _refereeCompetitionRepo = refereeCompetitionRepo;
            _scheduleRepo = scheduleRepo;
            _competitionRepo = competitionRepo;
            _mailSerivce = mailSerivce;
            _accountRepo = accountRepo;
            _tournamentRepo = tournamentRepo;
        }

        public MutipleRsp GetReferees()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _refereeRepo.All();
                if (lst == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    var lstRes = _mapper.Map<List<ListRefereeRsp>>(lst);
                    res.SetSuccess(lstRes, "data");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> ListRefereeTournament(int userId)
        {
            var res = new MutipleRsp();
            try
            {
                var data = await _refereeRepo.GetListReferee(userId);
                if (data == null) throw new Exception("No data");
                var mapper = _mapper.Map<RefereeTournament>(data);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception("Get list Fail");
            }
            return res;
        }
        public async Task<MutipleRsp> ListSupRefereeTournament(int userId)
        {
            var res = new MutipleRsp();
            try
            {
                var refe = await _refereeRepo.GetRefereeInfo(userId);
                var refeInfo = new SupRefereeTournament
                {
                    Image = refe.Image,
                    Name = refe.Name,
                    Role = "Trọng tài viên"
                };
                var data = await _refereeRepo.GetListSupReferee(refe.Id);
                List<SupRefereeCompetitionTournament> dateCompe = data.Select(cp => new SupRefereeCompetitionTournament
                {
                    Id = cp.Id,
                    Image = cp.Competition.Genre.Image,
                    CompetitionName = cp.Competition.Genre.Name,
                    TournamentName = refe.Tournament.Name,
                    Location = refe.Tournament.Location,
                }).ToList();
                if (data == null) throw new Exception("No data");
                var resData = new
                {
                    infoReferee = refeInfo,
                    competitions = dateCompe
                };
                res.SetData("data", resData);
            }
            catch (Exception ex)
            {
                throw new Exception("Get list Fail");
            }
            return res;
        }

        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var referee = _refereeRepo.GetById(id);
                if (referee == null)
                {
                    res.SetError("404", "Referee not found");
                }
                else
                {
                    var refereeRes = _mapper.Map<ListRefereeRsp>(referee);
                    res.setData("data", refereeRes);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(RefereeReq req)
        {
            var res = new SingleRsp();
            try
            {
                var existingAccount = _refereeRepo.Find(a => a.Email == req.Email).FirstOrDefault();
                if (existingAccount != null)
                {
                    res.SetError("400", "Email already exists");
                    return res;
                }
                var newReferee = _mapper.Map<Referee>(req);
                _refereeRepo.Add(newReferee);
                res.setData("data", newReferee);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> AddListReferee(List<RefereeReq> referees)
        {
            var res = new MutipleRsp();
            try
            {
                var refereeList = new List<Referee>();
                var accoutList = new List<Account>();
                var tournament = _tournamentRepo.GetById(referees[0].TournamentId);
                foreach (var item in referees)
                {

                    string email;

                    Random random = new Random();
                    int number = random.Next(10000000, 100000000);
                    email = $"{item.TournamentId}{item.PhoneNumber}{number}@referee.stem.vn";

                    Random randoms = new Random();
                    int numberPass = randoms.Next(0, 10000);
                    string password = $"Referee{item.TournamentId}{numberPass}";
                    var passwords = BCrypt.Net.BCrypt.HashPassword(password);
                    var account = new Account
                    {
                        Email = email,
                        Password = passwords,
                        Role = "RF",
                        PhoneNumber = item.PhoneNumber,
                        Image = item.Image,
                        Name = item.Name,



                    };

                    _accountRepo.Add(account);
                    var referee = new Referee
                    {
                        TournamentId = item.TournamentId,
                        Name = string.IsNullOrEmpty(item.Name) ? "No data" : item.Name,
                        Email = string.IsNullOrEmpty(item.Email) ? "No data" : item.Email,
                        Status = string.IsNullOrEmpty(item.Status) ? "No data" : item.Status,
                        PhoneNumber = string.IsNullOrEmpty(item.PhoneNumber) ? "No data" : item.PhoneNumber,
                        Image = string.IsNullOrEmpty(item.Image) ? "No data" : item.Image,
                        AccountId = account.Id,
                    };

                    refereeList.Add(referee);
                    var emailbody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 5px;
            border: 1px solid #ddd;
        }}
        .header {{
            text-align: left;
            padding-bottom: 10px;
            border-bottom: 1px solid #ddd;
            margin-bottom: 20px;
        }}
        .header img {{
            max-width: 150px;
        }}
        .content {{
            color: #333333;
            font-size: 16px;
            line-height: 1.5;
        }}
        .content h3 {{
            font-size: 18px;
            color: #000;
        }}
        .details {{
            margin-top: 20px;
            padding: 10px;
            background-color: #f4f4f4;
            border: 1px solid #ddd;
        }}
        .footer {{
            text-align: center;
            color: #666666;
            font-size: 14px;
            margin-top: 20px;
            padding-top: 10px;
            border-top: 1px solid #ddd;
        }}
        .link {{
            color: #007bff;
            text-decoration: none;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='https://firebasestorage.googleapis.com/v0/b/fine-acronym-438603-m5.firebasestorage.app/o/stem-sever%2Flogo-dask.png?alt=media&token=f1ac1eeb-4acc-402e-b11b-080f442d55bf' alt='STEM'>
        </div>
        <div class='content'>
            <p>Kính gửi {referee.Email} ,</p>
            <p>Bạn đã được phân công trong  giải đấu {tournament.Name}.</p>
            <h3>Chi tiết thông tin đăng nhập</h3>
            <div class='details'>
                <p><strong>STEM</strong></p>              
            <br>
            <table style='width:100%; border-collapse: collapse; margin-top: 20px;'>
                <tr>
                    <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Email</th>
                    <th style='border: 1px solid #ddd; padding: 8px; background-color: #f2f2f2;'>Password</th>
                </tr>
                <tr>
                    <td style='border: 1px solid #ddd; padding: 8px;'>{account.Email}</td>
                    <td style='border: 1px solid #ddd; padding: 8px;'>{password}</td>
                </tr>
            </table>
            </div>
        </div>
        <div class='footer'>
            <p>---<br>STEMSYSTEM<br><a href='http://157.66.27.69:5173/' class='link'>http://stem-robot.com</a></p>
            
            <p>Copyright © 2021 STEM, All rights reserved.</p>
        </div>
    </div>
</body>
</html>";



                    var mailRequest = new MailReq()
                    {
                        ToEmail = referee.Email,
                        Subject = "[STEM SYSTEM]",
                        Body = emailbody
                    };

                    await _mailSerivce.SendEmailAsync(mailRequest);




                }
                _refereeRepo.AddRange(refereeList);
                res.SetData("data", "success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Update(RefereeReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var referee = _refereeRepo.GetById(id);
                if (referee == null)
                {
                    res.SetError("404", "Referee not found");
                }
                else
                {
                    var existingAccount = _refereeRepo.Find(a => a.Email == req.Email && a.Id != id).FirstOrDefault();
                    if (existingAccount != null)
                    {
                        res.SetError("400", "Email already exists");
                        return res;
                    }

                    _mapper.Map(req, referee);
                    _refereeRepo.Update(referee);
                    res.setData("data", referee);
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
                var referee = _refereeRepo.GetById(id);
                if (referee == null)
                {
                    res.SetError("404", "Referee not found");
                }
                else
                {
                    _refereeRepo.Delete(id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetListRefereeByTournament(int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _refereeRepo.All().Where(x => x.TournamentId == tournamentId).ToList();
                if (lst == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    var lstRes = _mapper.Map<List<ListRefereeRsp>>(lst);
                    res.SetSuccess(lstRes, "success");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetListRefereeAvailable(int tournamentId, int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.All(includeProperties: "Locations", filter: x => x.Id == competitionId).FirstOrDefault();

                if (competition == null)
                {
                    res.SetError("404", "Competition not found");
                    return res;
                }
                if (competition.IsReferee == true)
                {
                    var refereeCompetition = _refereeCompetitionRepo.All(
                        includeProperties: "Referee",
                        filter: x => x.CompetitionId == competitionId
                        ).ToList();
                    var lstReferee = new RefereeRsp();
                    lstReferee.NumberLocation = competition.Locations.Count;
                    lstReferee.IsReferee = (bool)competition.IsReferee;
                    foreach (var item in refereeCompetition)
                    {
                        var referee = _refereeRepo.Find(x => x.Id == item.RefereeId).FirstOrDefault();
                        var refereeRsp = _mapper.Map<ListRefereeRsp>(referee);
                        refereeRsp.Role = item.Role;
                        lstReferee.listRefereeRsps.Add(refereeRsp);
                    }
                    res.setData("data", lstReferee);
                    res.SetMessage("True");
                }
                else
                {
                    var lstReferees = _refereeRepo.All().Where(x => x.TournamentId == tournamentId).ToList();

                    var availableReferee = new RefereeRsp();
                    availableReferee.NumberLocation = competition.Locations.Count;
                    availableReferee.IsReferee = (bool)competition.IsReferee;
                    foreach (var referee in lstReferees)
                    {
                        bool isAvailable = true;
                        var lstRefereeCompetition = _refereeCompetitionRepo.All(x => x.RefereeId == referee.Id).ToList();

                        foreach (var item in lstRefereeCompetition)
                        {
                            var competitionEndTime = _competitionRepo.Find(x => x.Id == item.CompetitionId).Select(x => x.EndTime).FirstOrDefault();
                            if (competitionEndTime.HasValue && competitionEndTime.Value > DateTime.Now)
                            {
                                isAvailable = false;
                                break;
                            }
                        }
                        if (isAvailable)
                        {
                            var refereeRsp = _mapper.Map<ListRefereeRsp>(referee);

                            availableReferee.listRefereeRsps.Add(refereeRsp);
                        }
                    }

                    res.setData("data", availableReferee);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp AssignRefereeInCompetition(int competitionId, List<AssginRefereeReq> referees, int numberTeamReferee, int numberSubReferee)
        {
            var res = new MutipleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                if (competition == null)
                {
                    res.SetError("404", "Competition not found");
                    return res;
                }
                competition.IsReferee = true;
                competition.NumberSubReferee = numberSubReferee;
                competition.NumberTeamReferee = numberTeamReferee;
                _competitionRepo.Update(competition);
                var refereeCompetitionList = new List<RefereeCompetition>();
                foreach (var referee in referees)
                {
                    var refereeCompetition = new RefereeCompetition
                    {
                        CompetitionId = competitionId,
                        RefereeId = referee.RefereeId,
                        Role = referee.Role
                    };
                    refereeCompetitionList.Add(refereeCompetition);
                    _refereeCompetitionRepo.Add(refereeCompetition);
                }
                var refereeCompetitionListRsp = _mapper.Map<List<RefereeCompetitionRsp>>(refereeCompetitionList);
                res.SetData("data", refereeCompetitionListRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }

}