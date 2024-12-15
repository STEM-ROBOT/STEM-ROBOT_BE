using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class GenreReq
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(300, ErrorMessage = "Name cannot exceed 300 characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "IsTop status is required.")]
        public bool? IsTop { get; set; }

        [StringLength(200, ErrorMessage = "HintRule cannot exceed 200 characters.")]
        public string? HintRule { get; set; }

        [StringLength(200, ErrorMessage = "HintScore cannot exceed 200 characters.")]
        public string? HintScore { get; set; }
    }
}
