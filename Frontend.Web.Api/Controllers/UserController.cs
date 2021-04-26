using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLookup;
using ModelUser;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using RST.Shared;
using RST.Shared.Enums;
using ServiceUsers;
using Microsoft.Extensions.Logging;

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        private readonly IEmailService _emailService;
        private ILogger<UserController> _logger;
        public UserController(IUserService userservice, IEmailService emailService, ILogger<UserController> logger)
        {
            _userService = userservice;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddUser")]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody] Users request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                request.Firstname = request.FullName.Split(' ')[0].Trim();
                request.Lastname = request.FullName.Replace(request.Firstname, "").Trim();

                var result = _userService.ValidateEmailandMobile(request.Email, request.Mobile);
                if (result == "")
                {
                    response.Data = _userService.AddUser(request);
                }
                else
                {
                    response.Messages.Add(result);
                    response.State = ResponseState.Error;
                }
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddUser ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdatePassword")]
        [AllowAnonymous]
        public IActionResult UpdatePassword([FromBody] Users request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                //var result = _userService.ValidateEmailandMobile(request.Email, request.Mobile);
                //if (result == "")
                //{
                response.Data = _userService.UpdatePassword(request);
                //}
                //else
                //{
                //    response.Messages.Add(result);
                //    response.State = ResponseState.Error;
                //}
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdatePassword ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UsersDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _userService.UpdateUser(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateUser ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public IActionResult DeleteUser([FromBody] UsersDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _userService.DeleteUser(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in DeleteUser ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetUsers")]
        public IActionResult GetUsers(string UserName)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _userService.GetAll(UserName);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetUsers==>" + exception.StackTrace, UserName);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetUsersDetail")]
        public IActionResult GetUsersDetail(int Id)
        {
            var response = new OperationResponse<Users>();
            try
            {
                response.Data = _userService.GetUsersDetail(Id);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetUsersDetail==>" + exception.StackTrace, Id);
            }
            return new JsonResult(response);
        }


        #region User Permission 
        [HttpPost]
        [Route("AddUserPermission")]
        public IActionResult AddUserPermission(User_SubMenuDTO request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _userService.AddUserPermission(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                //_logger.LogError(exception, "Error in Delete Bts Pack ==>" + exception.StackTrace, BtsPackId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetUserPermissionforUser")]
        public IActionResult GetUserPermissionforUser(int UserId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _userService.GetUserPermissionforUser(UserId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                //_logger.LogError(exception, "Error Getting GetAvailableApprovers==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }
        #endregion


        [HttpPost]
        [Route("SendEmail")]
        [AllowAnonymous]
        public IActionResult SendEmail()
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                string htmlString = "<h1>Send from asp.net core mvc action</h1>";
                //_emailService.Send("kumawat29deepak@gmail.com, deepak29kumawat@gmail.com", "ASP.NET Core mvc send email example", htmlString, true, null);
                _emailService.Send("kumawat29deepak@gmail.com", "ASP.NET Core mvc send email example", htmlString, true);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                //_logger.LogError(exception, "Error Getting GetAvailableApprovers==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetAdvertisement")]
        public IActionResult GetAdvertisement()
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _userService.GetAdvertisement();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetAdvertisement==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
    }
}
