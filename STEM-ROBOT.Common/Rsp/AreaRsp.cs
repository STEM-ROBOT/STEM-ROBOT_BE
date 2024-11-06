using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class AreaRsp
    {
        public int Id { get; set; }

        public string? Name { get; set; }

    }
    public class ProvinceRsp
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? AreaId { get; set; }

        public string? ProvinceCode { get; set; }

    }
    public class DistrictRsp
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? ProvinceId { get; set; }

        public string? DistrictCode { get; set; }

        public string? ProvinceCode { get; set; }
    }

    public class ListSchoolRsp
    {
        public int Id { get; set; }

        public string? SchoolName { get; set; }

        public string? SchoolCode { get; set; }

        public string? Address { get; set; }

        public string? ProvinceCode { get; set; }

        public string? DistrictCode { get; set; }

        public int? DistrictId { get; set; }

    }

}
