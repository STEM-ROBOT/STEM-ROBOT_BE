using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public abstract class BaseReq<T>
    {
        public string Keyword { get; set; }
        public int Id { get; set; }

        public abstract T ToModel(int? createBy = null);

        public BaseReq(string keyword)
        {
            Keyword = string.Empty;
        }
        public BaseReq(int id)
        {
            Id = id;
        }

        protected BaseReq()
        {
        }
    }
}
