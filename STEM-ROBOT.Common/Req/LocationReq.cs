using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class LocationReq
    {
        [Required(ErrorMessage = "LocationId is required")]
        [MaxLength(250, ErrorMessage = "LocationId can only be up to 250 characters long")]
        public string Address { get; set; } = null!;

        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "CompetitionId is required")]
        public int CompetitionId { get; set; }
    }
}
