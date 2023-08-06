using BIDs_API.PaymentPayPal.Interface;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.PaymentStaffModule.Interface;
using Business_Logic.Modules.PaymentUserModule.Interface;
using Business_Logic.Modules.PaymentUserModule.Request;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System.Net;

namespace BIDs_API.PaymentPayPal
{
    public class PayPalPayment : IPayPalPayment
    {
        private readonly ICommon _common;
        private readonly string ClientAppId;
        private readonly string SecretKey;
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;
        private readonly IPaymentUserService _paymentUserService;
        private readonly IPaymentStaffService _paymentStaffService;
        private readonly IUserPaymentInformationService _userPaymentInformationService;
        public PayPalPayment(ICommon common
            , IConfiguration _configuration
            , ISessionService sessionService
            , IUserService userService
            , IPaymentUserService paymentUserService
            , IPaymentStaffService paymentStaffService
            , IUserPaymentInformationService userPaymentInformationService)
        {
            _common = common;
            ClientAppId = _configuration["PaypalSettings:ClientId"];
            SecretKey = _configuration["PaypalSettings:SecretKey"];
            _sessionService = sessionService;
            _userService = userService;
            _paymentUserService = paymentUserService;
            _paymentStaffService = paymentStaffService;
            _userPaymentInformationService = userPaymentInformationService;
        }



        public async Task<string> PaymentPaypal(Guid SesionId, Guid UserID)
        {
            double exchangeRate = await _common.Exchange();

            var environment = new SandboxEnvironment(ClientAppId, SecretKey);
            var client = new PayPalHttpClient(environment);

            var sessionList = await _sessionService.GetSessionByID(SesionId);
            var session = sessionList.ElementAt(0);
            var User = await _userService.GetUserByID(UserID);

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
                InvoiceId = UserID.ToString()
            };

            var orderCreateRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                Payer = new Payer()
                {
                    Email = User.Email,
                },
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

                    var createPaymentUser = new CreatePaymentUserRequest()
                    {
                        SessionId = SesionId,
                        UserId = UserID,
                        PayPalTransactionId = result.Id,
                        Amount = session.FinalPrice,
                        PaymentDate = DateTime.UtcNow.AddHours(7),
                        PaymentDetail = "Thanh toán sản phẩm " + session.Item.Name + " với giá là " + session.FinalPrice + ".",
                        Status = result.Status
                    };

                    var paymentUser = await _paymentUserService.AddNewPaymentUser(createPaymentUser);

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
