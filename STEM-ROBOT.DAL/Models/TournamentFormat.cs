using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class TournamentFormat
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
