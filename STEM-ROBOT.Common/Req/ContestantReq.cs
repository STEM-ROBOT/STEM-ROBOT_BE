using System;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class ContestantReq
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "School name is required.")]
        public string? SchoolName { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime? StartTime { get; set; }
    }
}
