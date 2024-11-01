using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class RefereeReq
    {
        public int? TournamentId { get; set; }

        public string Name { get; set; }


        public string Image { get; set; }


        public string Email { get; set; }


        public string PhoneNumber { get; set; }


        public string Status { get; set; }
    }
    public class AssginRefereeReq
    {
        public int Id { get; set; }
        public int? TournamentId { get; set; }

        public string Name { get; set; }


        public string Image { get; set; }


        public string Email { get; set; }


        public string PhoneNumber { get; set; }


        public string Status { get; set; }

        public string Role { get; set; }
    }
}
