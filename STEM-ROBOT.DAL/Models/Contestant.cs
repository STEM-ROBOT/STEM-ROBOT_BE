﻿using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Contestant
{
    public int Id { get; set; }

    public int SchoolId { get; set; }

    public int TournamentId { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? Email { get; set; }

    public string? Status { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? Image { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<ContestantCompetition> ContestantCompetitions { get; set; } = new List<ContestantCompetition>();

    public virtual School School { get; set; } = null!;

    public virtual Tournament Tournament { get; set; } = null!;
}
