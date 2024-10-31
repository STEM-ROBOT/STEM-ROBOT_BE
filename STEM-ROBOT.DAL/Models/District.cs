using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class District
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ProvinceId { get; set; }

    public string? DistrictCode { get; set; }

    public string? ProvinceCode { get; set; }

    public virtual Province? Province { get; set; }

    public virtual ICollection<School> Schools { get; set; } = new List<School>();
}
