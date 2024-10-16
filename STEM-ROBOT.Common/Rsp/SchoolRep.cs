using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public  class SchoolRep
    {
        public int Id { get; set; }

        public string? SchoolName { get; set; }

        public string? SchoolCode { get; set; }

        public string? Address { get; set; }

        public string? Area { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? ProvinceCode { get; set; }

        public string? DistrictCode { get; set; }
    }
}
