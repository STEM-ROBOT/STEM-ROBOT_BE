using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class ScheduleRsp
    {
        public int RefereeId { get; set; }

        public int MatchId { get; set; }

        public int LocationId { get; set; }

        public DateTime StartTime { get; set; }
    }

    public class ScheduleConfigRsp
    {
        public bool? IsSchedule { get; set; }
        public ICollection<SchedulMainRefereeRsp> Referees { get; set; } = new List<SchedulMainRefereeRsp>();
        public ICollection<SchedulSubRefereeRsp> MatchReferees { get; set; } = new List<SchedulSubRefereeRsp>();
        public ICollection<SchedulRoundsRefereeRsp> Rounds { get; set; } = new List<SchedulRoundsRefereeRsp>();
    }
    public class SchedulMainRefereeRsp
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class SchedulSubRefereeRsp
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class SchedulRoundsRefereeRsp
    {
        public int RoundId { get; set; }
        public string roundName { get; set; }
        public ICollection<SchedulRoundsMatchsRefereeRsp> Matches { get; set; } = new List<SchedulRoundsMatchsRefereeRsp>();
    }

    public class SchedulRoundsMatchsRefereeRsp
    {
        public int matchId { get; set; }
        public int mainReferee { get; set; }
        public string mainRefereeName { get; set; }
        public ICollection<SchedulMainMatchRefereeRsp> matchRefereesdata { get; set; } = new List<SchedulMainMatchRefereeRsp>();
        public DateTime date { get; set; }
        public TimeSpan timeIn { get; set; }
        public string arena { get; set; }
    }
    public class SchedulMainMatchRefereeRsp
    {
        public int SubRefereeId { get; set; }
        public string SubRefereeName { get; set; }

    }

    public class ScheduleSecurityRsp
    {
        public int timeOut { get; set; }

        public int textView { get; set; }
    }
    public class ScheduleMatchScoreRsp
    {
        public int scoreId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Point { get; set; }

    }
    public class ScheduleMatchInfoRsp
    {
        public int matchId { get; set; }
        public string startTime { get; set; }
        public string startDate { get; set; }
        public string endTime { get; set; }
        public string durationHaft { get; set; }
        public string breakHaftTime { get; set; }
        public ICollection<ScheduleMatchHaftRsp> haftMatch { get; set; } = new List<ScheduleMatchHaftRsp>();

        public ICollection<ScheduleMatchTeamMatchRsp> teamMatch { get; set; } = new List<ScheduleMatchTeamMatchRsp>();
    }
    public class ScheduleMatchHaftRsp
    {

        public int HaftId { get; set; }
        public string HaftName { get; set; }

    }
    public class ScheduleMatchTeamMatchRsp
    {

        public int teamMatchId { get; set; }
        public string teamName { get; set; }
        public string teamLogo { get; set; }

    }

}
