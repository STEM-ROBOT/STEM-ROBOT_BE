﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class PaymentRsp
    {
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int OrderCode { get; set; }
        public string Status { get; set; }
    }
}
