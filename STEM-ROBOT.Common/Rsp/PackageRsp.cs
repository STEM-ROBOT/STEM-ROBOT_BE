﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class PackageRsp
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? MaxTournament { get; set; }

        public decimal? Price { get; set; }

        public int MaxTeam { get; set; }
        public int MaxMatch { get; set; }
    }
}
