using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class RefereeCompetitionRsp
    {
        public int Id { get; set; }
        public string? refereeEmail { get; set; }
        public DateTime dateStartCompetition { get; set; }
        public DateTime dateEndCompetition { get; set; }

        public TimeSpan hourStartInDay { get; set; }
        public TimeSpan hourEndInDay { get; set; }
        public TimeSpan timePlayMatch { get; set; }

        public ICollection<ScheduleReferee> scheduleReferee { get; set; } = new List<ScheduleReferee>();

    }
    public class ScheduleReferee
    {
        //schedule
        public int Id { get; set; }
        public string StartTime { get; set; }


        public string location { get; set; } // location name owr bang location

        public int matchId { get; set; } //id matchID

        public ICollection<TeamMatchReferee> teamMatch { get; set; } = new List<TeamMatchReferee>();

    }
    public class TeamMatchReferee
    {
        public int? teamId { get; set; }

        public string? teamLogo { get; set; } // image o bang team
        public string? teamType { get; set; }

    }

    public class AssignRefereeCompetitionRsp
    {
        public int CompetitionId { get; set; }
        public int RefereeId { get; set; }
        public string? Role { get; set; }

    }
}
