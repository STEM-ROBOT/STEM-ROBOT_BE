﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class OrderReq
    {
        [Required(ErrorMessage = "AccountId is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "PackageId is required")]
        public int PackageId { get; set; }
    }
}
