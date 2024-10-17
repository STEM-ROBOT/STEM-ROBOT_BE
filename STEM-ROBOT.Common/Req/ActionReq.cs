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
        [Required(ErrorMessage = "Score is required")]
        public int Score { get; set; }

        [Required(ErrorMessage = "Event time is required")]
        public DateTime EventTime { get; set; }

        [Required(ErrorMessage = "MatchHalfId is required")]
        public int MatchHalfId { get; set; }

        [Required(ErrorMessage = "ScoreCategoryId is required")]
        public int ScoreCategoryId { get; set; }

        [Required(ErrorMessage = "TeamId is required")]
        public int TeamId { get; set; }
    }
}
