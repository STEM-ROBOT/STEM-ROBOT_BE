using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class AccountReq
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters long.")]
        public string? Password { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        [Required(ErrorMessage = "School ID is required.")]
        public int? SchoolId { get; set; }

        public string? Role  = "MD";
    }

    public class ChangePass
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string? PasswordOld { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(5, ErrorMessage = "New password must be at least 5 characters long.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match.")]
        public string? ConfirmPass { get; set; }

        
        public bool VerifyPasswordChange(string currentPassword)
        {
           
            if (PasswordOld != currentPassword)
            {
                throw new ValidationException("Old password is incorrect.");
            }

           
            if (NewPassword != ConfirmPass)
            {
                throw new ValidationException("New password and confirmation do not match.");
            }

            return true;
        }
    }
}
