using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TransactionRsp
    {
        public int Id { get; set; }

        public string? PackageName { get; set; }

        public string? Status { get; set; }

        public DateTime? OrderDate { get; set; }


        public decimal? Amount { get; set; }

      
    }
}
