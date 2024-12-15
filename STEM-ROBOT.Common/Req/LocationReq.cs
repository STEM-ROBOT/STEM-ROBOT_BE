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
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(1000, ErrorMessage = "Address cannot exceed 1000 characters.")]
        public string Address { get; set; }

    }
}
