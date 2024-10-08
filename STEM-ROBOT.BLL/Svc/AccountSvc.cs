using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using STEM_ROBOT.Common.BLL;
using STEM_ROBOT.Common.DAL;
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
    public class AccountSvc : GenericSvc<Account>
    {
        private readonly AccountRepo _accountRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountSvc(AccountRepo accountRep, IConfiguration configuration, IMapper mapper) : base(accountRep)
        {
            _accountRepo = accountRep;
            _configuration = configuration;
            _mapper = mapper;
        }

        public MutipleRsp GetAll()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _accountRepo.All();
                var accountResLst = _mapper.Map<IEnumerable<AccountRes>>(lst);
                res.SetSuccess(accountResLst, "Success");
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
                var acc = _accountRepo.getID(id);
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
                var account = _mapper.Map<Account>(req);
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
                }
                else
                {
                    account = _mapper.Map<Account>(req);
                    _accountRepo.Update(account);
                    res.setData("200", account);
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
                var acc = _accountRepo.getID(id);
                if (acc == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _accountRepo.Delete(acc.Id);
                    res.setData("200", acc);
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