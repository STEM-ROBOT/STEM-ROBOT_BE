using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class ContestantTeam
{
    public int Id { get; set; }

    public int? ContestantId { get; set; }

    public int? TeamId { get; set; }

    public int? TeamRegisterId { get; set; }

    public virtual Contestant? Contestant { get; set; }

    public virtual Team? Team { get; set; }

    public virtual TeamRegister? TeamRegister { get; set; }
}
