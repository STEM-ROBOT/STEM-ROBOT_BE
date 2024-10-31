using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Area
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
}
