using AutoMapper;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class OrderSvc
    {
        private readonly PayOS _payOS;
        private readonly AccountRepo _accountRepo;
        private readonly IMapper _mapper;
        private readonly PackageRepo _packageRepo;
        private readonly OrderRepo _orderRepo;
        private readonly PaymentRepo _paymentRepo;

        public OrderSvc(PayOS payOS, IMapper mapper, AccountRepo accountRepo, PackageRepo packageRepo, OrderRepo packageAccountRepo, PaymentRepo paymentRepo)
        {
            _payOS = payOS;
            _mapper = mapper;
            _accountRepo = accountRepo;
            _packageRepo = packageRepo;
            _orderRepo = packageAccountRepo;
            _paymentRepo = paymentRepo;
        }
        public DateTime ConvertToVietnamTime(DateTime serverTime)
        {
            // Lấy thông tin múi giờ Việt Nam (UTC+7)
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển đổi từ thời gian server sang thời gian Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime.ToUniversalTime(), vietnamTimeZone);

            return vietnamTime;
        }
        public async Task<SingleRsp> CreateOrder(OrderReq request)
        {
            var res = new SingleRsp();
            try
            {
                long orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                var account = _accountRepo.GetById(request.AccountId);
                if (account == null)
                {
                    res.SetError("Account not found");
                    return res;
                }
                var package = _packageRepo.GetById(request.PackageId);
                if (package == null)
                {
                    res.SetError("Package not found");
                    return res;
                }
                var order = new Order
                {
                    Id = (int)orderCode,
                    AccountId = request.AccountId,
                    PackageId = request.PackageId,
                    Status = "Pending",
                    OrderDate = ConvertToVietnamTime(DateTime.Now),
                    Amount = package.Price,
                    LinkPayAgain = $"https://localhost:7283/api/payments/cancel/{orderCode}"

                };
                _orderRepo.Add(order);
                List<ItemData> items = new List<ItemData>
                {
                    new ItemData(package.Name, 1, (int)package.Price)
                };
                var payLink = await CreatePayos(items, orderCode, (int)package.Price);
                res.setData("data", payLink);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public async Task<string> CreatePayos(List<ItemData> items, long orderCode, int totalPay)
        {

            PaymentData paymentData = new PaymentData(orderCode, totalPay, "Thanh toan don hang", items, $"https://localhost:7283/api/orders/cancel/{orderCode}", $"https://localhost:7283/api/order/success/{orderCode}");


            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return createPayment.checkoutUrl;
        }
        public async Task<SingleRsp> CancelOrder(int orderCode)
        {
            var res = new SingleRsp();
            try
            {
                var order = _orderRepo.GetById(orderCode);
                var account = _accountRepo.GetById(order.AccountId);
                var package = _packageRepo.GetById(order.PackageId);
                account.MaxTournatment += package.MaxTournament;
                _accountRepo.Update(account);
                order.Status = "Success";
                _orderRepo.Update(order);
                var payment = new Payment
                {
                    OrderId = orderCode,
                    Amount = package.Price,
                    PurchaseDate = ConvertToVietnamTime(DateTime.Now),
                    Status = "Success",
                };
                _paymentRepo.Add(payment);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetRevenue()
        {
            var res = new SingleRsp();
            try
            {
                var totalRevenue = _paymentRepo.All(p => p.Status == "Success").Sum(p => p.Amount);
                res.setData("data", totalRevenue);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp GetRevenueByTime()
        {
            var res = new SingleRsp();
            try
            {
                var query = _paymentRepo.All(p => p.Status == "Success");

                var monthlyRevenue = query
                    .GroupBy(p => new { p.PurchaseDate.Value.Year, p.PurchaseDate.Value.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Revenue = g.Sum(p => p.Amount)
                    })
                    .OrderBy(result => result.Year)
                    .ThenBy(result => result.Month)
                    .ToList();

                // Đưa dữ liệu vào kết quả trả về
                res.setData("data", monthlyRevenue);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        public MutipleRsp GetOrders()
        {
            var res = new MutipleRsp();
            try
            {
                var orders = _orderRepo.All();
                res.SetData("data", orders);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp GetOrderById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var order = _orderRepo.GetById(id);
                res.setData("data", order);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

    }
}
