using AutoMapper;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class AreaSvc
    {
        private AreaRepo _areaRepo;
        private IMapper _mapper;
        private readonly ProvinceRepo _provinceRepo;
        private readonly DistrictRepo _districtRepo;
        private readonly SchoolRepo _schoolRepo;

        public AreaSvc(AreaRepo areaRepo, IMapper mapper, ProvinceRepo provinceRepo, DistrictRepo districtRepo, SchoolRepo schoolRepo)
        {
            _areaRepo = areaRepo;
            _mapper = mapper;
            _provinceRepo = provinceRepo;
            _districtRepo = districtRepo;
            _schoolRepo = schoolRepo;
        }

        public async Task<MutipleRsp> ListArea()
        {
            var res = new MutipleRsp();
            try
            {

                var list = await _areaRepo.GetListArea();
                if (list == null) throw new Exception("No data");
                var mapper = _mapper.Map<List<AreaRsp>>(list);
                res.SetData("data", mapper);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
        
        public MutipleRsp GetProvinceByArea(int areaId)
        {
            var res = new MutipleRsp();
            try
            {
                var area = _areaRepo.GetById(areaId);
                if (area == null) throw new Exception("Area not found");
                var list = _provinceRepo.All().Where(p => p.AreaId == areaId);
                var mapper = _mapper.Map<List<ProvinceRsp>>(list);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
        public MutipleRsp GetDistrictByProvince(int provinceId)
        {
            var res = new MutipleRsp();
            try
            {
                var province = _provinceRepo.GetById(provinceId);
                if (province == null) throw new Exception("Province not found");
                var list = _districtRepo.All().Where(d => d.ProvinceId == provinceId);
                var mapper = _mapper.Map<List<DistrictRsp>>(list);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }

        public MutipleRsp GetSchoolByDistrict(int districtId)
        {
            var res = new MutipleRsp();
            try
            {
                var district = _districtRepo.GetById(districtId);
                if (district == null) throw new Exception("District not found");
                var list = _schoolRepo.All().Where(s => s.DistrictId == districtId);
                var mapper = _mapper.Map<List<ListSchoolRsp>>(list);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
    }
    
}
