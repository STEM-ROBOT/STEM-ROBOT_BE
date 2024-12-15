using System.ComponentModel.DataAnnotations;

namespace STEM_ROBOT.Common.Req
{
    public class PackageReq
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "MaxTournament is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MaxTournament must be a positive integer.")]
        public int? MaxTournament { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive decimal value.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "MaxTeam is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MaxTeam must be a positive integer.")]
        public int? MaxTeam { get; set; }

        [Required(ErrorMessage = "MaxMatch is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MaxMatch must be a positive integer.")]
        public int? MaxMatch { get; set; }
    }
}
