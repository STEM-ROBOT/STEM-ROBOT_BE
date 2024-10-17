using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using STEM_ROBOT.Common.BLL;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;

namespace STEM_ROBOT.BLL.Svc
{
    public class AccountSvc 
    {
        private readonly AccountRepo _accountRepo;
        private readonly IMapper _mapper;

        public AccountSvc(AccountRepo accountRep, IMapper mapper) 
        {
            _accountRepo = accountRep;
            _mapper = mapper;
        }

        public async Task<MutipleRsp> GetAll()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = await _accountRepo.GetAccounts();
                var accountResLst = _mapper.Map<List<AccountRes>>(lst);
                res.SetSuccess(accountResLst, "Success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public async Task<SingleRsp> GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var acc = await _accountRepo.GetAccountById(id);
                if (acc == null)
                {
                    res.SetError("404", "No data found");
                }
                var accountRes = _mapper.Map<AccountRes>(acc);
                res.setData("Success", accountRes);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create([FromBody] AccountReq req)
        {
            var res = new SingleRsp();
            try
            {
                var existingAccount = _accountRepo.Find(a => a.Email == req.Email).FirstOrDefault();
                if (existingAccount != null)
                {
                    res.SetError("400", "Email already exists");
                    return res;
                }

                var account = _mapper.Map<Account>(req);

                if (account.RoleId == 1)
                {
                    res.SetError("403", "You can't create an account with role Admin");
                    return res;
                }

                _accountRepo.Add(account);
                res.setData("Account added successfully", account);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Update([FromBody] AccountReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var account = _accountRepo.getID(id);
                if (account == null)
                {
                    res.SetError("404", "No data found");
                    return res;
                }
                var existingAccount = _accountRepo.Find(a => a.Email == req.Email && a.Id != id).FirstOrDefault();
                if (existingAccount != null)
                {
                    res.SetError("400", "Email already exists");
                    return res;
                }
                if (account.RoleId == 1 || req.RoleId == 1)
                {
                    res.SetError("403", "You can't update an account with role Admin");
                    return res;
                }
                _mapper.Map(req, account);
                _accountRepo.Update(account);
                res.setData("200", account);
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
                var acc = _accountRepo.getID(id);
                if (acc == null)
                {
                    res.SetError("404", "No data found");
                }
                if (acc.RoleId == 1)
                {
                    res.SetError("403", "You can't delete an account with role Admin");
                    return res;
                }
                _accountRepo.Delete(acc.Id);
                res.setData("200", acc);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
