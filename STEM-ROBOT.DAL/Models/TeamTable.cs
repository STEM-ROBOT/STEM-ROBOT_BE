using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TeamTable
{
    public int Id { get; set; }

    public int? TeamId { get; set; }

    public int? TableGroupId { get; set; }

    public virtual TableGroup? TableGroup { get; set; }

    public virtual Team? Team { get; set; }
}
