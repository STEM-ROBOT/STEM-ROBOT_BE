using STEM_ROBOT.Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TeamMatchReq
    {
        public int TeamId { get; set; }
        public bool IsHome { get; set; }

    }
    public class AssignTeamsToMatchesInStageTableReq
    {
        public int MatchId { get; set; }
        public List<TeamMatchReq> Teams { get; set; }
    }

    public class TeamMatchConfigCompetition
    {

        public int teamId { get; set; }
        public string teamName { get; set; }
        public int teamMatchId { get; set; }
        public int matchId { get; set; }        

    }
   
}
