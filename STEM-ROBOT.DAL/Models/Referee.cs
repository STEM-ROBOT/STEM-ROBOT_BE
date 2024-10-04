using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Referee
{
    public int Id { get; set; }

    public int TournamentId { get; set; }

    public string Role { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string Name { get; set; } = null!;

    public string? Status { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<RefereeCompetition> RefereeCompetitions { get; set; } = new List<RefereeCompetition>();

    public virtual Tournament Tournament { get; set; } = null!;
}
