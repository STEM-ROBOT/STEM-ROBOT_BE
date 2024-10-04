using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int RefereeId { get; set; }

    public int MatchId { get; set; }

    public int PositionId { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;

    public virtual RefereeCompetition Referee { get; set; } = null!;
}
