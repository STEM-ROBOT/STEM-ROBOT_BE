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
        public string? Status { get; set; }

        public string? Phone { get; set; }
        public ICollection<TournamentComeptition> competition {  get; set; } = new List<TournamentComeptition>();
    }
    public class TournamentComeptition
    {
       

        public int? GenreId { get; set; }

        public DateTime? RegisterTime = DateTime.UtcNow;

        public bool? IsActive = false;

        public string? Regulation = null;

        public int? NumberContestantTeam = null;

        public bool? IsTop = null;

        public int? NumberView = null;

        public int? FormatId = null;

        public DateTime? StartTime = DateTime.UtcNow;

        public string? Status = null;

        public string? Mode = null;

        public int? NumberTeam = null!;

        public int? NumberTeamNextRound = null!;

        public int? NumberTable = null!;

        public int? WinScore = null!;

        public int? LoseScore = null!;

        public int? TieScore = null!;

        public int? NumberSubReferee = null!;

        public int? NumberTeamReferee = null!;

        public TimeSpan? TimeOfMatch = null!;



        public string Name  = null!;

       
        public string Location  = null!;

        public string Image = null!;


        public TimeSpan? TimeBreak = TimeSpan.Zero;

        public DateTime? EndTime = DateTime.UtcNow;

        public DateTime? TimeStartPlay =DateTime.UtcNow;

        public DateTime? TimeEndPlay = DateTime.UtcNow;
    }
}

