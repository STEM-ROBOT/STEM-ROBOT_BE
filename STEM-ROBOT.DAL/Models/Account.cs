﻿using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Account
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public int? MaxTournatment { get; set; }

    public int? UsedTournament { get; set; }

    public virtual ICollection<PakageAccount> PakageAccounts { get; set; } = new List<PakageAccount>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
