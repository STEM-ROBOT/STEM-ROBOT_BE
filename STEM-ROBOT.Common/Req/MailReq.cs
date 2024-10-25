using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class MailReq
    {
        public string ToEmail { get; set; } = default!;

        public string Subject { get; set; } = default!;

        public string Body { get; set; } = default!;
    }
}
