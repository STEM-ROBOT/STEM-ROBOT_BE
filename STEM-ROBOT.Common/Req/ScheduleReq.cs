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
        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "RefereeId is required")]
        public int RefereeId { get; set; }

        [Required(ErrorMessage = "MatchId is required")]
        public int MatchId { get; set; }

        [Required(ErrorMessage = "LocationId is required")]
        public int LocationId { get; set; }
    }
}
