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
    
        
    
}




