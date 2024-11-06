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
        public AreaSvc(AreaRepo areaRepo, IMapper mapper)
        {
            _areaRepo = areaRepo;
            _mapper = mapper;
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
        public async Task<MutipleRsp> ListProvince(int id)
        {
            var res = new MutipleRsp();
            try
            {

                var list = await _areaRepo.GetProvince(id);
                if (list == null) throw new Exception("No data");
                var mapper = _mapper.Map<List<ProvinceList>>(list);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> ListDistrict(int id)
        {
            var res = new MutipleRsp();
            try
            {

                var list = await _areaRepo.GetDistrict(id);
                if (list == null) throw new Exception("No data");
                var mapper = _mapper.Map<List<DistrictList>>(list);
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
