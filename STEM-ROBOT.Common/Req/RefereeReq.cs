using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class RefereeReq
    {
        [Required(ErrorMessage = "TournamentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TournamentId must be a positive integer.")]
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; }
    }

    public class AssignRefereeReq
    {
        [Required(ErrorMessage = "RefereeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "RefereeId must be a positive integer.")]
        public int RefereeId { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; }
    }
}
