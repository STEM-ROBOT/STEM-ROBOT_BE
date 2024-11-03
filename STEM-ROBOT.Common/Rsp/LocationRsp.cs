using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class LocationRsp
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Status { get; set; }
        public int CompetitionId { get; set; }
    }
}
