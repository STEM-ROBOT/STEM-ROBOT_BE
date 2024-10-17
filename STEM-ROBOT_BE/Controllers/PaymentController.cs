using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;

namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        /*private readonly PaymentSvc _paymentSvc;
        public PaymentController(PaymentSvc paymentSvc)
        {
            _paymentSvc = paymentSvc;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int userID, PaymentReq request)
        {
            var result = await _paymentSvc.CreateOrder(userID, request);
            return Ok(result);
        }*/
    }
}
