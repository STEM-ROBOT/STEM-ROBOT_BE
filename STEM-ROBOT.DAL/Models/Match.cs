using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Match
{
    public int Id { get; set; }

    public int RoundId { get; set; }

    public int TableId { get; set; }

    public DateTime StartDate { get; set; }

    public string? Status { get; set; }

    public DateTime? TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }

    public virtual ICollection<MatchHalf> MatchHalves { get; set; } = new List<MatchHalf>();

    public virtual Stage Round { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual TableGroup Table { get; set; } = null!;

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();
}
