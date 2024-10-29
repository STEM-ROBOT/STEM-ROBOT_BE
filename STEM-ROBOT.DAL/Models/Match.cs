using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Match
{
    public int Id { get; set; }

    public int? StageId { get; set; }

    public int? TableId { get; set; }

    public DateTime? StartDate { get; set; }

    public string? Status { get; set; }

    public TimeSpan? TimeIn { get; set; }

    public TimeSpan? TimeOut { get; set; }

    public int? LocationId { get; set; }

    public bool? IsSetup { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<MatchHalf> MatchHalves { get; set; } = new List<MatchHalf>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Stage? Stage { get; set; }

    public virtual TableGroup? Table { get; set; }

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();
}
