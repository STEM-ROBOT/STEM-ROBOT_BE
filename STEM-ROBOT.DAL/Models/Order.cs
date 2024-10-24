using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? PackageId { get; set; }

    public int? AccountId { get; set; }

    public string? Status { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? LinkPayAgain { get; set; }

    public decimal? Amount { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Package? Package { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
