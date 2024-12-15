using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class MatchReq
    {
        [Required(ErrorMessage = "RoundId is required.")]
        public int? RoundId { get; set; }

        [Required(ErrorMessage = "TableId is required.")]
        public int? TableId { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }
    }

    public class MatchConfigReq
    {
        [Required(ErrorMessage = "TimeOfMatch is required.")]
        public TimeSpan? TimeOfMatch { get; set; }

        [Required(ErrorMessage = "TimeBreak is required.")]
        public TimeSpan? TimeBreak { get; set; }

        [Required(ErrorMessage = "NumberHaft is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "NumberHaft must be a positive integer.")]
        public int? NumberHaft { get; set; }

        [Required(ErrorMessage = "BreakTimeHaft is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BreakTimeHaft must be a positive integer.")]
        public int? BreakTimeHaft { get; set; }

        [Required(ErrorMessage = "TimeOfHaft is required.")]
        public TimeSpan? TimeOfHaft { get; set; }

        public TimeSpan? TimeStartPlay { get; set; }

        public TimeSpan? TimeEndPlay { get; set; }

        public DateTime? startTime { get; set; }

        [Required(ErrorMessage = "Matchs collection is required.")]
        public ICollection<MatchDataTimeReq> matchs { get; set; } = new List<MatchDataTimeReq>();
    }

    public class MatchDataTimeReq
    {
        [Required(ErrorMessage = "Id is required.")]
        public int? id { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime? startDate { get; set; }

        [Required(ErrorMessage = "LocationId is required.")]
        public int? locationId { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }
    }
}
