using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class School
{
    public int Id { get; set; }

    public string SchoolName { get; set; } = null!;

    public string? SchoolCode { get; set; }

    public string Address { get; set; } = null!;

    public string? Area { get; set; }

    public string? AreaCode { get; set; }

    public string? Province { get; set; }

    public string? ProvinceCode { get; set; }

    public string? District { get; set; }

    public string? DistrictCode { get; set; }

    public virtual ICollection<Contestant> Contestants { get; set; } = new List<Contestant>();
}
