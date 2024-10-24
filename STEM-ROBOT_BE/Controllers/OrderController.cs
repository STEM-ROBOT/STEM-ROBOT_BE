using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderSvc _paymentSvc;
        private readonly PayOS _payOS;
        public OrderController(OrderSvc paymentSvc)
        {
            _paymentSvc = paymentSvc;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderReq request)
        {
            var result = await _paymentSvc.CreateOrder(request);

            return Ok(result.Data);
            //return Redirect(result.Data.ToString());
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute]int id)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(id);
                return Ok(paymentLinkInformation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/

        [HttpGet("success/{orderCode}")]
        public IActionResult Success()
        {
            return Ok("Payment success");
        }

        [HttpGet("cancel/{orderCode}")]
        public async Task<IActionResult> Cancel(int orderCode)
        {
            var result = await _paymentSvc.CancelOrder(orderCode);
            return Redirect("https://www.youtube.com/");
        }

        [HttpGet("get-revenue")]
        public IActionResult GetRevenue()
        {
            var res = _paymentSvc.GetRevenue();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }
    }
}
