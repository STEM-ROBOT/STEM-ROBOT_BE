using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public class PackageSvc
    {
        private readonly PackageRepo _packageRepo;
        private readonly IMapper _mapper;
        public PackageSvc(PackageRepo packageRepo, IMapper mapper)
        {
            _packageRepo = packageRepo;
            _mapper = mapper;
        }

        public MutipleRsp GetPackages()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _packageRepo.All();
                if (lst != null)
                {
                    var lstRes = _mapper.Map<List<PackageRsp>>(lst);
                    res.SetSuccess(lstRes, "data");
                }
                else
                {
                    res.SetError("No data found");
                }
            }
            catch (Exception ex)
            {   
                res.SetError(ex.StackTrace);
            }
            return res;
        }
        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var getPackage = _packageRepo.GetById(id);
                if (getPackage == null)
                {
                    res.SetError("404", "No data found");
                    return res;
                }
                else
                {
                    var packageRes = _mapper.Map<PackageRsp>(getPackage);
                    res.setData("data", packageRes);
                    return res;
                }

            }
            catch (Exception ex)
            {
                res.SetError(ex.StackTrace);
            }
            return res;
        }

        public SingleRsp CreatePackage([FromBody] PackageReq packageReq)
        {
            var res = new SingleRsp();
            try
            {
                var newPackage = _mapper.Map<Package>(packageReq);
                _packageRepo.Add(newPackage);
                res.setData("data", newPackage);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.StackTrace);
            }
            return res;
        }
        public SingleRsp UpdatePackage([FromBody] PackageReq packageReq, int id)
        {
            var res = new SingleRsp();
            try
            {
                var updPackage = _packageRepo.GetById(id);
                if(updPackage == null)
                {
                    res.SetError("404", "No data found");
                    return res;
                }
                _mapper.Map(packageReq, updPackage);
                _packageRepo.Update(updPackage);
                res.setData("data", updPackage);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.StackTrace);
            }
            return res;
        }
        public SingleRsp DeletePackage(int id)
        {
            var res = new SingleRsp();
            try
            {
                var package = _packageRepo.GetById(id);
                if (package == null)
                {
                    res.SetError("404", "No data found");
                    return res;
                }
                _packageRepo.Delete(id);
                res.SetMessage("Delete successfully");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.StackTrace);
            }
            return res;
        }
    }
}
