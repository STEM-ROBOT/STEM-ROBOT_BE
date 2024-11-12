using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class RefereeRsp
    {
        public int NumberLocation { get; set; }
        public bool IsReferee { get; set; }
        public ICollection<ListRefereeRsp> listRefereeRsps { get; set; }  = new List<ListRefereeRsp>();
    }
    public class ListRefereeRsp
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
        public int? TournamentId { get; set; }
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public string? avatar { get; set; }

        public string? nameTournament { get; set; }

        public string? Location { get; set; }

        public string? ImageTournament { get; set;}
        public string? role { get; } = "TRỌNG TÀI";
       

        public ICollection<ListRefereeCompetition> referee { get; set; } = new List<ListRefereeCompetition>();

    }

    public class ListRefereeCompetition
    {
        public int Id { get; set; }

        public int? RefereeId { get; set; }

        public int? CompetitionId { get; set; }

        public string? Role { get; set; }


        public string? imageGenre { get; set; }

        public string? nameGenre { get; set; }
    }
    public class ListRefereeSchedule
    {
        public int Id { get; set; }

        public int? TournamentId { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public string? Image { get; set; }

        public int? AccountId { get; set; }
        public ICollection<ListSchedule> listschedule { get; set; } = new List<ListSchedule>();
    }

    public class ListSchedule
    {
        public int Id { get; set; }

        public int? RefereeCompetitionId { get; set; }

        public int? MatchId { get; set; }

        public DateTime? StartTime { get; set; }

        public string? OptCode { get; set; }

        public DateTime? TimeOut { get; set; }

        public string? BackupReferee { get; set; }
    }
}
