using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Action
{
    public int Id { get; set; }

    public int MatchHalfId { get; set; }

    public int ScoreId { get; set; }

    public int TeamId { get; set; }

    public int Point { get; set; }

    public DateTime EventTime { get; set; }

    public virtual MatchHalf MatchHalf { get; set; } = null!;

    public virtual Score Score { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
