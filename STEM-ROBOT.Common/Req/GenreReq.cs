using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class GenreReq
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; } = null!;

        public string? Rules { get; set; }

        public string Image { get; set; } = null!;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
    }
}
