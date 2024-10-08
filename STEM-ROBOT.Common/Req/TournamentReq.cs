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
        [Required(ErrorMessage = "Please choose FormatId")]
        public int FormatId { get; set; }

        [Required(ErrorMessage = "TournamentLevel is required")]
        [MaxLength(300, ErrorMessage = "TournamentLevel cannot exceed 300 characters")]
        public string? Level { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(500, ErrorMessage = "Name cannot exceed 500 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "StartDate is required")]
        [DataType(DataType.Date, ErrorMessage = "StartDate must be a valid date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        [DataType(DataType.Date, ErrorMessage = "EndDate must be a valid date")]
       
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(100, ErrorMessage = "Status cannot exceed 100 characters")]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "Mode is required")]
        [MaxLength(100, ErrorMessage = "Mode cannot exceed 100 characters")]
        public string Mode { get; set; } = null!;

        [Required(ErrorMessage = "Location is required")]
        [MaxLength(500, ErrorMessage = "Location cannot exceed 500 characters")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "NumberTeam is required")]
        [Range(1, 1000, ErrorMessage = "NumberTeam must be between 1 and 1000")]
        public int NumberTeam { get; set; }

        [Required(ErrorMessage = "NumberTeamNextRound is required")]
        [Range(1, 1000, ErrorMessage = "NumberTeamNextRound must be between 1 and 1000")]
        public int NumberTeamNextRound { get; set; }

        [Range(1, 100, ErrorMessage = "NumberTable must be between 1 and 50")]
        public int? NumberTable { get; set; }


        [Required(ErrorMessage = "WinScore is required")]
        [Range(1, 100, ErrorMessage = "WinScore must be between 1 and 100")]
        public int WinScore { get; set; }


        [Required(ErrorMessage = "LoseScore is required")]
        [Range(1, 100, ErrorMessage = "LoseScore must be between 1 and 100")]
        public int LoseScore { get; set; }

        [Required(ErrorMessage = "TieScore is required")]
        [Range(1, 100, ErrorMessage = "TieScore must be between 1 and 100")]
        public int TieScore { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [MaxLength(500, ErrorMessage = "Image cannot exceed 500 characters")]
        public string Image { get; set; } = null!;



    }
}

