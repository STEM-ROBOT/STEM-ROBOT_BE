using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TeamRegister
{
    public int Id { get; set; }

    public int? CompetitionId { get; set; }

    public int? TeamId { get; set; }

    public int? AccountId { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ContactInfo { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public DateTime? RegisterTime { get; set; }

    public string? Email { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual ICollection<ContestantTeam> ContestantTeams { get; set; } = new List<ContestantTeam>();

    public virtual Team? Team { get; set; }
}
