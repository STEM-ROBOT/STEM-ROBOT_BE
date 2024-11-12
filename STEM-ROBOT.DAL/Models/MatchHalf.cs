using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class MatchHalf
{
    public int Id { get; set; }

    public int? MatchId { get; set; }

    public string? Status { get; set; }

    public DateTime? TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }

    public string? HalfName { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Match? Match { get; set; }
}
