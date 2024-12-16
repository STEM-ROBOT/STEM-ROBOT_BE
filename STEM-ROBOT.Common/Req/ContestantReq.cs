using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class ContestantReq
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Image { get; set; }

        public string? SchoolName { get; set; }
        public DateTime? StartTime { get; set; }
    }
}