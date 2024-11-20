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
}
