using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Team
{
    public int Id { get; set; }

    public int? CompetitionId { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public int? TableId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ContactInfo { get; set; }

    public string? Image { get; set; }

    public bool? IsSetup { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual ICollection<ContestantTeam> ContestantTeams { get; set; } = new List<ContestantTeam>();

    public virtual ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();

    public virtual ICollection<TeamRegister> TeamRegisters { get; set; } = new List<TeamRegister>();

    public virtual ICollection<TeamTable> TeamTables { get; set; } = new List<TeamTable>();
}
