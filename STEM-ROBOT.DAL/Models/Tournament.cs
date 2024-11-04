using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public string? TournamentLevel { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Contestant> Contestants { get; set; } = new List<Contestant>();

    public virtual ICollection<Referee> Referees { get; set; } = new List<Referee>();
}
