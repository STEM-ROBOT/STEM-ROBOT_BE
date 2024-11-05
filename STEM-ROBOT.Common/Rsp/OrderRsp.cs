using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class OrderRsp
    {
        public int Id { get; set; }
        public string? Status { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? LinkPayAgain { get; set; }

        public decimal? Amount { get; set; }

        public string MethodOrder { get; } = "PayOS";

        public string nameUser { get; set; }
        public string Image { get; set; }

    }
}
