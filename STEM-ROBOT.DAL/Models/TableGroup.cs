using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TableGroup
{
    public int Id { get; set; }

    public int? StageId { get; set; }

    public string? Name { get; set; }

    public bool? IsAsign { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual Stage? Stage { get; set; }

    public virtual ICollection<TeamTable> TeamTables { get; set; } = new List<TeamTable>();
}
