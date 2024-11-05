using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class RefereeRsp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public int TournamentId { get; set; }
        public string Image { get; set; }
    }

    public class RefereeTournament
    {
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public string? Image { get; set; }

        public string? NameTournament { get; set; }

        public string? Location { get; set; }

        public string? ImageTournament { get; set;}

        public ICollection<ListRefereeCompetition> referee { get; set; } = new List<ListRefereeCompetition>();

    }

    public class ListRefereeCompetition
    {
        public int Id { get; set; }

        public int? RefereeId { get; set; }

        public int? CompetitionId { get; set; }

        public string? imageGenre { get; set; }

        public string? nameGenre { get; set; }
    }
}
