
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class AccountReq
    {
        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; } 

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string Password { get; set; } 

        public string? Image { get; set; }

        /*[Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } */

    }
}
