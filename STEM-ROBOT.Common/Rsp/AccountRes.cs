using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class AccountRes
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Image { get; set; }
    }
}
