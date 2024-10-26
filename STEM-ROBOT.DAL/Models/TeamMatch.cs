using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TeamMatch
{
    public int Id { get; set; }

    public int? MatchId { get; set; }

    public int? TeamId { get; set; }

    public string? Result { get; set; }

    public string? NameDefault { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Match? Match { get; set; }

    public virtual Team? Team { get; set; }
}
