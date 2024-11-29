using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Generators;
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
        private readonly OrderRepo _orderRepo;
        private readonly PackageRepo _packageRepo;

        public AccountSvc(AccountRepo accountRep, IMapper mapper, OrderRepo orderRepo, PackageRepo packageRepo)
        {
            _accountRepo = accountRep;
            _mapper = mapper;
            _orderRepo = orderRepo;
            _packageRepo = packageRepo;
        }

        public SingleRsp GetAccounts()
        {
            var res = new SingleRsp();
            try
            {
                var lst =  _accountRepo.All();
                if (lst != null)
                {
                    var accountResLst = _mapper.Map<List<AccountRsp>>(lst);
                    res.setData("Success",accountResLst);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public  SingleRsp GetInfoUser(int id)
        {
            var res = new SingleRsp();
            try
            { 
                var acc = _accountRepo.GetById(id);
                if(acc.Role == "AD")
                {
                    res.SetMessage("You can't get an account with role Admin");
                    return res;
                }
                if (acc == null)
                {
                    res.SetError("404", "No data found");
                    return res;
                }
                var accountRes = _mapper.Map<AccountRsp>(acc);
                res.setData("data", accountRes);
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


                if (account.Role == "Admin")
                {
                    res.SetError("403", "You can't create an account with role Admin");
                    return res;
                }
                account.MaxTournatment = 3;
                account.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
                _accountRepo.Add(account);
                res.setData("data","Success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        //hash password 
        public SingleRsp Update([FromBody] AccountUpdateReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var account = _accountRepo.GetById(id);
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

                if (account.Role == "Admin" )
                {
                    res.SetError("403", "You can't update an account with role Admin");
                    return res;
                }
                var pass = account.Password;
               
                _mapper.Map(req, account);

                _accountRepo.Update(account);
                res.setData("data", account);
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
                var acc = _accountRepo.GetById(id);
                if (acc == null)
                {
                    res.SetError("404", "No data found");
                }
      if (acc.Role == "Admin")
                {
                    res.SetError("403", "You can't delete an account with role Admin");
                    return res;
                }

                _accountRepo.Delete(acc.Id);
                res.setData("data", acc);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetPackageUsed(int accountId)
        {
            var res = new SingleRsp();
            try
            {
                var recentOrder = _orderRepo.All(o => o.AccountId == accountId && o.Status == "Success").OrderByDescending(o => o.OrderDate).FirstOrDefault();
                if (recentOrder != null)
                {
                    var package = _packageRepo.GetById(recentOrder.PackageId);

                    if (package != null)
                    {
                        var packageRsp = _mapper.Map<PackageRsp>(package);
                        res.setData("data", packageRsp);
                        return res;
                    }
                    else
                    {
                        res.SetError("Package not found");
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp ChangePassword(int userID, ChangePass pass)
        {
            var res = new SingleRsp();
            try
            {
                var user = _accountRepo.GetById(userID);
                if (user == null) throw new Exception("No data");
                bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(pass.PasswordOld,user.Password);
                if (!isOldPasswordCorrect)
                {
                    throw new Exception("Please check again pass");
                }
                if (pass.NewPassword != pass.ConfirmPass) throw new Exception("Please check confirm");
                var passwords = BCrypt.Net.BCrypt.HashPassword(pass.NewPassword);
                user.Password = passwords;
                _accountRepo.Update(user);
                res.SetMessage("OK");
            }
            catch(Exception ex)
            {
                throw new Exception("Change password fail");
            }
            return res;
        }
    }
}
