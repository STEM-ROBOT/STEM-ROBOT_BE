using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public int? MaxTournatment { get; set; }

    public int? SchoolId { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Contestant> Contestants { get; set; } = new List<Contestant>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Referee> Referees { get; set; } = new List<Referee>();

    public virtual School? School { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
