using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TeamMatch
{
    public int Id { get; set; }

    public int? MatchId { get; set; }

    public int? TeamId { get; set; }

    public string? NameDefault { get; set; }

    public bool? IsPlay { get; set; }

    public string? ResultPlay { get; set; }

    public int? TotalScore { get; set; }

    public bool? IsHome { get; set; }

    public bool? IsSetup { get; set; }

    public string? MatchWinCode { get; set; }

    public string? ResultPlayTable { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Match? Match { get; set; }

    public virtual Team? Team { get; set; }
}
