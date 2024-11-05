﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class AccountRsp
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        public int? CountTournament { get; set; }

        public int? SchoolId { get; set; }

        public string? Role { get; set; }

        public string? PackageName { get; set; }
        public int CountContestant { get; set; }
    }
   

    
}
