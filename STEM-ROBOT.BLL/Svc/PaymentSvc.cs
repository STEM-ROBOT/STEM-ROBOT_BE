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
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly PackageRepo _packageRepo;

        public PaymentSvc(AccountRepo accountRepo, IMapper mapper, HttpClient httpClient, IConfiguration configuration, PayOS payOS)
        {
            _accountRepo = accountRepo;
            _payOS = payOS;
            _mapper = mapper;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /*public async Task<string> CreateOrder(int accountId, PaymentReq request)
        {

            var user = await _accountRepo.GetAccountById(accountId);
            if (user == null) throw new ArgumentException("No user");

            long orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            string orderCodeas = orderCode.ToString();
            var order = new PakageAccount
            {
                PackageId = request.PackageId,
                AccountId = accountId,
                PurchaseDate = DateTime.Now,
                Status = "PENDING",

            };
            _unitOfWork.OrderRepository.Insert(order);
            await _unitOfWork.SaveChangeAsync();
            decimal totalPrice = 0; // Move total price initialization here
            List<ItemData> items = new List<ItemData>();
            foreach (var item1 in request.orderRequestNews)
            {
                var quantity = item1.Quantity;
                var productID = item1.ProductID;
                var product = await _unitOfWork.ProductRepository.GetByIDAsync(productID);
                if (product == null) throw new ArgumentException("Product not found");
                if (product.Inventory < 0) throw new ArgumentException("Out of stock");


                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = productID,
                    Quantity = quantity,
                    Price = product.Price,
                    PriceSale = product.PriceSale,
                    Weight = item1.Weight

                };


                var price = Convert.ToInt32(product.PriceSale);
                totalPrice += quantity * product.PriceSale; // Accumulate total price for each item
                product.Inventory -= quantity;
                product.Purchases += quantity;
                ItemData item = new ItemData(product.ProductName, quantity, price);
                items.Add(item);
                _unitOfWork.OrderDetailRepository.Insert(orderDetail);
            }

            order.TotalPrice = totalPrice; // Set total price after calculating


            await _unitOfWork.SaveChangeAsync();


            if (request.PaymentMethod == "CASH")
            {
                order.Status = "2";
                await _unitOfWork.SaveChangeAsync();
                return order.OrderId;
            }
            else if (request.PaymentMethod == "PAYOS")
            {
                var paylink = await CreatePayos(items, orderCode, (int)totalPrice);
                order.PayUrl = paylink;
                order.PaymentStatus = "PENDING";
                await _unitOfWork.SaveChangeAsync();
                return paylink;
            }
            return null;
        }*/

    }
}
