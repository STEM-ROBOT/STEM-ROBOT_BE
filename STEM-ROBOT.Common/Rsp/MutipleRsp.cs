using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class MutipleRsp : Basersp
    {
       
        public List<object> DataList { get; set; }


        public MutipleRsp() : base()
        {

        }

        public MutipleRsp(string message) : base(message)
        {

        }

        public MutipleRsp(string message, string titleError) : base(message, titleError)
        {

        }


        public void SetData(string key, object o)
        {
            if (Data == null)
            {
                Data = new Dictionary<string, object>();
            }
            Data.Add(key, o);
        }


        public void SetFailure(object o, string message)
        {
            var t = new Dto(o, message);
            SetData("failure", t);
        }

        public void SetSuccess(object o, string message)
        {
            var t = new Dto(o, message);
            SetData("success", t);
        }

        public Dictionary<string, object> Data { get; private set; }

        public class Dto
        {
            public Dto(object data, string message)
            {
                Data = data;
                Message = message;
            }

            public object Data { get; private set; }

            public string Message { get; private set; } 
        }
       
       

        
    }
}
