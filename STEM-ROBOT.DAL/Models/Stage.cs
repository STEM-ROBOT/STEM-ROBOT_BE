using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Stage
{
    public int Id { get; set; }

    public int? CompetitionId { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }

    public string? StageMode { get; set; }

    public string? StageCheck { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<TableGroup> TableGroups { get; set; } = new List<TableGroup>();
}
