using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class ScheduleReq
    {
        public int? RefereeCompetitionId { get; set; }

        public int? MatchId { get; set; }
    }
    public class ScheduleRandomReq
    {
        public int? teamMatchWinId { get; set; }
        public int? teamMatchRandomId { get; set; }
        public int? teamId { get; set; }

        public ICollection<ScheduleRandomTeamMatchReq> TeamMatchs{ get; set; } = new List<ScheduleRandomTeamMatchReq>();
    }
    public class ScheduleRandomTeamMatchReq
    {
        public int? Id { get; set; }
        public int? HitCount { get; set; }


    }
}
