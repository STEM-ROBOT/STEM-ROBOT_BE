using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Regulation
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<GenreRegulation> GenreRegulations { get; set; } = new List<GenreRegulation>();
}
