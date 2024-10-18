using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class PackageReq
    {
        [Required(ErrorMessage = "PackageId is required")]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage ="MaxTournament is required")]
        public int? MaxTournament { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { get; set; }
    }
}
