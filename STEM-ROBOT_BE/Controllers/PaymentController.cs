using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentSvc _paymentSvc;
        public PaymentController(PaymentSvc paymentSvc)
        {
            _paymentSvc = paymentSvc;
        }

        [HttpGet("{orderCode}")]
        public IActionResult GetPaymentByOrderId(int orderCode)
        {
            var res = _paymentSvc.GetPaymentByOrder(orderCode);
            if (!res.Success)
            {
                return StatusCode(500, res.Message);
            }
            return Ok(res.Data);
        }
    }
}