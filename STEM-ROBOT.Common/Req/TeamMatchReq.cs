using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class TeamMatchReq
    {
        [Required(ErrorMessage = "TeamId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId must be a positive integer.")]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "IsHome status is required.")]
        public bool IsHome { get; set; }
    }

    public class AssignTeamsToMatchesInStageTableReq
    {
        [Required(ErrorMessage = "MatchId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MatchId must be a positive integer.")]
        public int MatchId { get; set; }

     
        public List<TeamMatchReq> Teams { get; set; } = new List<TeamMatchReq>();
    }

    public class TeamMatchConfigCompetition
    {
        [Required(ErrorMessage = "TeamId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId must be a positive integer.")]
        public int teamId { get; set; }

        [Required(ErrorMessage = "TeamName is required.")]
        [StringLength(100, ErrorMessage = "TeamName cannot exceed 100 characters.")]
        public string teamName { get; set; }

        [Required(ErrorMessage = "TeamMatchId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamMatchId must be a positive integer.")]
        public int teamMatchId { get; set; }

        [Required(ErrorMessage = "MatchId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MatchId must be a positive integer.")]
        public int matchId { get; set; }
    }
}
