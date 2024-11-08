using AutoMapper;
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
        private readonly IMapper _mapper;
        public PaymentSvc(PaymentRepo paymentRepo, OrderRepo orderRepo, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        public SingleRsp GetPaymentByOrder(int orderCode)
        {
            var res = new SingleRsp();
            try
            {
                var order = _orderRepo.GetById(orderCode);
                if (order == null)
                {
                    res.SetError("Order not found");
                    return res;
                }
                var payment = _paymentRepo.All().Where(p => p.OrderId == orderCode).OrderByDescending(x => x.Id).ToList();
                if (payment.Count == 0)
                {
                    res.SetError("Payment not found");
                    return res;
                }
                var paymentRsp = _mapper.Map<List<PaymentRsp>>(payment);
                res.setData("data", paymentRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
