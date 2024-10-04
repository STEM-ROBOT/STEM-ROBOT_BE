using STEM_ROBOT.Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.BLL
{
    public interface IGenericSvc<T>
    {
       
        SingleRsp Add(T entity);

        SingleRsp Update(T entity);

      
        SingleRsp Delete(int id);

      
        SingleRsp GetById(int id);

        MutipleRsp GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null);
    }
}
