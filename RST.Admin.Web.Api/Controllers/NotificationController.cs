using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelNotification;
using RST.Shared;
using RST.Shared.Enums;
using ServiceNotification;

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : Controller
    {
        INotificationService _notificationService;
        private ILogger<NotificationController> _logger;
        private readonly IConfiguration _configuration;
        public NotificationController(INotificationService notificationService,
            ILogger<NotificationController> logger,
            IConfiguration configuration)
        {
            _notificationService = notificationService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Notification
        /// </summary>
        #region Notification
        [HttpPost]
        [Route("UpdateNotification")]
        public IActionResult UpdateNotification([FromBody] NotificationDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _notificationService.UpdateNotification(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateNotification ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteNotification")]
        public IActionResult DeleteNotification([FromBody] NotificationDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _notificationService.DeleteNotification(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteNotification ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetNotification")]
        public IActionResult GetNotification(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _notificationService.GetNotification(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetNotification==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
