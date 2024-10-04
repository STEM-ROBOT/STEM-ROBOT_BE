using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class ContestantCompetition
{
    public int Id { get; set; }

    public int ContestantId { get; set; }

    public int CompetitionId { get; set; }

    public int TeamId { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual Contestant Contestant { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
