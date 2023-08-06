using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.SessionModule.Interface;
using BIDs_API.PaymentPayPal.Interface;

namespace BIDs_API.PaymentPayPal
{
    public class PayPalPayment : IPayPalPayment
    {
        private readonly ICommon _common;
        private readonly string ClientAppId;
        private readonly string SecretKey;
        private readonly ISessionService _sessionService;
        public PayPalPayment(ICommon common
            , IConfiguration _configuration
            , ISessionService sessionService)
        {
            _common = common;
            ClientAppId = _configuration["PaypalSettings:ClientId"];
            SecretKey = _configuration["PaypalSettings:SecretKey"];
            _sessionService = sessionService;
        }



        public async Task<string> PaymentPaypal(Guid SesionId, Guid payerId)
        {
            double exchangeRate = await _common.Exchange();

            var environment = new SandboxEnvironment(ClientAppId, SecretKey);
            var client = new PayPalHttpClient(environment);

            var sessionList = await _sessionService.GetSessionByID(SesionId);
            var session = sessionList.ElementAt(0);

            var itemList = new List<Item>()
            {
                new Item()
                {
                    Name = session.Item.Name,
                    UnitAmount = new Money()
                    {
                        CurrencyCode = "USD",
                        Value = Math.Round(session.FinalPrice / exchangeRate, 2).ToString()
                    },
                    Description = session.Item.DescriptionDetail,
                    Quantity = session.Item.Quantity.ToString(),
                    Sku = "sku",
                    Tax = new Money()
                    {
                        CurrencyCode = "USD",
                        Value = "0"
                    }
                }
            };

            var total = Math.Round(session.FinalPrice / exchangeRate, 2);

            var amountDetails = new AmountWithBreakdown()
            {
                CurrencyCode = "USD",
                Value = total.ToString(),
                AmountBreakdown = new AmountBreakdown()
                {
                    ItemTotal = new Money()
                    {
                        CurrencyCode = "USD",
                        Value = total.ToString()
                    }
                }
            };

            var description = "Sản phẩm " + session.Item.Name + " được đấu giá thông qua hệ thống đấu giá trực tuyến BIDs.";

            var purchaseUnitRequest = new PurchaseUnitRequest()
            {
                AmountWithBreakdown = amountDetails,
                Items = itemList,
                Description = description,
                InvoiceId = session.Id.ToString()
            };

            var orderCreateRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    purchaseUnitRequest
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = "https://www.google.com/search?q=thanh+cong",
                    CancelUrl = "https://www.google.com/search?q=that+bai"
                }
            };


            try
            {
                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(orderCreateRequest);
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;

                if (statusCode == HttpStatusCode.Created)
                {
                    var result = response.Result<Order>();
                    var approvalLink = result.Links.FirstOrDefault(link => link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase));

                    if (approvalLink != null)
                    {
                        var approvalUrl = approvalLink.Href;
                        return approvalUrl;
                    }
                    else
                    {
                        return "https://www.google.com/search?q=that+bai";
                    }
                }
                else
                {
                    return "https://www.google.com/search?q=that+bai";
                }
            }
            catch (Exception ex)
            {
                return "https://www.google.com/search?q=that+bai";
            }
        }

    }
}
