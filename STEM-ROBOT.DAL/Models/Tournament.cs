using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int TournamentFormatId { get; set; }

    public string? TournamentLevel { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public string Mode { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int MaxTeam { get; set; }

    public int? NumberTable { get; set; }

    public int TeamFormat { get; set; }

    public int TeamMember { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Contestant> Contestants { get; set; } = new List<Contestant>();

    public virtual ICollection<Referee> Referees { get; set; } = new List<Referee>();

    public virtual TournamentFormat TournamentFormat { get; set; } = null!;
}
