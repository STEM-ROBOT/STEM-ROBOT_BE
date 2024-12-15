using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TeamRsp
    {
        public int? Id { get; set; }

        public int? CompetitionId { get; set; }

        public int? ContestantInTeam { get; set;}

        public string? Name { get; set; }

        public string? Status { get; set; }

        public int? TableId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

        public bool? IsSetup { get; set; }

        public List<Constestant> member { get; set; }
    }

    public class Constestant
    {
        public int? ContestantId { get; set; }

        public string? ContestantName { get; set; }
    }

    public class ListTeamRspByTournament
    {
        public int? Id { get; set; }

        public int? CompetitionId { get; set; }


        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }
    }
    public class TeamScheduleRsp
    {
        public int Id { get; set; }
        public DateTime dateStartCompetition { get; set; }
        public DateTime dateEndCompetition { get; set; }

        public TimeSpan hourStartInDay { get; set; }
        public TimeSpan hourEndInDay { get; set; }
        public TimeSpan timePlayMatch { get; set; }

        public ICollection<ScheduleTeam> scheduleTeam { get; set; } = new List<ScheduleTeam>();

    }
    public class ScheduleTeam
    {
        //schedule
        public int Id { get; set; }
        public string StartTime { get; set; }

        public bool? status { get; set; }

        public string location { get; set; } // location name owr bang location

        public int matchId { get; set; } //id matchID

        public ICollection<TeamMatchAdhesion> teamMatch { get; set; } = new List<TeamMatchAdhesion>();

    }
    public class TeamMatchAdhesion
    {
        public int? teamId { get; set; }
        public string? teamName {  get; set; }
        public string? teamLogo { get; set; } // image o bang team
        public string? teamType { get; set; }

    }
}
 