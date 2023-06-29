using Business_Logic.Modules.LoginModule.InterFace;
using Business_Logic.Modules.LoginModule.Request;
using Data_Access.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using Business_Logic.Modules.UserModule.Response;
using Microsoft.AspNetCore.SignalR;
using BIDs_API.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _LoginService;
        private readonly IConfiguration _configuration;
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;
        private readonly IHubContext<UserHub> _hubContext;
        public LoginController(ILoginService LoginService
            , IConfiguration configuration
            , IStaffService staffService
            , IUserService userService
            , IHubContext<UserHub> hubContext)
        {
            _LoginService = LoginService;
            _configuration = configuration;
            _staffService = staffService;
            _userService = userService;
            _hubContext = hubContext;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var result = _LoginService.Login(login);

            if(result == null)
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
                    new Claim("Role", string.Join(",", result.Role))

                }),
                Expires = expiry,
                SigningCredentials = creds
            };
            var token = jwtToken.CreateToken(tokenDescription);
            return Ok(new LoginRespone { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("decrypttoken")]
        public async Task<IActionResult> DecryptTokenForStaff([FromHeader]string token)
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
                    }
                }
                return Ok(new
                {
                    Email = email,
                    Role = role
                });
            }
            return BadRequest();
        }



        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("reset-password/{email}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string email)
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

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("send-email/{email}")]
        public async Task<IActionResult> sendemail([FromRoute] string email)
        {
            try
            {
                await _LoginService.sendemail(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-account")]
        public async Task<ActionResult<UserResponseUser>> PostUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _LoginService.CreateAccount(createUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserAdd", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
