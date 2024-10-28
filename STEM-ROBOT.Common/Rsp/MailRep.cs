using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class MailRep
    {
        public int ResponseCode { get; set; }
        public string Result { get; set; }
        public string Errormessage { get; set; }
    }
}
