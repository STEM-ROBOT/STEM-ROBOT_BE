using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class StageTable
{
    public int Id { get; set; }

    public int? StageId { get; set; }

    public int? TableGroupId { get; set; }

    public virtual Stage? Stage { get; set; }

    public virtual TableGroup? TableGroup { get; set; }
}
