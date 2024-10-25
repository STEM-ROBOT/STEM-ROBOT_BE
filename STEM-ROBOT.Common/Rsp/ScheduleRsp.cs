using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public  class ScheduleRsp
    {
        public int RefereeId { get; set; }

        public int MatchId { get; set; }

        public int LocationId { get; set; }

        public DateTime StartTime { get; set; }
    }
}
