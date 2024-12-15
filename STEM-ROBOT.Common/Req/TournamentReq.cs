using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class TournamentReq
    {
        [Required(ErrorMessage = "TournamentLevel is required.")]
        [StringLength(50, ErrorMessage = "TournamentLevel cannot exceed 50 characters.")]
        public string? TournamentLevel { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string? Location { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        [Phone(ErrorMessage = "Invalid phone format.")]
        public string? Phone { get; set; }

        public string? Introduce { get; set; }

        public string? ProvinceCode { get; set; }

        public string? AreaCode { get; set; }

        public ICollection<TournamentCompetition> competition {  get; set; } = new List<TournamentCompetition>();
   
    }

    public class TournamentCompetition
    {
        [Required(ErrorMessage = "GenreId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "GenreId must be a positive integer.")]
        public int? GenreId { get; set; }

        public DateTime? RegisterTime { get; set; } = DateTime.UtcNow;

        public bool? IsActive { get; set; } = false;

        public string? Regulation { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "NumberContestantTeam must be a positive integer.")]
        public int? NumberContestantTeam { get; set; }

        public bool? IsTop { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "NumberView must be zero or a positive integer.")]
        public int? NumberView { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "FormatId must be a positive integer.")]
        public int? FormatId { get; set; }

        public DateTime? StartTime { get; set; } = DateTime.UtcNow;

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Mode is required.")]
        [StringLength(20, ErrorMessage = "Mode cannot exceed 20 characters.")]
        public string? Mode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "NumberTeam must be a positive integer.")]
        public int? NumberTeam { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "NumberTeamNextRound must be a positive integer.")]
        public int? NumberTeamNextRound { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "NumberTable must be a positive integer.")]
        public int? NumberTable { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "WinScore must be zero or a positive integer.")]
        public int? WinScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "LoseScore must be zero or a positive integer.")]
        public int? LoseScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "TieScore must be zero or a positive integer.")]
        public int? TieScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "NumberSubReferee must be zero or a positive integer.")]
        public int? NumberSubReferee { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "NumberTeamReferee must be zero or a positive integer.")]
        public int? NumberTeamReferee { get; set; }

        public TimeSpan? TimeOfMatch { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string Location { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public TimeSpan? TimeBreak { get; set; } = TimeSpan.Zero;

        public DateTime? EndTime { get; set; } = DateTime.UtcNow;

        public DateTime? TimeStartPlay { get; set; } = DateTime.UtcNow;

        public DateTime? TimeEndPlay { get; set; } = DateTime.UtcNow;
    }
}
