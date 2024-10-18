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
    public class PaymentSvc
    {
        private readonly PayOS _payOS;
        private readonly AccountRepo _accountRepo;
        private readonly IMapper _mapper;
        private readonly PackageRepo _packageRepo;
        private readonly PackageAccountRepo _packageAccountRepo;

        public PaymentSvc(PayOS payOS, IMapper mapper, AccountRepo accountRepo, PackageRepo packageRepo, PackageAccountRepo packageAccountRepo)
        {
            _payOS = payOS;
            _mapper = mapper;
            _accountRepo = accountRepo;
            _packageRepo = packageRepo;
            _packageAccountRepo = packageAccountRepo;
          
        }
        public async Task<SingleRsp> CreateOrder(PaymentReq request)
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
                var packageAccount = new PakageAccount
                {
                    Id = (int)orderCode,
                    AccountId = request.AccountId,
                    PackageId = request.PackageId,
                    PurchaseDate = DateTime.Now,
                    Status = "Pending"
                };
                _packageAccountRepo.Add(packageAccount);
                List<ItemData> items = new List<ItemData>
                {
                    new ItemData(package.Name, 1, (int)package.Price)
                };
                var payLink = await CreatePayos(items, orderCode, (int)package.Price);
                res.setData("200", payLink);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public async Task<string> CreatePayos(List<ItemData> items, long orderCode, int totalPay)
        {
#if DEBUG
            PaymentData paymentData = new PaymentData(orderCode, totalPay, "Thanh toan don hang", items, $"https://localhost:7283/api/payments/cancel/{orderCode}", $"https://localhost:7283/api/payments/success/{orderCode}");
#else
           
#endif
            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
            
            return createPayment.checkoutUrl;
        }
        public async Task<SingleRsp> CancelOrder(int orderCode)
        {
            var res = new SingleRsp();
            var packageAccount = _packageAccountRepo.GetById(orderCode);
            var account = _accountRepo.GetById(packageAccount.AccountId);
            var package = _packageRepo.GetById(packageAccount.PackageId);
            account.MaxTournatment += package.MaxTournament;
            _accountRepo.Update(account);
            return res; 
        }

    }
}
