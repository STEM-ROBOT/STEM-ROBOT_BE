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

        [Required(ErrorMessage = "Bonus points are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Bonus must be a positive value")]
        public int Bonus { get; set; }

        [Required(ErrorMessage = "Minus points are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minus must be a positive value")]
        public int Minus { get; set; }

        [Required(ErrorMessage = "GenreId is required")]
        public int GenreId { get; set; }
    }
}
