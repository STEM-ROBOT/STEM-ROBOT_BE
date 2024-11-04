using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class CompetitionConfigFormatReq
    {
        public int? FormatId { get; set; }

        public DateTime? RegisterTime { get; set; }

        public DateTime? StartTime { get; set; }

        public int? NumberContestantTeam { get; set; }

        public bool? IsTop { get; set; }

        public int? NumberTeam { get; set; }

        public int? NumberTeamNextRound { get; set; }

        public int? NumberTable { get; set; }

        public int? WinScore { get; set; }

        public int? LoseScore { get; set; }

        public int? TieScore { get; set; }


    }


}
