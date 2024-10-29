using AutoMapper;


using Microsoft.AspNetCore.SignalR;
using NetTopologySuite.Utilities;
using STEM_ROBOT.BLL.Mail;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class TournamentSvc
    {
        private readonly IMapper _mapper;
        private readonly TournamentRepo _tournament;
        //  public IHubContext<TournamentClient> _hubContext;
        private readonly IMailService _mailService;
        private readonly AccountRepo _account;
        private readonly ContestantRepo _contestantRepo;
        private readonly CompetitionRepo _competitionRepo;
        private readonly TeamRepo _teamRepo;
        public TournamentSvc(TournamentRepo tournamentRepo, IMapper mapper, IMailService mailService, AccountRepo account, ContestantRepo contestantRepo, CompetitionRepo competitionRepo, TeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
            _mapper = mapper;
            _tournament = tournamentRepo;
            _mailService = mailService;
            _account = account;
            _contestantRepo = contestantRepo;
            _competitionRepo = competitionRepo;
        }

        public async Task<SingleRsp> getStatus(int id)
        {

            var res = new SingleRsp();
            try
            {
                var tournament = _tournament.GetById(id);
                if (tournament == null) throw new Exception("No ID");
                if (tournament.Status == "Pending")
                {
                    res.setData("data", tournament);
                    return res;
                }
                throw new Exception("Not Pending");
            }
            catch (Exception ex)
            {
                throw new Exception("Fail data");
            }
        }
        public async Task<SingleRsp> AddTournement(int userID, TournamentReq request)
        {
            var res = new SingleRsp();
            try
            {


                var user = _account.GetById(userID);


                var tournament = _mapper.Map<Tournament>(request);
                tournament.AccountId = userID;
                var userName = user.Name;
                var email = user.Email;
                _tournament.Add(tournament);
                var emailbody = $@"
                        <div><h3>THÔNG TIN GIẢI ĐẤU CỦA BẠN</h3> 
                        <div>
                            
                            <span>Tên người đăng kí giải: </span> <strong>{userName}</strong><br>
                           
                        </div>
                       
                        <div>
                            <span>Bạn hãy kiểm tra email thường xuyên để nhận thông báo về số lượng team đăng kí giải nhé !!!</strong>
                        </div>
                           
                        <p>STem Xin trân trọng cảm ơn bạn đã sử dụng dịch vụ</p>
                    </div>
                    ";

                var mailRequest = new MailReq()
                {
                    ToEmail = email,
                    Subject = "[STEM PLATFORM]",
                    Body = emailbody
                };

                await _mailService.SendEmailAsync(mailRequest);

                res.Setmessage("data");

            }
            catch (Exception ex)
            {
                throw new Exception("Fail add tonament");
            }
            return res;
        }

        public async Task<MutipleRsp> GetTournament(string? name = null, string? status = null, int? competitionId = null, int page = 1, int pageSize = 10)
        {
            var res = new MutipleRsp();
            try
            {
                var listTournament = await _tournament.GetListTournament(name, status, competitionId, page, pageSize);
                if (listTournament == null) throw new Exception("Please Check Againt");
                res.SetData("data", listTournament);
            }
            catch (Exception ex)
            {
                throw new Exception("Get ListFail");
            }
            return res;
        }
        //list tournament 
        public async Task<MutipleRsp> getListTournamentModerator(int userID)
        {
            var res = new MutipleRsp();
            try
            {
                var listTournament = await _tournament.getTournamentModerator(userID);
                if (listTournament == null) throw new Exception("Please Check Againt");
                res.SetData("data", listTournament);
            }
            catch (Exception ex)
            {
                throw new Exception("Get ListFail");
            }
            return res;
        }
        //sum contestant

        //public async Task<SingleRsp> CountTimeTournament(int tourNamentID)
        //{
        //    var res = new SingleRsp();
        //    try
        //    {
        //        var tournament = _tournament.getID(tourNamentID);
        //        if(tournament == null)
        //        {
        //            res.SetError("No TournamentID");

        //        }
        //        await _hubContext.Clients.All.SendAsync("ReceiveCountdown", tournament.EndDate);

        //    }
        //    catch(Exception ex)
        //    {
        //        res.SetError("500", ex.Message);
        //    }
        //    return res;
        //}

        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var tournament = _tournament.GetById(id);
                if (tournament == null)
                {
                    res.SetError("404", "No ID");
                };
                var lstCompe = _competitionRepo.All().Where(x => x.TournamentId == id).ToList();
                var tourmanetRsp = _mapper.Map<TournamentInforRsp>(tournament);
                int totalTeams = CalculateTotalTeamsInTournament(id);
                tourmanetRsp.NumberTeam = totalTeams;

                res.setData("data", tourmanetRsp);
            }
            catch (Exception ex)
            {
                throw new Exception("Fail data");
            }
            return res;
        }
        public int CalculateTotalTeamsInTournament(int tournamentId)
        {
            try
            {
                var competitions = _competitionRepo.All().Where(c => c.TournamentId == tournamentId).ToList();
                if (competitions == null || !competitions.Any())
                {
                    throw new Exception("Không tìm thấy cuộc thi nào thuộc giải đấu.");
                }
                int totalTeam = 0;
                foreach (var competition in competitions)
                {
                    if (competition.NumberTeam.HasValue)
                    {
                        totalTeam += competition.NumberTeam.Value;
                    }
                }

                return totalTeam;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tính tổng số lượng đội tham gia: {ex.Message}");
            }
        }
        
    }
}
