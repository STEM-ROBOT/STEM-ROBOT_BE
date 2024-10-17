using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<ScoreCategory> ScoreCategories { get; set; } = new List<ScoreCategory>();
}
