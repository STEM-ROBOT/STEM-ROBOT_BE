﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class CompetitionRep
    {
        public int Id { get; set; }

        public int? TournamentId { get; set; }

        public string TournamentName { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public int? GenreId { get; set; }

        public string? NameGenre { get; set; }

        public string? Address { get; set; }

        public string? ContactPerson { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int? FormatId { get; set; }

        public DateTime? StartTime { get; set; }

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
    public class ListCompetiton
    {
        public int Id { get; set; }

        public string? Image { get; set; }
        public string? Name { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
    }
}
