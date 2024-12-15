using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class TeamReq
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "ContactInfo cannot exceed 200 characters.")]
        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Contestants list is required.")]
        [MinLength(1, ErrorMessage = "Contestants list must contain at least one contestant.")]
        public List<ContestantTeamReq> Contestants { get; set; } = new List<ContestantTeamReq>();
    }

    public class TeamRegisterReq
    {
        [Required(ErrorMessage = "CompetitionId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CompetitionId must be a positive integer.")]
        public int? CompetitionId { get; set; }

        [Required(ErrorMessage = "AccountId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "AccountId must be a positive integer.")]
        public int? AccountId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "ContactInfo cannot exceed 200 characters.")]
        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "RegisterTime is required.")]
        public DateTime? RegisterTime { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Contestants list is required.")]
        [MinLength(1, ErrorMessage = "Contestants list must contain at least one contestant.")]
        public List<ContestantTeamReq> Contestants { get; set; } = new List<ContestantTeamReq>();
    }

    public class ContestantTeamReq
    {
        [Required(ErrorMessage = "ContestantId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ContestantId must be a positive integer.")]
        public int? ContestantId { get; set; }
    }
}
