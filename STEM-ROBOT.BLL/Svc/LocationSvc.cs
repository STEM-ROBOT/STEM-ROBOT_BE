﻿using AutoMapper;
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
    public class LocationSvc
    {
        private readonly LocationRepo _locationRepo;
        private readonly IMapper _mapper;

        public LocationSvc(LocationRepo locationRepo, IMapper mapper)
        {
            _locationRepo = locationRepo;
            _mapper = mapper;
        }

        public async Task<MutipleRsp> GetLocations()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = await _locationRepo.GetLocations();
                if (lst != null)
                {
                    res.SetSuccess(lst, "200");
                }
                else
                {
                    res.SetError("404", "No data found");
                }

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
                var getLocation = await _locationRepo.GetLocationById(id);

                if (getLocation == null)
                {
                    res.SetError("404", "No data found");
                }

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp CreateLocation(LocationReq req)
        {
            var res = new SingleRsp();
            try
            {
                var location = _mapper.Map<Location>(req);
                _locationRepo.Add(location);
                res.setData("Account added successfully", location);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp UpdateLocation(LocationReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var location = _locationRepo.GetById(id);
                if(location == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _mapper.Map(res, location);
                    _locationRepo.Update(location);
                    res.setData("200", location);
                }
                res.setData("Account added successfully", location);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp  DeleteLocation(int id)
        {
            var res = new SingleRsp();
            try
            {
                var location = _locationRepo.GetById(id);
                if (location == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _locationRepo.Delete(id);
                    res.SetMessage("Delete successfully");
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
