﻿using System;
using System.Collections.Generic;

namespace STEM_ROBOT.DAL.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? TransactionCode { get; set; }

    public decimal? Amount { get; set; }

    public string? Status { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public virtual Order? Order { get; set; }
}
