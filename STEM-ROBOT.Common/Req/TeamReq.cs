using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TeamReq
    {
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

    }
}
