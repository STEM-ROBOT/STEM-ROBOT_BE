using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int? RefereeId { get; set; }

    public int? MatchId { get; set; }

    public DateTime? StartTime { get; set; }

    public string? OptCode { get; set; }

    public virtual Match? Match { get; set; }

    public virtual RefereeCompetition? Referee { get; set; }
}
