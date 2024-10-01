using STEM_ROBOT.Common.DAL;
using STEM_ROBOT.Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.BLL
{
    public class GenericSvc<T> : IGenericSvc<T> where T : class
    {
        protected readonly IGenericRep<T> _repository;

        public GenericSvc(IGenericRep<T> repository)
        {
            _repository = repository;
        }

       
        public SingleRsp Add(T entity)
        {
            var res = new SingleRsp();
            try
            {
                _repository.Add(entity);
                res.setData("200", entity);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Update(T entity)
        {
            var res = new SingleRsp();
            try
            {
                _repository.Update(entity);
                res.setData("200", entity);
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
                _repository.Delete(id);
                res.setData("200", $"Deleted entity with ID: {id}");
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
                var entity = _repository.getID(id);
                if (entity != null)
                {
                    res.setData("200", entity);
                }
                else
                {
                    res.SetError("404", $"Entity with ID: {id} not found");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        
        public MutipleRsp GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        {
            var res = new MutipleRsp();
            try
            {
                var data = _repository.All(filter, null, includeProperties, pageIndex, pageSize).ToList();
                res.SetSuccess(data, "Data retrieved successfully");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
