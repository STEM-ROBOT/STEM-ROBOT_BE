using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class ActionReq
    {

        [Required(ErrorMessage = "Event time is required")]
        public TimeSpan EventTime { get; set; }

        [Required(ErrorMessage = "MatchHalfId is required")]
        public int MatchHalfId { get; set; }

        [Required(ErrorMessage = "ScoreCategoryId is required")]
        public int ScoreCategoryId { get; set; }

        [Required(ErrorMessage = "TeamId is required")]
        public int? TeamMatchId { get; set; }

    }
}
