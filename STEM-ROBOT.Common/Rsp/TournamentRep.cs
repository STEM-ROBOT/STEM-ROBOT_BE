using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TournamentListRep
    {
        public int totalPages { get; set; }
        public ICollection<TournamentRep> tournamentRep { get; set; } = new List<TournamentRep>();
    }
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
        public string? TournamentLevel { get; set; }

        public DateTime? CreateDate { get; set; }
        public int? views { get; set; }
        public string? Introduce { get; set; }

        public List<ImageCompetition> imagesCompetition { get; set; } = new List<ImageCompetition>();

    }
    public class ImageCompetition
    {
        public string imageCompetition { get; set; }
    }
    public class TournamentInforRsp
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public int contestant { get; set; }
        public int? Views { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? TournamentLevel { get; set; }
        public int competitionNumber { get; set; }

        public string? phoneNumber { get; set; }
        public int competitionActivateNumber { get; set; }
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
