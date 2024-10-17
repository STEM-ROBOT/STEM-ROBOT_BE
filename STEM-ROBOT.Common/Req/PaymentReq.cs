using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class PaymentReq
    {
        [Required(ErrorMessage = "AccountId is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "PackageId is required")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}
