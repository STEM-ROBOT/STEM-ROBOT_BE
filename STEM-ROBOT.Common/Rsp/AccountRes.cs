using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class AccountRes
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        public string Name { get; set; }


        public string? PhoneNumber { get; set; }


        public string Email { get; set; }



        public string? Image { get; set; }


        public string Status { get; set; }
        
    }
   
    
}
