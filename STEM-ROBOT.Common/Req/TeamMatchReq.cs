using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public  class TeamMatchReq
    {
        public int TeamId { get; set; }
        public bool IsHome { get; set; }

    }
    public class AssignTeamsToMatchesInStageTableReq
    {
        public int MatchId { get; set; }
        public List<TeamMatchReq> Teams { get; set; }
    }
}
