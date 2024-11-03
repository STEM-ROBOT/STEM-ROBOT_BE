﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class LocationReq
    {
        public string Address { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string ContactPerson{ get; set; }
        public string CompetitionId { get; set; }
    }
}
