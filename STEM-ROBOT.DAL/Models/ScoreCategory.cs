using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class ScoreCategory
{
    public int Id { get; set; }

    public int? GenreId { get; set; }

    public string? Description { get; set; }

    public int? Bonus { get; set; }

    public int? Minus { get; set; }

    public virtual Genre? Genre { get; set; }
}
