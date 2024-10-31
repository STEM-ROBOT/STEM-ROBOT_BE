using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Province
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? AreaId { get; set; }

    public string? ProvinceCode { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
