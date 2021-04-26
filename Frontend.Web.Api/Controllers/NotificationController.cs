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

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Route("GetApprovedNotification")]
        public IActionResult GetApprovedNotification()
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _notificationService.GetApprovedNotification();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetApprovedNotification==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
