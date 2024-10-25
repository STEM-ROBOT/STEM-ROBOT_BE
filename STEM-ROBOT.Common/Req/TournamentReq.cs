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
       

        public string? TournamentLevel { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Image { get; set; }


        public ICollection<TournamentComeptition> competition {  get; set; } = new List<TournamentComeptition>();
    }
    public class TournamentComeptition
    {
       

        public int? GenreId { get; set; }

        public DateTime? RegisterTime { get; set; }

        public bool? IsActive { get; set; }

        public string? Regulation { get; set; }

        public int? NumberContestantTeam { get; set; }

        public bool? IsTop { get; set; }

        public int? NumberView { get; set; }

        public int? FormatId { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Status { get; set; }

        public string? Mode { get; set; }

        public int? NumberTeam { get; set; }

        public int? NumberTeamNextRound { get; set; }

        public int? NumberTable { get; set; }

        public int? WinScore { get; set; }

        public int? LoseScore { get; set; }

        public int? TieScore { get; set; }

        public int? NumberSubReferee { get; set; }

        public int? NumberTeamReferee { get; set; }

        public TimeSpan? TimeOfMatch { get; set; }

        public TimeSpan? TimeBreak { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? TimeStartPlay { get; set; }

        public DateTime? TimeEndPlay { get; set; }
    }
}

