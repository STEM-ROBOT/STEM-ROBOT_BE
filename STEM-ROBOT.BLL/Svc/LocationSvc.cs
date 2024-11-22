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
    public class LocationSvc
    {
        private readonly LocationRepo _locationRepo;
        private readonly IMapper _mapper;
        private readonly CompetitionRepo _competitionRepo;
        public LocationSvc(LocationRepo locationRepo, IMapper mapper, CompetitionRepo competitionRepo)
        {
            _locationRepo = locationRepo;
            _mapper = mapper;
            _competitionRepo = competitionRepo;
        }

        public MutipleRsp GetLocations()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _locationRepo.All();
                if (lst != null)
                {
                    var locationMapp = _mapper.Map<List<LocationRsp>>(lst);

                    res.SetSuccess(locationMapp, "data");
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
        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var getLocation = _locationRepo.GetById(id);

                if (getLocation == null)
                {
                    res.SetError("404", "No data found");
                }
                var locationMapp = _mapper.Map<LocationRsp>(getLocation);
                res.setData("data", locationMapp);
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
                if (location == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _mapper.Map(res, location);
                    _locationRepo.Update(location);
                    res.setData("data", location);
                }
                res.setData("Account added successfully", location);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteLocation(int id)
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
        public MutipleRsp GetLocationsByCompetition(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var competition = _competitionRepo.GetById(competitionId);
                if (competition == null)
                {
                    res.SetError("404", "No competition found");
                }
                var lstLocation = _locationRepo.All().Where(l => l.CompetitionId == competitionId).ToList();
                if (lstLocation != null)
                {
                    var locationMapp = _mapper.Map<List<LocationRsp>>(lstLocation);
                    res.SetData("data", locationMapp);
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
        public MutipleRsp GetAvailableLocations(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var lstLocation = _locationRepo.All().Where(l => l.CompetitionId == competitionId && l.Status == "Trống").ToList();
                if (lstLocation != null)
                {
                    var locationMapp = _mapper.Map<List<LocationRsp>>(lstLocation);
                    res.SetData("data", locationMapp);
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

        public MutipleRsp AddListLocation(List<LocationReq> locations, int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
              
                var competition = _competitionRepo.GetById(competitionId);
                competition.IsLocation = true;
                _competitionRepo.Update(competition);
                var locationList = new List<Location>();
                foreach (var location in locations)
                {
                    var locationMapp = _mapper.Map<Location>(location);
                    locationMapp.CompetitionId = competitionId;
                    locationMapp.Competition=competition;
                    locationList.Add(locationMapp);
                }
                _locationRepo.AddRange(locationList);
                
                res.SetData("data","succes");
            }
            catch (Exception ex)
            {
                res.SetError("Lam dep trai", ex.Message);
            }
            return res;
        }
    }
}
