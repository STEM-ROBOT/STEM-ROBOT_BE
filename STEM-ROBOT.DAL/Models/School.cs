using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class School
{
    public int Id { get; set; }

    public string? SchoolName { get; set; }

    public string? SchoolCode { get; set; }

    public string? Address { get; set; }

    public string? ProvinceCode { get; set; }

    public string? DistrictCode { get; set; }

    public int? DistrictId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual District? District { get; set; }
}
