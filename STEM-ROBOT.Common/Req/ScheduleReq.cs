using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class ScheduleReq
    {
        public int? RefereeCompetitionId { get; set; }

        public int? MatchId { get; set; }
    }
}
