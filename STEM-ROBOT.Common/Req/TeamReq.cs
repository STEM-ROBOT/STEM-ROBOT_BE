using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TeamReq
    {
        public int? CompetitionId { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

       

        public string? Image = "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png";
    }
}
