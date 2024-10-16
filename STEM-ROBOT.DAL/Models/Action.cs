using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Action
{
    public int Id { get; set; }

    public int? MatchHalfId { get; set; }

    public int? ScoreCategoryId { get; set; }

    public int? TeamId { get; set; }

    public int? Score { get; set; }

    public DateTime? EventTime { get; set; }

    public virtual MatchHalf? MatchHalf { get; set; }

    public virtual Team? Team { get; set; }
}
