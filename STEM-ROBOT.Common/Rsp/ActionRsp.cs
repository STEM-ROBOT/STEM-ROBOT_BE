using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class ActionRsp
    {
        public int Id { get; set; }

        public int? MatchHalfId { get; set; }

        public int? ScoreCategoryId { get; set; }

        public int? Score { get; set; }

        public TimeSpan? EventTime { get; set; }

        public int? TeamMatchId { get; set; }

        public int? RefereeCompetitionId { get; set; }

        public string? Status { get; set; }
    }
}
