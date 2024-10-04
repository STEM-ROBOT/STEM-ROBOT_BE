using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Team
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public int? TableGroupId { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Competition Competition { get; set; } = null!;

    public virtual ICollection<ContestantCompetition> ContestantCompetitions { get; set; } = new List<ContestantCompetition>();

    public virtual TableGroup? TableGroup { get; set; }

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();
}
