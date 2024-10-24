using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TournamentReq
    {

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(500, ErrorMessage = "Name cannot exceed 500 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Location is required")]
        [MaxLength(500, ErrorMessage = "Location cannot exceed 500 characters")]
        public string Location { get; set; } = null!;

        public string Image { get; set; } = null!;



    }
}

