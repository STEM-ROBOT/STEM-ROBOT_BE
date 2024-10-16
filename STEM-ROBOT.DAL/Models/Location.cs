using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Location
{
    public int Id { get; set; }

    public string? Address { get; set; }

    public string? ContactPerson { get; set; }

    public string? Status { get; set; }

    public int? CompetitionId { get; set; }

    public virtual Competition? Competition { get; set; }
}
