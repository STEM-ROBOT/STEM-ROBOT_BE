﻿using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class RefereeCompetition
{
    public int Id { get; set; }

    public int? RefereeId { get; set; }

    public int? CompetitionId { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Competition? Competition { get; set; }

    public virtual Referee? Referee { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
