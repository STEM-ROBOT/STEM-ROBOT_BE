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
        [Required(ErrorMessage = "Please enter Name")]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage ="Status is required")]
        public string? Status { get; set; }
        
        public int? TableId { get; set; }
    }
}
