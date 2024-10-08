using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Team
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? UpdateDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Competition Competition { get; set; } = null!;

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();
}
