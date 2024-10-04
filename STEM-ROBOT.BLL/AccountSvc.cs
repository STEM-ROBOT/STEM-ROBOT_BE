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

namespace STEM_ROBOT.BLL
{
    public class AccountSvc : GenericSvc<Account>
    {
        private readonly AccountRepo _accountRepo;
        private readonly IConfiguration _configuration;

        public AccountSvc(AccountRepo accountRep, IConfiguration configuration) : base(accountRep)
        {
            _accountRepo = accountRep;
            _configuration = configuration;
        }

        public MutipleRsp GetAll()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _accountRepo.All();
                if (lst.Count() == 0)
                {
                    res.SetError("404", "No data found");
                }
                res.SetData("Success", lst);
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
                res.setData("Success", acc);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        /*public SingleRsp CreateAccount([FromBody] AccountReq acccountReq)
        {
            var res = new SingleRsp();
            try
            {
                var newAcc = new Account();
                newAcc.RoleId = acccountReq.RoleId;
                newAcc.Name = acccountReq.Name;
                newAcc.Email = acccountReq.Email;
                newAcc.Password = acccountReq.Password;
                newAcc.PhoneNumber = acccountReq.PhoneNumber;
                newAcc.Image = acccountReq.Image;
                newAcc.Status = "Active";
                var lstAcc = _accountRepo.All().ToList();
                if(lstAcc.Count() != 0)
                {
                    foreach (var item in lstAcc)
                    {
                        if (newAcc.Email.Equals(item.Email))
                        {
                            return res.SetError("400", "Email already exists");
                        }
                    }
                }
                else
                {
                    return res.setData("Success", res.)
                }


                return res;
            }
            catch(Exception ex)
            {
                res.SetError("500", ex.Message);
            }
        }*/
    }
}
