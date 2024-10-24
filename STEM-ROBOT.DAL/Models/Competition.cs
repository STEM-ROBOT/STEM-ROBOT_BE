using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Competition
{
    public int Id { get; set; }

    public int? TournamentId { get; set; }

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

    public virtual CompetitionFormat? Format { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<RefereeCompetition> RefereeCompetitions { get; set; } = new List<RefereeCompetition>();

    public virtual ICollection<ScoreCategory> ScoreCategories { get; set; } = new List<ScoreCategory>();

    public virtual ICollection<Stage> Stages { get; set; } = new List<Stage>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    public virtual Tournament? Tournament { get; set; }
}
