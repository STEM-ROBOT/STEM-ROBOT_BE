using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TournamentFormatReq
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = null!;
        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; } = null!;
    }
}
