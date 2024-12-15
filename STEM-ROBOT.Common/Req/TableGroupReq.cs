using System;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class TableGroupReq
    {
        [Required(ErrorMessage = "StageId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StageId must be a positive integer.")]
        public int? StageId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }
    }
}
