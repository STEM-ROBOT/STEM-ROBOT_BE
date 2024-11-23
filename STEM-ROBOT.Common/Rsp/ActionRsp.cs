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

    public class HaftAction
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<ActionsRefereeSupRsp> Actions { get; set; } = new List<ActionsRefereeSupRsp>();
    }
    public class ActionsRefereeSupRsp
    {
        public int Id { get; set; }

        public int? ScoreCategoryId { get; set; }
        public string? ScoreCategoryDescription { get; set; }

        public int? ScoreCategoryPoint { get; set; }

        public string? ScoreCategoryType { get; set; }

        public string? EventTime { get; set; }

        public int? TeamMatchId { get; set; }

        public string? TeamName { get; set; }
        public int? RefereeCompetitionId { get; set; }

        public string? Status { get; set; }
    }
}
