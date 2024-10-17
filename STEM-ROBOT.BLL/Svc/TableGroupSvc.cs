using AutoMapper;
using Azure.Core;
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
    public class TableGroupSvc
    {
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly IMapper _mapper;
        public TableGroupSvc(TableGroupRepo tableGroupRepo,IMapper mapper)
        {
            _tableGroupRepo = tableGroupRepo;
            _mapper = mapper;
        }
        public MutipleRsp GetListTable()
        {
            var res = new MutipleRsp();
            try
            {
                var table = _tableGroupRepo.All();
                if (table == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<TableGroup>>(table);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp GetIdTable(int id)
        {
            var res = new SingleRsp();
            try
            {
                var table = _tableGroupRepo.GetById(id);
                if (table == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<TableGroup>(table);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp AddTable(TableGroupReq request)
        {

            var res = new SingleRsp();
            try
            {
                
              
                var mapper = _mapper.Map<TableGroup>(request);
                if (mapper == null)
                {
                    res.SetError("No data");
                }
                _tableGroupRepo.Add(mapper);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateTable(int id,TableGroupReq request)
        {

            var res = new SingleRsp();
            try
            {
                var table = _tableGroupRepo.GetById(id);

                if (table == null)
                {
                    res.SetError("No data");
                }
                _mapper.Map(request,table);
                _tableGroupRepo.Update(table);
                res.setData("OK", table);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteTable(int id)
        {
            var res = new SingleRsp();
            try
            {
                var table = _tableGroupRepo.GetById(id);

                if (table == null)
                {
                    res.SetError("No data");
                }
                _tableGroupRepo.Delete(table.Id);
                res.setData("OK", table);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
    }
}
