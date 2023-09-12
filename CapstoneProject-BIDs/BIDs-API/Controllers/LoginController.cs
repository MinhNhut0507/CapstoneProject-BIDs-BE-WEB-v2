using BIDs_API.PaymentPayPal.Interface;
using BIDs_API.SignalR;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.LoginModule.Data;
using Business_Logic.Modules.LoginModule.InterFace;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _LoginService;
        private readonly IConfiguration _configuration;
        private readonly IStaffService _staffService;
        private readonly ISessionService _sessionService;
        private readonly IHubContext<SessionHub> _hubContext;
        private readonly IPayPalPayment _payPal;
        private readonly ICommon _Common;
        public LoginController(ILoginService LoginService
            , IConfiguration configuration
            , IStaffService staffService
            , ISessionService sessionService
            , IHubContext<SessionHub> hubContext
            , IPayPalPayment payPal
            , ICommon common)
        {
            _LoginService = LoginService;
            _configuration = configuration;
            _staffService = staffService;
            _sessionService = sessionService;
            _hubContext = hubContext;
            _payPal = payPal;
            _Common = common;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var emailDev = _configuration["AdminLogin:email"];
            var passwordDev = _configuration["AdminLogin:password"];
            var roleDev = _configuration["AdminLogin:role"];
            var result = new ReturnAccountLogin();
            if (login.Email == emailDev && login.Password == passwordDev)
            {
                result.Id = Guid.NewGuid();
                result.Email = emailDev;
                result.Role = roleDev;
            }
            else 
            {
                result = _LoginService.Login(login);
            }
            if (result == null)
                return BadRequest(new LoginRespone { Successful = false, Error = "Sai tài khoản hoặc mật khẩu"});

            var jwtToken = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddHours(Convert.ToInt32(2));

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                 {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                    new Claim("Email", string.Join(",", login.Email)),
                    new Claim("role", string.Join(",", result.Role)),
                    new Claim("Role", string.Join(",", result.Role)),
                    new Claim("Id", string.Join(",", result.Id))
                }),
                Expires = expiry,
                SigningCredentials = creds
            };
            var token = jwtToken.CreateToken(tokenDescription);
            return Ok(new LoginRespone { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("decrypttoken")]
        public async Task<IActionResult> DecryptToken([FromHeader]string token)
        {
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                //Decode JWT
                var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var readToken = claims;
                var respone = readToken.Claims;
                var email = "";
                var role = "";
                Guid id = new Guid();
                foreach (var x in respone)
                {
                    switch (x.Type)
                    {
                        case "Email":
                            email = x.Value;
                            break;
                        case "Role":
                            role = x.Value;
                            break;
                        case "Id":
                            id = Guid.Parse(x.Value);
                            break;
                    }
                }
                return Ok(new
                {
                    Email = email,
                    Role = role,
                    id = id
                });
            }
            return BadRequest();
        }



        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("reset_password/{email}")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email)
        {
            try
            {
                await _LoginService.ResetPassword(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("payment_complete")]
        public async Task<IActionResult> CreateOrderCompletePaypal([FromQuery] Guid sessionId, [FromQuery] Guid payerId, [FromQuery] string urlSuccess, [FromQuery] string urlFail)
        {
            try
            {
                var response = await _payPal.PaymentPaypalComplete(sessionId, payerId, urlSuccess, urlFail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("payment_joinning")]
        public async Task<IActionResult> CreateOrderJoiningPaypal([FromQuery] Guid sessionId, [FromQuery] Guid payerId, [FromQuery] string urlSuccess, [FromQuery] string urlFail)
        {
            try
            {
                var response = await _payPal.PaymentPaypalJoining(sessionId, payerId, urlSuccess, urlFail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("test_return_deposit")]
        public async Task<IActionResult> TestReturnDeposit([FromQuery] Guid sessionId)
        {
            try
            {
                var response = await _payPal.PaymentStaffReturnDeposit(sessionId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_payment_user")]
        public async Task<IActionResult> ReportPaymentUser([FromQuery] Guid userId, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var response = await _Common.ReportPaymentUser(userId, start, end);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_payment_user_total")]
        public async Task<IActionResult> ReportPaymentUserTotal([FromQuery] Guid userId)
        {
            try
            {
                var response = await _Common.ReportPaymentUserToTal(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_payment_total")]
        public async Task<IActionResult> ReportPaymentTotal()
        {
            try
            {
                var response = await _Common.ReportPaymentToTal();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_session_total")]
        public async Task<IActionResult> ReportSessionTotal()
        {
            try
            {
                var response = await _Common.ReportSessionTotal();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_session_total_by_date")]
        public async Task<IActionResult> ReportSessionTotalByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _Common.ReportSessionTotalByDate(startDate, endDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_category")]
        public async Task<IActionResult> ReportCategory([FromQuery] Guid CategoryId,[FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _Common.ReportCategoryDetail(CategoryId,startDate, endDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("report_user")]
        public async Task<IActionResult> ReportUser([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _Common.ReportUser(startDate, endDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
