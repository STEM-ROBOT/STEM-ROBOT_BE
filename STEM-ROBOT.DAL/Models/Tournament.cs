using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public int? FormatId { get; set; }

    public string? TournamentLevel { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public string? Mode { get; set; }

    public string? Image { get; set; }

    public int? NumberTeam { get; set; }

    public int? NumberTeamNextRound { get; set; }

    public int? NumberTable { get; set; }

    public int? WinScore { get; set; }

    public int? LoseScore { get; set; }

    public int? TieScore { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Contestant> Contestants { get; set; } = new List<Contestant>();

    public virtual TournamentFormat? Format { get; set; }

    public virtual ICollection<Referee> Referees { get; set; } = new List<Referee>();
}
