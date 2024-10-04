using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TeamMatch
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public int TeamId { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
