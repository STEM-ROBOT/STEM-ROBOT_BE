using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TableGroup
{
    public int Id { get; set; }

    public int RoundId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Status { get; set; }

    public int? UpdateBy { get; set; }

    public int? CreateBy { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual Stage Round { get; set; } = null!;
}
