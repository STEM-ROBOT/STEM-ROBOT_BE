using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using OfficeOpenXml;
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
    public class ContestantSvc
    {
        private readonly IMapper _mapper;
        private readonly ContestantRepo _contestantRepo;
        private readonly TournamentRepo _tournamentRepo;
        private readonly ContestantTeamRepo _contestantTeamRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TeamRepo _teamRepo;
        private readonly CompetitionRepo _competitionRepo;
        private readonly AccountRepo _accountRepo;
        public ContestantSvc(ContestantRepo contestantRepo, IMapper mapper, TournamentRepo tournamentRepo, IHttpContextAccessor httpContextAccessor, ContestantTeamRepo contestantTeamRepo, TeamRepo teamRepo, CompetitionRepo competitionRepo, AccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
            _competitionRepo = competitionRepo;
            _tournamentRepo = tournamentRepo;
            _contestantRepo = contestantRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _contestantTeamRepo = contestantTeamRepo;
            _teamRepo = teamRepo;
        }
        public async Task<MutipleRsp> AddContestant(IFormFile file)
        {

            MutipleRsp res = new MutipleRsp();
            try
            {
                var contestant = new List<Contestant>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                        int rowCount = workSheet.Dimension.Rows;
                        for (int row = 3; row < rowCount; row++)
                        {

                            var contestants = new Contestant
                            {

                                //    SchoolId = int.TryParse(workSheet.Cells[row, 1].Value?.ToString().Trim(), out var schoolId)? schoolId: 0,  
                                TournamentId = int.TryParse(workSheet.Cells[row, 2].Value?.ToString().Trim(), out var tournamentId) ? tournamentId : 0,
                                Name = string.IsNullOrEmpty(workSheet.Cells[row, 3].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 3].Value?.ToString().Trim(),
                                Email = string.IsNullOrEmpty(workSheet.Cells[row, 4].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 4].Value?.ToString().Trim(),
                                Status = string.IsNullOrEmpty(workSheet.Cells[row, 5].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 5].Value?.ToString().Trim(),
                                Gender = string.IsNullOrEmpty(workSheet.Cells[row, 6].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 6].Value?.ToString().Trim(),
                                Phone = string.IsNullOrEmpty(workSheet.Cells[row, 7].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 7].Value?.ToString().Trim(),
                                Image = string.IsNullOrEmpty(workSheet.Cells[row, 8].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 8].Value?.ToString().Trim(),

                            };
                            contestant.Add(contestants);
                        }

                    }
                }
                await _contestantRepo.BulkInsertAsyncSchool(contestant);


            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        public MutipleRsp AddListContestantInTournament(List<ContestantReq> contestants, int accountId, int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var tournament = _tournamentRepo.GetById(tournamentId);
                if (tournament == null)
                {
                    res.SetError("404", "Tournament not found");
                    return res;
                }
                var contestantLst = _mapper.Map<List<Contestant>>(contestants);
                foreach (var contestant in contestantLst)
                {
                    contestant.AccountId = accountId;
                    contestant.TournamentId = tournamentId;
                }
                _contestantRepo.AddRange(contestantLst);
                res.SetMessage("Add successfully");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        public SingleRsp GetListContestants()
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.All();
                if (contestant != null)
                {
                    var mapper = _mapper.Map<IEnumerable<Contestant>>(contestant);
                    res.setData("data", mapper);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetListAvailableContestantByTournament(int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var contestants = _contestantRepo.All(
                    filter: x => x.TournamentId == tournamentId,
                    includeProperties: "Tournament"
                ).ToList();

                if (contestants == null || !contestants.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var lstContestant = new List<ContestantInTournament>();
                foreach (var contestant in contestants)
                {
                    if (contestant.EndTime < ConvertToVietnamTime(DateTime.Now) || contestant.EndTime == null)
                    {
                        lstContestant.Add(new ContestantInTournament
                        {
                            Id = contestant.Id,
                            Name = contestant.Name,
                            Email = contestant.Email,
                            AccountId = contestant.AccountId,
                            TournamentId = contestant.TournamentId,
                            Gender = contestant.Gender,
                            Phone = contestant.Phone,
                            Image = contestant.Image,
                        });
                    }
                }

                res.SetData("data", lstContestant);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp GetListAvailableContestantByAccount(int accountId, int tounamentId, int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var contestants = _contestantRepo.All(
                    filter: x => x.AccountId == accountId && x.TournamentId == tounamentId,
                    includeProperties: "Account,ContestantTeams.TeamRegister.Competition"
                ).ToList();
                var competition = _competitionRepo.GetById(competitionId);
                if (contestants == null || !contestants.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var lstContestant = new List<ContestantInTournament>();

                foreach (var contestant in contestants)
                {
                    if (contestant.ContestantTeams.Count < 1)
                    {
                        lstContestant.Add(new ContestantInTournament
                        {
                            Id = contestant.Id,
                            Name = contestant.Name,
                            Email = contestant.Email,
                            AccountId = contestant.AccountId,
                            TournamentId = contestant.TournamentId,
                            Gender = contestant.Gender,
                            Phone = contestant.Phone,
                            Image = contestant.Image,
                            StartTime = contestant.StartTime,
                            SchoolName = contestant.SchoolName,
                            GenreName = "Đang không tham gia"
                        });
                    }
                    else
                    {
                        var contestantTeams = contestant.ContestantTeams?
                                                   .ToList()
                                                   .Where(c => c.TeamRegister?.Competition != null && c.TeamRegister.Competition.EndTime < competition.StartTime && c.TeamRegister.Competition.StartTime > competition.EndTime)
                                                   .FirstOrDefault();

                        if (contestantTeams != null)
                        {
                            lstContestant.Add(new ContestantInTournament
                            {
                                Id = contestant.Id,
                                Name = contestant.Name,
                                Email = contestant.Email,
                                AccountId = contestant.AccountId,
                                TournamentId = contestant.TournamentId,
                                Gender = contestant.Gender,
                                Phone = contestant.Phone,
                                Image = contestant.Image,
                                StartTime = contestant.StartTime,
                                SchoolName = contestant.SchoolName,
                                GenreName = "Đang không tham gia"
                            });
                        }
                    }


                }
                res.SetData("data", lstContestant);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp GetListContestantByTournament(int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var tournament = _tournamentRepo.GetById(tournamentId);
                if (tournament == null)
                {
                    res.SetError("404", "Tournament not found");
                    return res;
                }
                var contestants = _contestantRepo.All(
                    filter: x => x.TournamentId == tournamentId,
                    includeProperties: "Tournament"
                ).ToList();

                if (contestants == null || !contestants.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var lstContestant = new List<ContestantInTournament>();
                foreach (var contestant in contestants)
                {

                    lstContestant.Add(new ContestantInTournament
                    {
                        Id = contestant.Id,
                        Name = contestant.Name,
                        Email = contestant.Email,
                        AccountId = contestant.AccountId,
                        TournamentId = contestant.TournamentId,
                        Gender = contestant.Gender,
                        Phone = contestant.Phone,
                        Image = contestant.Image,
                        SchoolName=contestant.SchoolName,
                    });

                }

                res.SetData("data", lstContestant);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp GetListContestantByAccount(int accountId)
        {
            var res = new MutipleRsp();
            try
            {
                var account = _accountRepo.GetById(accountId);
                if (account == null)
                {
                    res.SetError("404", "Account not found");
                    return res;
                }
                var contestants = _contestantRepo.All(
                    filter: x => x.AccountId == accountId,
                    includeProperties: "Account,ContestantTeams.Team.Competition.Genre"
                ).ToList();

                if (contestants == null || !contestants.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var lstContestant = new List<ContestantInTournament>();
                var nowTime = ConvertToVietnamTime(DateTime.Now);

                foreach (var contestant in contestants)
                {
                    var competition = contestant.ContestantTeams?.ToList().Where(c => c.Team.Competition.StartTime < nowTime && c.Team.Competition.EndTime > nowTime).FirstOrDefault().Team?.Competition;
                    var genreName = competition?.Genre?.Name;
                    lstContestant.Add(new ContestantInTournament
                    {
                        Id = contestant.Id,
                        Name = contestant.Name,
                        Email = contestant.Email,
                        AccountId = contestant.AccountId,
                        TournamentId = contestant.TournamentId,
                        Gender = contestant.Gender,
                        Phone = contestant.Phone,
                        Image = contestant.Image,
                        GenreName = competition == null ? "Đang không tham gia" : genreName,
                    });

                }

                res.SetData("data", lstContestant);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetContestantID(int ID)
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.GetById(ID);
                if (contestant != null)
                {
                    var mapper = _mapper.Map<Contestant>(contestant);
                    res.setData("data", mapper);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateContestant(int contestantID, ContestantReq contestantReq)
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.GetById(contestantID);
                if (contestant != null)
                {
                    _mapper.Map(contestantReq, contestant);
                    _contestantRepo.Update(contestant);
                    res.setData("data", contestant);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteContestant(int contestantID)
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.GetById(contestantID);
                if (contestant != null)
                {
                    _contestantRepo.Delete(contestant.Id);
                    res.setData("data", contestant);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp AddContestantTeam(int teamId, List<ContestantTeamReq> request)
        {
            var res = new MutipleRsp();
            try
            {
                var team = _teamRepo.GetById(teamId);
                var competition = _competitionRepo.Find(x => x.Id == team.CompetitionId).FirstOrDefault();
                if (competition == null)
                {
                    res.SetError("404", "Competition not found");
                }
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    foreach (var item in request)
                    {
                        var contestant = _contestantRepo.GetById(item.ContestantId);
                        contestant.EndTime = competition.EndTime;
                        _contestantRepo.Update(contestant);
                        var contestantTeam = _mapper.Map<ContestantTeam>(item);
                        contestantTeam.TeamId = teamId;
                        _contestantTeamRepo.Add(contestantTeam);
                    }
                    res.SetMessage("Add successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp GetContestantInTeam(int teamId)
        {
            var res = new MutipleRsp();
            try
            {
                var team = _teamRepo.GetById(teamId);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                    return res;
                }
                var lstContestantTeam = _contestantTeamRepo.All().Where(ct => ct.TeamId == team.Id).ToList();
                if (lstContestantTeam == null || !lstContestantTeam.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var lstContestant = new List<Contestant>();
                foreach (var contestantteam in lstContestantTeam)
                {
                    var contestant = _contestantRepo.GetById(contestantteam.ContestantId);
                    lstContestant.Add(contestant);
                }
                var lstContestantRsp = _mapper.Map<List<ContestantRep>>(lstContestant);
                res.SetData("data", lstContestantRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetAvailableContestantByCompetition(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                if (competition == null)
                {
                    res.SetError("No competition found");
                }
                var contestants = _contestantRepo.All(
                    filter: x => x.TournamentId == competition.TournamentId,
                    includeProperties: "ContestantTeams"
                ).ToList();
                if (contestants.Count == 0)
                {
                    res.SetError("No contestant found");
                }
                var lstContestant = new List<Contestant>();
                foreach (var contestant in contestants)
                {
                    if (contestant.ContestantTeams == null || contestant.ContestantTeams.Count == 0)
                    {
                        lstContestant.Add(contestant);
                    }
                }
                var lstContestantRsp = _mapper.Map<List<ContestantRep>>(lstContestant);
                res.SetData("data", lstContestantRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp AddContestantPublic(int tournamentId, int accountId, ContestantReq request)
        {
            var res = new SingleRsp();
            try
            {
                var tournament = _tournamentRepo.GetById(tournamentId);
                if (tournament == null)
                {
                    res.SetError("404", "Tournament not found");
                    return res;
                }
                if (tournament.Status == "Private")
                {
                    res.SetError("404", "You can not access this tournament");
                    return res;
                }
                var account = _accountRepo.GetById(accountId);
                if (account == null)
                {
                    res.SetError("404", "Account not found");
                    return res;
                }
                var contestant = _mapper.Map<Contestant>(request);
                contestant.AccountId = accountId;
                contestant.TournamentId = tournamentId;        
                contestant.StartTime = ConvertToVietnamTime(DateTime.Now);
                _contestantRepo.Add(contestant);
                res.setData("data", "success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> GetContestantRegister(int tournamentId, int userId)
        {
            var res = new SingleRsp();
            try
            {
                var list_contestants = _contestantRepo.All(filter: c => c.TournamentId == tournamentId && c.AccountId == userId);
                var list_res = _mapper.Map<List<ContestantReq>>(list_contestants);

                res.setData("data", list_res);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
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
