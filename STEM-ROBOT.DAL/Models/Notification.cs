using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public int? AccountId { get; set; }

    public string? RouterUi { get; set; }

    public virtual Account? Account { get; set; }
}
