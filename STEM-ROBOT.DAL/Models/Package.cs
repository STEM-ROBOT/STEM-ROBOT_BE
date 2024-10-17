using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Package
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? MaxTournament { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<PakageAccount> PakageAccounts { get; set; } = new List<PakageAccount>();
}
