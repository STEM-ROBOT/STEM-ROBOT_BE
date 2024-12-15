using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
   public  class MatchPoint
    {
    public int haftMatch { get; set; }
    public string haftName { get; set; }

    public TeamAcctivity activity { get; set; } 


    }

    public class TeamAcctivity
    {

        public ICollection<TeamActivity1> activityTeam1 { get; set; } = new List<TeamActivity1>();
        public ICollection<TeamActivity2> activityTeam2 { get; set; } = new List<TeamActivity2>();
    }
    public class TeamActivity1 { 
        public string teamName { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int point { get; set; }
        public string timeScore { get; set; }
    }
    public class TeamActivity2 {
        public string teamName { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int point { get; set; }
        public string timeScore { get; set; }
    }

    //Match schedule 
    public class Teampoint
    {
        public int? id { get; set; }
        public string? teamName { get; set; }
        public string? teamImage { get; set; }
        public string? teamMatchResultPlay { get; set; }
        public int? tolalScore { get; set; }

    }

    public class MatchlistPointParent
    {
        public int? teamMatchId { get; set; }
        public int? MatchId { get; set; }
        public int? TeamId { get; set; }
        public int? teamMatchResult {  get; set; }
        public string? teamName { get; set; }
        public string? teamImage { get; set; }
        public ICollection<MatchListPoint > halfActionTeam {  get; set; } = new List<MatchListPoint>();
    }
    public class MatchListPoint
    {
        public int? halfId { get; set; }
        public string? halfName { get; set; }
        
        public int? id { get; set; } 
        public int? refereeCompetitionId { get; set; }
        public string? refereeCompetitionName { get; set; }
        public string? scoreTime { get; set; }
        public string? scoreDescription { get; set; }
        public string? scoreType { get; set; }
        public int? scorePoint { get; set; }
        public string? status { get; set; }

    }


}




