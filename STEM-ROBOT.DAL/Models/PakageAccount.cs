using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class PakageAccount
{
    public int Id { get; set; }

    public int? PackageId { get; set; }

    public int? AccountId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public string? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Package? Package { get; set; }
}
