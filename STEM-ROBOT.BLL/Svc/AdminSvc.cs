using AutoMapper;
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
    public class AdminSvc
    {
        private readonly TournamentRepo _tournamentRepo;
        private readonly AccountRepo _accountRepo;
        private readonly OrderRepo _orderRepo;
        private readonly PaymentRepo _paymentRepo;
        private readonly GenreRepo _genreRepo;
        private IMapper _mapper;

        public AdminSvc(TournamentRepo tournamentRepo, AccountRepo accountRepo, OrderRepo orderRepo, PaymentRepo paymentRepo,IMapper mapper, GenreRepo genreRepo)
        {
            _tournamentRepo = tournamentRepo;
            _accountRepo = accountRepo;
            _orderRepo = orderRepo;
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _genreRepo = genreRepo;
        }
        public async Task<SingleRsp> CountTournament()
        {
            var res = new SingleRsp();
            try
            {
                var count = await _tournamentRepo.CountTournament();
                if (count == 0) return null;
                res.setData("data", count);
            }
            catch (Exception ex)
            {
                throw new Exception("Please check tournament !");
            }
            return res;
        } 
        public async Task<MutipleRsp> GetListAccountAdmin()
        {
            var res = new MutipleRsp();
            try
            {
                var listAccount = await _accountRepo.GetAccount();
                if (listAccount == null) throw new Exception("No Data");
                var mapper = _mapper.Map<List<AccountRsp>>(listAccount);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception("Please check data");
            }
            return res;
        }

        //Genre 
        public async Task<MutipleRsp> GetListGenre()
        {
            var res = new MutipleRsp();
            try
            {
                var listGenre = _genreRepo.All();
                if (listGenre == null) throw new Exception("Please check data");
                var mapper = _mapper.Map<List<GenreRsp>>(listGenre);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception("Please check data");
            }
            return res;
        }
        public  SingleRsp AddGenre(GenreReq genre)
        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Genre>(genre);
                _genreRepo.Add(mapper);
                res.SetMessage("Success");

            }catch(Exception ex)
            {
                throw new Exception("Add fail");

            }
            return res;
        }

        public async Task<MutipleRsp> GetListOrder(string? NameUser)
        {
            var res = new MutipleRsp();
            try
            {
                var listOrder = await _orderRepo.GetListOrder(NameUser);
                if (listOrder == null) throw new Exception("No data");
                var mapper = _mapper.Map<List<OrderRsp>>(listOrder);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception("Get List Order Fail");
            }
            return res;
        }
    }
}
