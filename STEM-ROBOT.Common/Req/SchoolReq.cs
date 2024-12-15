using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class SchoolReq
    {
        [Required(ErrorMessage = "School name is required.")]
        [StringLength(200, ErrorMessage = "School name cannot exceed 200 characters.")]
        public string? SchoolName { get; set; }

        [Required(ErrorMessage = "School code is required.")]
        [StringLength(20, ErrorMessage = "School code cannot exceed 20 characters.")]
        public string? SchoolCode { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(400, ErrorMessage = "Address cannot exceed 400 characters.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Area cannot exceed 50 characters.")]
        public string? Area { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(50, ErrorMessage = "Province cannot exceed 50 characters.")]
        public string? Province { get; set; }

        [Required(ErrorMessage = "District is required.")]
        [StringLength(50, ErrorMessage = "District cannot exceed 50 characters.")]
        public string? District { get; set; }

        [StringLength(20, ErrorMessage = "Province code cannot exceed 20 characters.")]
        public string? ProvinceCode { get; set; }

        [StringLength(20, ErrorMessage = "District code cannot exceed 20 characters.")]
        public string? DistrictCode { get; set; }
    }
}
