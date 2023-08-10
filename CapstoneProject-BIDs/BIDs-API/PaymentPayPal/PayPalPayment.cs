using BIDs_API.PaymentPayPal.Interface;
using BIDs_API.SignalR;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.PaymentStaffModule.Interface;
using Business_Logic.Modules.PaymentStaffModule.Request;
using Business_Logic.Modules.PaymentUserModule.Interface;
using Business_Logic.Modules.PaymentUserModule.Request;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Polly.Caching;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BIDs_API.PaymentPayPal
{
    public class PayPalPayment : IPayPalPayment
    {
        private readonly ICommon _common;
        private readonly string ClientAppId;
        private readonly string SecretKey;
        private readonly string EmailBIDs;
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;
        private readonly IPaymentUserService _paymentUserService;
        private readonly IPaymentStaffService _paymentStaffService;
        private readonly IUserPaymentInformationService _userPaymentInformationService;
        private readonly IHubContext<PaymentUserHub> _userHubContext;
        private readonly IHubContext<PaymentStaffHub> _staffHubContext;
        private readonly IHubContext<SessionHub> _sessionHubContext;

        public PayPalPayment(ICommon common
            , IConfiguration _configuration
            , ISessionService sessionService
            , IUserService userService
            , IPaymentUserService paymentUserService
            , IPaymentStaffService paymentStaffService
            , IUserPaymentInformationService userPaymentInformationService
            , IHubContext<PaymentUserHub> userHubContext
            , IHubContext<PaymentStaffHub> staffHubContext
            , IHubContext<SessionHub> sessionHubContext)
        {
            _common = common;
            ClientAppId = _configuration["PaypalSettings:ClientId"];
            SecretKey = _configuration["PaypalSettings:SecretKey"];
            EmailBIDs = _configuration["PaypalSettings:Email"];
            _sessionService = sessionService;
            _userService = userService;
            _paymentUserService = paymentUserService;
            _paymentStaffService = paymentStaffService;
            _userPaymentInformationService = userPaymentInformationService;
            _userHubContext = userHubContext;
            _staffHubContext = staffHubContext;
            _sessionHubContext = sessionHubContext;
        }



        public async Task<string> PaymentPaypalComplete(Guid SesionId, Guid UserID, string urlSuccess, string urlFail)
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

            var description = "Thanh toán sản phẩm đấu giá được đấu giá thông qua hệ thống đấu giá trực tuyến BIDs.";

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
                    ReturnUrl = urlSuccess,
                    CancelUrl = urlFail
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
                    await _userHubContext.Clients.All.SendAsync("ReceivePaymentUserAdd", paymentUser);

                    var approvalLink = result.Links.FirstOrDefault(link => link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase));

                    if (approvalLink != null)
                    {
                        var approvalUrl = approvalLink.Href;
                        return approvalUrl;
                    }
                    else
                    {
                        return urlFail;
                    }
                }
                else
                {
                    return urlFail;
                }
            }
            catch (Exception ex)
            {
                return urlFail;
            }
        }

        public async Task<string> PaymentStaffToWinner(Guid sessionId, Guid userId, Guid staffId, string urlSuccess, string urlFail)
        {
            double exchangeRate = await _common.Exchange();
            var sessionList = await _sessionService.GetSessionByID(sessionId);
            var session = sessionList.ElementAt(0);
            var User = await _userService.GetUserByID(userId);
            var PaypalUser = await _userPaymentInformationService.GetUserPaymentInformationByUser(userId);


            var EmailPaypalUser = PaypalUser.PayPalAccount;
            var Total = Math.Round(session.FinalPrice / exchangeRate, 2);

            using (HttpClient client = new HttpClient())
            {
                // Xây dựng chuỗi xác thực Basic Authorization
                string authString = $"{ClientAppId}:{SecretKey}";
                byte[] authBytes = Encoding.ASCII.GetBytes(authString);
                string base64Auth = Convert.ToBase64String(authBytes);

                // Đặt header Authorization cho yêu cầu HTTP
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);

                // Tạo thanh toán hoàn trả
                var payload = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                    {
                        new
                        {
                            amount = new
                            {
                                currency_code = "USD",
                                value = Total.ToString()
                            }
                        }
                    },
                    payer = new
                    {
                        email_address = EmailBIDs // Email của người chuyển tiền
                    },
                    payee = new
                    {
                        email_address = EmailPaypalUser // Email của người nhận tiền
                    }
                };

                var payloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var orderData = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderResponse>(responseContent);
                string orderId = orderData.id;

                // Xác nhận thanh toán hoàn trả
                var confirmUrl = $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/confirm-payment-source";

                var payloadConfirm = new
                {
                    payment_source = new
                    {
                        paypal = new
                        {
                            name = new
                            {
                                given_name = User.Name,
                                surname = User.Email
                            },
                            email_address = EmailPaypalUser,
                            experience_context = new
                            {
                                payment_method_preference = "IMMEDIATE_PAYMENT_REQUIRED",
                                brand_name = "EXAMPLE INC",
                                locale = "en-US",
                                landing_page = "LOGIN",
                                shipping_preference = "SET_PROVIDED_ADDRESS",
                                user_action = "PAY_NOW",
                                return_url = urlSuccess,
                                cancel_url = urlFail
                            }
                        }
                    }
                };

                var payloadJsonConfirm = Newtonsoft.Json.JsonConvert.SerializeObject(payloadConfirm);
                var contentConfirm = new StringContent(payloadJsonConfirm, Encoding.UTF8, "application/json");

                var responseConfirm = await client.PostAsync(confirmUrl, contentConfirm);
                var responseContentConfirm = await responseConfirm.Content.ReadAsStringAsync();
                var responseStatus = responseConfirm.ReasonPhrase;

                // Xử lý phản hồi từ PayPal và trả về status xác nhận nguồn thanh toán
                if (responseConfirm.IsSuccessStatusCode)
                {
                    var createPaymentStaff = new CreatePaymentStaffRequest()
                    {
                        SessionId = sessionId,
                        StaffId = staffId,
                        UserPaymentInformationId = PaypalUser.Id,
                        PayPalTransactionId = orderId,
                        Amount = 20000000,
                        PaymentDate = DateTime.UtcNow.AddHours(7),
                        PaymentDetail = "Thanh toán cho sản phẩm đấu giá " + session.Item.Name + ".",
                        Status = responseStatus
                    };

                    var paymentStaff = await _paymentStaffService.AddNewPaymentStaff(createPaymentStaff);

                    await _staffHubContext.Clients.All.SendAsync("ReceivePaymentStaffAdd", paymentStaff);

                    return responseStatus;
                }
                else
                {
                    return responseStatus;
                }
            }
        }

        public async Task<string> PaymentPaypalJoining(Guid SesionId, Guid UserID, string urlSuccess, string urlFail)
        {
            double exchangeRate = await _common.Exchange();

            var environment = new SandboxEnvironment(ClientAppId, SecretKey);
            var client = new PayPalHttpClient(environment);

            var sessionList = await _sessionService.GetSessionByID(SesionId);
            var session = sessionList.ElementAt(0);
            var User = await _userService.GetUserByID(UserID);
            var Deposit = session.Item.FirstPrice * session.Fee.DepositFee;
            var JoiningFee = session.Item.FirstPrice * session.Fee.ParticipationFee;
            if (JoiningFee > 200000)
            {
                JoiningFee = 200000;
            }
            if (JoiningFee < 10000)
            {
                JoiningFee = 10000;
            }

            var total = Math.Round((Deposit + JoiningFee) / exchangeRate, 2);

            var itemList = new List<Item>()
            {
                new Item()
                {
                    Name = session.Item.Name,
                    UnitAmount = new Money()
                    {
                        CurrencyCode = "USD",
                        Value = total.ToString()
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

            var description = "Phí tham gia và phí đạt cọc của phiên đấu giá trong hệ thống BIDs";

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
                    ReturnUrl = urlSuccess,
                    CancelUrl = urlFail
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
                        PaymentDetail = "Thanh toán phí tham gia và phí đặt cọc của sản phẩm " + session.Item.Name + " với giá là " + total + ".",
                        Status = result.Status
                    };

                    var paymentUser = await _paymentUserService.AddNewPaymentUser(createPaymentUser);
                    await _userHubContext.Clients.All.SendAsync("ReceivePaymentUserAdd", paymentUser);

                    var approvalLink = result.Links.FirstOrDefault(link => link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase));

                    if (approvalLink != null)
                    {
                        var approvalUrl = approvalLink.Href;
                        return approvalUrl;
                    }
                    else
                    {
                        return urlFail;
                    }
                }
                else
                {
                    return urlFail;
                }
            }
            catch (Exception ex)
            {
                return urlFail;
            }
        }

        public async Task<string> CheckAndUpdateOrderComplete(string orderId)
        {
            string apiUrl = $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}";

            using (HttpClient client = new HttpClient())
            {
                // Xây dựng chuỗi xác thực Basic Authorization
                string authString = $"{ClientAppId}:{SecretKey}";
                byte[] authBytes = Encoding.ASCII.GetBytes(authString);
                string base64Auth = Convert.ToBase64String(authBytes);

                // Đặt header Authorization cho yêu cầu HTTP
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);

                HttpResponseMessage response = await client.GetAsync(apiUrl);



                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(responseBody);

                    string status = jsonObject["status"].ToString();
                    var updatePaymentUser = new UpdatePaymentUserStatusRequest()
                    {
                        TransactionId = orderId,
                        Status = status
                    };

                    var paymentUser = await _paymentUserService.UpdatePaymentUser(updatePaymentUser);
                    await _userHubContext.Clients.All.SendAsync("ReceivePaymentUserUpdate", paymentUser);

                    var updateStatusSession = new UpdateSessionStatusRequest()
                    {
                        SessionID = paymentUser.SessionId
                    };

                    var session = await _sessionService.UpdateSessionStatusComplete(updateStatusSession);
                    await _sessionHubContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);

                    return paymentUser.Status;
                }
                else
                {
                    Console.WriteLine($"Lỗi khi truy vấn đơn đặt hàng: {response.StatusCode}");
                    string errorBody = await response.Content.ReadAsStringAsync();
                    return errorBody;
                }
            }
        }

        public async Task<string> PaymentStaffReturnDeposit(Guid sessionId, Guid userId, Guid staffId, string urlSuccess, string urlFail)
        {
            double exchangeRate = await _common.Exchange();
            var sessionList = await _sessionService.GetSessionByID(sessionId);
            var session = sessionList.ElementAt(0);
            var User = await _userService.GetUserByID(userId);
            var PaypalUser = await _userPaymentInformationService.GetUserPaymentInformationByUser(userId);
            var Deposit = session.Item.FirstPrice * session.Fee.DepositFee;

            var EmailPaypalUser = PaypalUser.PayPalAccount;
            var Total = Math.Round( 20000000 / exchangeRate, 2);

            using (HttpClient client = new HttpClient())
            {
                // Xây dựng chuỗi xác thực Basic Authorization
                string authString = $"{ClientAppId}:{SecretKey}";
                byte[] authBytes = Encoding.ASCII.GetBytes(authString);
                string base64Auth = Convert.ToBase64String(authBytes);

                // Đặt header Authorization cho yêu cầu HTTP
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);

                // Tạo thanh toán hoàn trả
                var payload = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                    {
                        new
                        {
                            amount = new
                            {
                                currency_code = "USD",
                                value = Total.ToString()
                            }
                        }
                    },
                    payer = new
                    {
                        email_address = EmailBIDs // Email của người chuyển tiền
                    },
                    payee = new
                    {
                        email_address = EmailPaypalUser // Email của người nhận tiền
                    }
                };

                var payloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var orderData = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderResponse>(responseContent);
                string orderId = orderData.id;

                // Xác nhận thanh toán hoàn trả
                var confirmUrl = $"https://api-m.sandbox.paypal.com/v2/checkout/orders/{orderId}/confirm-payment-source";

                var payloadConfirm = new
                {
                    payment_source = new
                    {
                        paypal = new
                        {
                            name = new
                            {
                                given_name = User.Name,
                                surname = User.Email
                            },
                            email_address = EmailPaypalUser,
                            experience_context = new
                            {
                                payment_method_preference = "IMMEDIATE_PAYMENT_REQUIRED",
                                brand_name = "EXAMPLE INC",
                                locale = "en-US",
                                landing_page = "LOGIN",
                                shipping_preference = "SET_PROVIDED_ADDRESS",
                                user_action = "PAY_NOW",
                                return_url = urlSuccess,
                                cancel_url = urlFail
                            }
                        }
                    }
                };

                var payloadJsonConfirm = Newtonsoft.Json.JsonConvert.SerializeObject(payloadConfirm);
                var contentConfirm = new StringContent(payloadJsonConfirm, Encoding.UTF8, "application/json");

                var responseConfirm = await client.PostAsync(confirmUrl, contentConfirm);
                var responseContentConfirm = await responseConfirm.Content.ReadAsStringAsync();
                var responseStatus = responseConfirm.ReasonPhrase;

                // Xử lý phản hồi từ PayPal và trả về status xác nhận nguồn thanh toán
                if (responseConfirm.IsSuccessStatusCode)
                {
                    var createPaymentStaff = new CreatePaymentStaffRequest()
                    {
                        SessionId = sessionId,
                        StaffId = staffId,
                        UserPaymentInformationId = PaypalUser.Id,
                        PayPalTransactionId = orderId,
                        Amount = 20000000,
                        PaymentDate = DateTime.UtcNow.AddHours(7),
                        PaymentDetail = "Hoàn trả phí đặt cọc cho sản phẩm đấu giá " + session.Item.Name + ".",
                        Status = responseStatus
                    };

                    var paymentStaff = await _paymentStaffService.AddNewPaymentStaff(createPaymentStaff);

                    await _staffHubContext.Clients.All.SendAsync("ReceivePaymentStaffAdd", paymentStaff);

                    return responseStatus;
                }
                else
                {
                    return responseStatus; 
                }
            }
        }

        public class OrderResponse
        {
            public string id { get; set; }
            // Các thuộc tính khác tùy theo cấu trúc JSON phản hồi từ PayPal
        }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }
    }
}
