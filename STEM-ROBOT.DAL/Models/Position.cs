using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Position
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public int CompetitionId { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
