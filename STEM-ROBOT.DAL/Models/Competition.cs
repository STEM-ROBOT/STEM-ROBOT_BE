using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Competition
{
    public int Id { get; set; }

    public int? TournamentId { get; set; }

    public int? GenreId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<RefereeCompetition> RefereeCompetitions { get; set; } = new List<RefereeCompetition>();

    public virtual Tournament? Tournament { get; set; }
}
