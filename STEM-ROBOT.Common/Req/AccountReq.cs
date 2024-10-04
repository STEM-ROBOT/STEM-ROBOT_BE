using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class AccountReq
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Image { get; set; }

    }
}
