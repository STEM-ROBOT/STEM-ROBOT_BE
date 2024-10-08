using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int RefereeId { get; set; }

    public int MatchId { get; set; }

    public int LocationId { get; set; }

    public DateTime StartTime { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;

    public virtual Referee Referee { get; set; } = null!;
}
