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

        public RefereeSvc(RefereeRepo refereeRepo, IMapper mapper)
        {
            _refereeRepo = refereeRepo;
            _mapper = mapper;
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
    }
}
