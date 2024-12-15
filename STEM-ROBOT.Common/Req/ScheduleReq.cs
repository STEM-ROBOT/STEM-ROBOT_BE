using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class ScheduleReq
    {
        [Required(ErrorMessage = "RefereeCompetitionId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "RefereeCompetitionId must be a positive integer greater than or equal to 1.")]
        public int? RefereeCompetitionId { get; set; }

        [Required(ErrorMessage = "MatchId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MatchId must be a positive integer greater than or equal to 1.")]
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
