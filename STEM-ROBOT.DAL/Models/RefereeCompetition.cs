using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class RefereeCompetition
{
    public int Id { get; set; }

    public int RefereeId { get; set; }

    public int CompetitionId { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual Referee Referee { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
