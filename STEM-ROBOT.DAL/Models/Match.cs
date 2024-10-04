using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Match
{
    public int Id { get; set; }

    public int StageId { get; set; }

    public int? TableId { get; set; }

    public DateTime StartDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }

    public virtual ICollection<MatchHalf> MatchHalves { get; set; } = new List<MatchHalf>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Stage Stage { get; set; } = null!;

    public virtual TableGroup? Table { get; set; }

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();
}
