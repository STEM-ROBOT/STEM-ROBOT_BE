using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TournamentRep
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        public int contestant { get; set; }

        public int competitionNumber { get; set; }

        public int competitionActivateNumber { get; set; }

        public List<ImageCompetition> imagesCompetition { get; set; } = new List<ImageCompetition>();
        public int? views { get; set; }
    }
    public class ImageCompetition
    {
        public string imageCompetition { get; set; }
    }

    public class TournamentModerator
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        public int genre { get; set; }

        public int IsActive { get; set; }
       

    }
}
