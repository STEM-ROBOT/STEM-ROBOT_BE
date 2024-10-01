using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class SingleRsp : Basersp
    { 
        public object Data { get; set; } 

        public SingleRsp():base() { }
        public SingleRsp(string message) : base(message)
        {

        }        
        public SingleRsp(string message, string titleError):base(message, titleError)
        {

        }
    

        public void setData(string code, object data)
        {
            Code = code;
            Data = data;
        }
    }
}
