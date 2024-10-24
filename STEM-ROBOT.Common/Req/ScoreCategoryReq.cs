using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class ScoreCategoryReq
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "CompetitionId is required")]
        public int? CompetitionId { get; set; }

        [Required(ErrorMessage = "Point is required")]
        public int Point { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
    }
}
