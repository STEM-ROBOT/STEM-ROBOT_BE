using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class PaymentSvc
    {
        private readonly PaymentRepo _paymentRepo;
        private readonly OrderRepo _orderRepo;
        public PaymentSvc(PaymentRepo paymentRepo, OrderRepo orderRepo)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
        }
        public SingleRsp GetPaymentByOrder(int orderId)
        {
            var res = new SingleRsp();
            try
            {
                var order = _orderRepo.GetById(orderId);
                if (order == null)
                {
                    res.SetError("Order not found");
                    return res;
                }
                var payment = _paymentRepo.All().Where(p => p.OrderId == orderId).OrderByDescending(x => x.Id).ToList();
                if (payment.Count == 0)
                {
                    res.SetError("Payment not found");
                    return res;
                }
                res.setData("200", payment);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
