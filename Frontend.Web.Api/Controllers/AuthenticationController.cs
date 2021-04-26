using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using RST.Shared;
using RST.Shared.Enums;
using ModelUser;
using ServiceUsers;
using ServiceSMS;
using System.Collections;
using RestSharp;

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        //private readonly IListsLookupRepository _lookupRepository;
        //private IWebHostEnvironment _environment;
        private ILogger<AuthenticationController> _logger;
        private ISmsService _smsService;
        //public AuthenticationController(IConfiguration configuration, IListsLookupRepository lookupRepository, IWebHostEnvironment environment, ILogger<AuthenticationController> logger)
        public AuthenticationController(IConfiguration configuration,
            IUserService userservice,
            ILogger<AuthenticationController> logger,
            ISmsService smsService)
        {
            _configuration = configuration;
            _userService = userservice;
            //_lookupRepository = lookupRepository;
            //_environment = environment;
            _logger = logger;
            _smsService = smsService;
        }


        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate(string userRef, string password)
        {
            password = System.Net.WebUtility.HtmlDecode(password);
            var encryptPassword = ServiceHelper.Helper.Encrypt(password);

            //var DeCrypytPassword = ServiceHelper.Helper.Decrypt(EncryptPassword);

            var result = new OperationResponse<Users>();
            try
            {
                if (_userService.IsValidUserAndPasswordCombination(userRef, encryptPassword) == "Y")
                {
                    var userInfo = _userService.GetUserInfo(userRef, encryptPassword);
                    if (userInfo != null)
                    {
                        //result.Data = GenerateToken(userInfo);
                        result.Data = userInfo;
                    }
                    else
                    {
                        result.State = ResponseState.Error;
                        result.Messages.Add("Error, user is not authorized to access the system");
                    }
                }
                else
                {
                    result.State = ResponseState.Error;
                    result.Messages.Add("Error, username or password are invalid");
                }
            }
            catch (Exception exception)
            {
                result.State = ResponseState.Error;
                result.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GenerateOTP==>" + exception.StackTrace, userRef, password);
            }
            return new JsonResult(result);
        }

        //private string GenerateToken(UserLookup user)
        private string GenerateToken(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.RowId.ToString()),
                new Claim(ClaimTypes.Name, user.Firstname + " "+ user.Lastname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecurityKey"))),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("GenerateOTP")]
        public IActionResult GenerateOTP(string MobileNumber)
        {
            var response = new OperationResponse<string>();
            var ValidationResponse = _userService.ValidateEmailandMobile("", MobileNumber);
            if (ValidationResponse != "")
            {
                Random generator = new Random();
                string OTP = generator.Next(0, 999999).ToString("D6");
                try
                {
                    IRestResponse result = _smsService.SendOTP(MobileNumber, OTP, "Reset Password");
                    response.Data = result.Content.ToString();
                }
                catch (Exception exception)
                {
                    response.State = ResponseState.Error;
                    response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                    _logger.LogError(exception, "Error Getting GenerateOTP==>" + exception.StackTrace, MobileNumber);
                }
            }
            else
            {
                response.Messages.Add("Invalid Mobile Number!");
                response.State = ResponseState.Error;
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("VerifyOTP")]
        public IActionResult VerifyOTP(string SessionId, string OTP, string MobileNumber)
        {
            var response = new OperationResponse<Users>();
            try
            {
                IRestResponse result = _smsService.VerifyOTP(SessionId, OTP);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    response.Data = null;
                    response.State = ResponseState.Error;
                    response.Messages.Add("Invalid OTP.");
                }
                else
                {
                    response.Data = _userService.GetUsersDetail(MobileNumber);
                }
                //response.Data = result.Content.ToString();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GenerateOTP==>" + exception.StackTrace, SessionId, OTP, MobileNumber);
            }
            return new JsonResult(response);
        }
    }
}
