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
        private readonly OrderSvc _orderSvc;
        private readonly PayOS _payOS;
        public OrderController(OrderSvc orderSvc)
        {
            _orderSvc = orderSvc;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderReq request)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (user == null) return BadRequest("Please login ");
            int userId = int.Parse(user.Value);
            var result = await _orderSvc.CreateOrder(userId,request);
            return Ok(result.Data);
            //return Redirect(result.Data.ToString());
        }



        [HttpGet("success/{orderCode}")]
        public async Task<IActionResult> Success(int orderCode)
        {
            var result = await _orderSvc.SuccessOrder(orderCode);
            return Redirect("http://localhost:5173/payment/success");
        }

        [HttpGet("cancel/{orderCode}")]
        public IActionResult Cancel()
        {      
            return Redirect("http://localhost:5173/payment/fail");
        }

        [HttpGet("total-revenue")]
        public IActionResult GetRevenue()
        {
            var res = _orderSvc.GetRevenue();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet("revenue-by-time")]
        public IActionResult GetrevenueByTime()
        {
            var res = _orderSvc.GetRevenueByTime();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }

        [HttpGet()]
        public IActionResult GetOrder()
        {
            var res = _orderSvc.GetOrders();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var res = _orderSvc.GetOrderById(id);
            if (res.Success)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.Message);
        }
        
    }
}
