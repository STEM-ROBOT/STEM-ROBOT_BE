using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Team
{
    public int Id { get; set; }

    public int? CompetitionId { get; set; }

    public string? Name { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public int? TableId { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual ICollection<ContestantCompetition> ContestantCompetitions { get; set; } = new List<ContestantCompetition>();

    public virtual TableGroup? Table { get; set; }

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();
}
