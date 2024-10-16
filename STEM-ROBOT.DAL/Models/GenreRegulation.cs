using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class GenreRegulation
{
    public int Id { get; set; }

    public int? GenreId { get; set; }

    public int? RoleId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Regulation? Role { get; set; }
}
