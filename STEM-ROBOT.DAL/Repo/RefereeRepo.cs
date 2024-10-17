﻿using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class RefereeRepo : GenericRep<Referee>
    {
        public RefereeRepo(StemdbContext context) : base(context) { }
    }
}
