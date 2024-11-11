using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Action
{
    public int Id { get; set; }

    public int? MatchHalfId { get; set; }

    public int? ScoreCategoryId { get; set; }

    public int? Score { get; set; }

    public TimeSpan? EventTime { get; set; }

    public int? TeamMatchId { get; set; }

    public int? RefereeCompetitionId { get; set; }

    public virtual MatchHalf? MatchHalf { get; set; }

    public virtual RefereeCompetition? RefereeCompetition { get; set; }

    public virtual ScoreCategory? ScoreCategory { get; set; }

    public virtual TeamMatch? TeamMatch { get; set; }
}
