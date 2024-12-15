using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class TeamTableReq
    {
        [Required(ErrorMessage = "TeamId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId must be a positive integer.")]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "TableGroupId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TableGroupId must be a positive integer.")]
        public int TableGroupId { get; set; }
    }

    public class TableAssignmentReq
    {   
        public ICollection<TableAssign> tableAssign { get; set; } = new List<TableAssign>();
    }

    public class TableAssign
    {
        [Required(ErrorMessage = "TableGroupId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TableGroupId must be a positive integer.")]
        public int TableGroupId { get; set; }

        [Required(ErrorMessage = "TeamNextRound is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamNextRound must be a positive integer.")]
        public int TeamNextRound { get; set; }

        [Required(ErrorMessage = "TableGroupName is required.")]
        [StringLength(100, ErrorMessage = "TableGroupName cannot exceed 100 characters.")]
        public string TableGroupName { get; set; }

        [Required(ErrorMessage = "Teams list is required.")]
        [MinLength(1, ErrorMessage = "Teams list must contain at least one team.")]
        public List<int> Teams { get; set; } = new List<int>();
    }
}
