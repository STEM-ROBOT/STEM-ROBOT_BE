using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Rules { get; set; }

    public string Image { get; set; } = null!;

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
