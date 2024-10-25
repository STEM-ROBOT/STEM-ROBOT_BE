using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TeamReq
    {
        [Required(ErrorMessage = "CompetitionId is required")]
        public int? CompetitionId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string? Status { get; set; }

        public int? TableId { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Contact Information is required")]
        public string? ContactInfo { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string? Image { get; set; }


    }
}
