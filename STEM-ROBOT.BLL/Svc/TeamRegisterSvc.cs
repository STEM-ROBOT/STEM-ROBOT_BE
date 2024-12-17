using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Index.HPRtree;
using Org.BouncyCastle.Ocsp;
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

    public class TeamRegisterSvc
    {
        private readonly TeamRegisterRepo _teamRegisterRepo;
        private readonly CompetitionRepo _competitionRepo;
        private readonly ContestantTeamRepo _contestantTeamRepo;
        private readonly TournamentRepo _tournamentRepo;
        private readonly TeamRepo _teamRepo;
        private readonly AccountRepo _accountRepo;
        private readonly IMapper _mapper;
        private readonly NotificationRepo _notificationRepo;
        public TeamRegisterSvc(TeamRegisterRepo repo, AccountRepo accountRepo, CompetitionRepo competitionRepo, ContestantTeamRepo contestantTeamRepo, IMapper mapper, TeamRepo teamRepo, TournamentRepo tournamentRepo, NotificationRepo notificationRepo)
        {
            _teamRegisterRepo = repo;
            _competitionRepo = competitionRepo;
            _contestantTeamRepo = contestantTeamRepo;
            _mapper = mapper;
            _accountRepo = accountRepo;
            _teamRepo = teamRepo;
            _tournamentRepo = tournamentRepo;
            _notificationRepo = notificationRepo;
        }


        public async Task<SingleRsp> RegisterTeamCompetion(TeamRegisterReq teamRegister, int competitionId, int userId)
        {
                var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                var tournament = _tournamentRepo.GetById(competition.TournamentId);

                var account = _accountRepo.GetById(userId);
                if (competition == null)
                {
                    res.Setmessage("Nội dung thi đấu không tồn tại !");
                }
                else
                {

                    teamRegister.CompetitionId = competition.Id;
                    teamRegister.AccountId = userId;
                    teamRegister.Status = "Đang xử lý";
                    teamRegister.RegisterTime = ConvertToVietnamTime(DateTime.Now);
                    var newTeamRegister = _mapper.Map<TeamRegister>(teamRegister);
                    _teamRegisterRepo.Add(newTeamRegister);
                    var listContestTants = _mapper.Map<List<ContestantTeam>>(teamRegister.Contestants);
                    foreach (var contestant in listContestTants)
                    {
                        contestant.TeamRegisterId = newTeamRegister.Id;
                    }
                    _contestantTeamRepo.AddRange(listContestTants);
                    var nottification = new Notification
                    {
                        Description = $"Đội {teamRegister.Name} đã đăng kí tham gia vào nội dung thi đấu của giải {tournament.Name}",
                        AccountId = tournament.AccountId,
                        RouterUi = $"/my-tournament/{tournament.Id}/mycompetition/{competitionId}/team-register",
                        CreateDate = ConvertToVietnamTime(DateTime.Now),
                        Status = false,
                    };
                    _notificationRepo.Add(nottification);
                    res.SetMessage("success");




                }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }

        public async Task<SingleRsp> getListTeamRegister(int competitionId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
            if (competition == null)
            {
                res.SetMessage("Nội dung thi đấu không tồn tại!");
            }
            else
            {
                
                    var list = await _teamRegisterRepo.getTeamRegister(competitionId);

                    var newTeamRegister = _mapper.Map<List<TeamRegisterRsp>>(list);

                    res.setData("data", newTeamRegister);
                
            }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }
        public async Task<SingleRsp> getListTeamRegisterTournament(int tournamentId)
        {
            var res = new SingleRsp();
            try
            {
                var competition = _tournamentRepo.GetById(tournamentId);
            if (competition == null)
            {
                res.SetMessage("Giải đấu không tồn tại!");
            }
            else
            {
                
                    var list = await _teamRegisterRepo.getTeamRegisterTournament(tournamentId);
                    var newTeamRegister = list.Select(tr => new TeamRegisterTournamentRsp
                    {
                        competition = tr.Competition.Genre.Name,
                        Name = tr.Name,
                        contactPerson = tr.ContactInfo,
                        contactPhone = tr.PhoneNumber,
                        Id = tr.Id,
                        Members = tr.ContestantTeams.Count,
                        RegisterTime = tr.RegisterTime,
                        Status = tr.Status
                    });

                    res.setData("data", newTeamRegister);
               
            }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
            }
            return res;
        }

        public async Task<SingleRsp> updateStatusTeamRegister(int id, TeamRegisterStatusRsp teamRegisterStatusRsp)
        {
            var res = new SingleRsp();
            try
            {
                var teamRegister = _teamRegisterRepo.GetById(id);
                var competition = _competitionRepo.GetById(teamRegister.CompetitionId);
                var tournament = _tournamentRepo.GetById(competition.TournamentId);
                if (teamRegister == null)
                {
                    res.SetMessage("Nội dung thi đấu không tồn tại!");
                }
                else
                {
                    var nottification = new Notification
                    {
                        
                        AccountId = teamRegister.AccountId,
                        RouterUi = $"/tournament-adhesion/{tournament.Id}/competitions-adhesion/{teamRegister.CompetitionId}/team-competition-adhesion",
                        CreateDate = ConvertToVietnamTime(DateTime.Now),
                        Status = false,
                    };
                    

                    var slot = _teamRepo.All(x => x.CompetitionId == teamRegister.CompetitionId && x.IsSetup != true).FirstOrDefault();
                    if (slot == null)
                    {
                        res.SetMessage("Hết teams để đăng kí");
                    }
                    if (teamRegisterStatusRsp.status == "Chấp nhận")
                    {
                        nottification.Description = $"Đội {teamRegister.Name} đã được chấp nhận đăng ký";
                        teamRegister.Status = teamRegisterStatusRsp.status;
                        teamRegister.TeamId = slot.Id;
                        var contestantTeam = _contestantTeamRepo.All(x => x.TeamRegisterId == teamRegister.Id).ToList();
                        foreach (var item in contestantTeam)
                        {
                            item.TeamId = slot.Id;

                        }
                        _contestantTeamRepo.UpdateRange(contestantTeam);
                        slot.IsSetup = true;
                        slot.ContactInfo = teamRegister.ContactInfo;
                        slot.Image = teamRegister.Image;
                        slot.Name= teamRegister.Name;
                        slot.PhoneNumber = teamRegister.PhoneNumber;                      
                        _teamRegisterRepo.Update(teamRegister);
                        _teamRepo.Update(slot);

                        res.SetMessage("Cập nhật thành công");

                    }
                    else
                    {
                        teamRegister.Status = teamRegisterStatusRsp.status;
                        _teamRegisterRepo.Update(teamRegister);
                        nottification.Description = $"Đội {teamRegister.Name} đã bị từ chối đăng ký";
                        res.SetMessage("Cập nhật thành công");
                    }
                    _notificationRepo.Add(nottification);

                }
            }
            catch (Exception ex)
            {
                res.SetError(ex.ToString());
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
    }
}
