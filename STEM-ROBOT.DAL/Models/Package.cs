using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Package
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? MaxTournament { get; set; }

    public decimal? Price { get; set; }

    public int? MaxTeam { get; set; }

    public int? MaxMatch { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
