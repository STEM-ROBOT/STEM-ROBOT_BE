using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class ScoreCategory
{
    public int Id { get; set; }

    public int? CompetitionId { get; set; }

    public string? Description { get; set; }

    public int? Point { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Competition? Competition { get; set; }
}
