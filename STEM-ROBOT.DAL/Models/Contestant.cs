using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Contestant
{
    public int Id { get; set; }

    public int? TournamentId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Status { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Image { get; set; }

    public int? AccountId { get; set; }

    public string? SchoolName { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<ContestantTeam> ContestantTeams { get; set; } = new List<ContestantTeam>();

    public virtual Tournament? Tournament { get; set; }
}
