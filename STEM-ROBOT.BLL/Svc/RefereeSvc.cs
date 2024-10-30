using AutoMapper;
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
        private readonly RefereeCompetitionRepo _refereeCompetitionRepo;
        private readonly ScheduleRepo _scheduleRepo;
        private readonly CompetitionRepo _competitionRepo;

        public RefereeSvc(RefereeRepo refereeRepo, IMapper mapper, RefereeCompetitionRepo refereeCompetitionRepo, ScheduleRepo scheduleRepo, CompetitionRepo competitionRepo)
        {
            _refereeRepo = refereeRepo;
            _mapper = mapper;
            _refereeCompetitionRepo = refereeCompetitionRepo;
            _scheduleRepo = scheduleRepo;
            _competitionRepo = competitionRepo;
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
                    var lstRes = _mapper.Map<List<RefereeRsp>>(lst);
                    res.SetSuccess(lstRes, "200");
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
                var referee = _refereeRepo.GetById(id);
                if (referee == null)
                {
                    res.SetError("404", "Referee not found");
                }
                else
                {
                    var refereeRes = _mapper.Map<RefereeRsp>(referee);
                    res.setData("200", refereeRes);
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
                res.setData("200", newReferee);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp AddListReferee(List<RefereeReq> referees)
        {
            var res = new MutipleRsp();
            try
            {
                var refereeList = new List<Referee>();

                foreach (var item in referees)
                {
                    var referee = new Referee
                    {
                        TournamentId = item.TournamentId,
                        Name = string.IsNullOrEmpty(item.Name) ? "No data" : item.Name,
                        Email = string.IsNullOrEmpty(item.Email) ? "No data" : item.Email,
                        Status = string.IsNullOrEmpty(item.Status) ? "No data" : item.Status,
                        PhoneNumber = string.IsNullOrEmpty(item.PhoneNumber) ? "No data" : item.PhoneNumber,
                        Image = string.IsNullOrEmpty(item.Image) ? "No data" : item.Image,
                    };

                    refereeList.Add(referee);
                    _refereeRepo.Add(referee);
                }


                res.SetData("200", refereeList);
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
                    res.setData("200", referee);
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
                    var lstRes = _mapper.Map<List<RefereeRsp>>(lst);
                    res.SetSuccess(lstRes, "200");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        /*public MutipleRsp GetListRefereeAvailable(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                // Lấy tournamentId từ competitionId
                var tournamentId = _competitionRepo.All()
                    .Where(c => c.Id == competitionId)
                    .Select(c => c.TournamentId)
                    .FirstOrDefault();

                if (tournamentId == null)
                {
                    throw new Exception("Không tìm thấy giải đấu cho cuộc thi này.");
                }

                // Lấy danh sách RefereeId từ RefereeCompetition dựa trên competitionId
                var refereeCompetitionList = _refereeCompetitionRepo.All()
                    .Where(rc => rc.CompetitionId == competitionId)
                    .ToList();

                if (refereeCompetitionList == null || !refereeCompetitionList.Any())
                {
                    throw new Exception("Không tìm thấy trọng tài nào trong cuộc thi này.");
                }

                var refereeIds = refereeCompetitionList.Select(rc => rc.RefereeId).Distinct().ToList();

                // Lọc danh sách trọng tài phải thuộc giải đấu của tournamentId
                var tournamentReferees = _refereeRepo.All()
                    .Where(r => r.TournamentId == tournamentId && refereeIds.Contains(r.Id))
                    .Select(r => r.Id)
                    .ToList();

                // Lấy danh sách các trọng tài đang bận từ bảng Schedule
                var busyRefereeIds = _scheduleRepo.All()
                    .Where(s => s.StartTime > DateTime.Now)  // Lấy lịch trong tương lai
                    .Select(s => s.RefereeId)
                    .Distinct()
                    .ToList();

                // Lấy danh sách trọng tài rảnh (những người nằm trong danh sách tournamentReferees và không bận)
                var availableReferees = _refereeRepo.All()
                    .Where(r => tournamentReferees.Contains(r.Id) && !busyRefereeIds.Contains(r.Id))
                    .ToList();

                if (availableReferees == null || !availableReferees.Any())
                {
                    res.SetSuccess("200", "Không có trọng tài rảnh.");
                    return res;
                }

                var availableReferees_mapper = _mapper.Map<List<RefereeRsp>>(availableReferees);
                res.SetData("200", availableReferees_mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }*/
    }



}
