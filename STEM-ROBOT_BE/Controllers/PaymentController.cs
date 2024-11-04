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

        [HttpGet("{orderId}")]
        public IActionResult GetPaymentByOrderId(int orderId)
        {
            var res = _paymentSvc.GetPaymentByOrder(orderId);
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }
    }
}
