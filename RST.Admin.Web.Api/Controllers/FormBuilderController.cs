using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Grid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelFormBuilder;
using RST.Shared;
using RST.Shared.Enums;
using ServiceFormBuilder;
using ServiceNotification;

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormBuilderController : Controller
    {
        private ILogger<ProductController> _logger;
        private IGridHandler _gridHandler;
        private readonly IConfiguration _configuration;
        INotificationService _notificationService;
        IFormBuilderService _formBuilderService;
        public FormBuilderController(IFormBuilderService formBuilderService,
            INotificationService notificationService,
            ILogger<ProductController> logger,
            IConfiguration configuration,
            IGridHandler gridHandler)
        {
            _notificationService = notificationService;
            _logger = logger;
            _configuration = configuration;
            _gridHandler = gridHandler;
            _formBuilderService = formBuilderService;
        }


        /// <summary>
        /// Form Builder
        /// </summary>
        #region Form Builder
        [HttpPost]
        [Route("AddFormBuilder")]
        public IActionResult AddFormBuilder([FromBody] FormBuilder request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _formBuilderService.AddFormBuilder(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddFormBuilder ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateFormBuilderbyId")]
        public IActionResult UpdateFormBuilderbyId([FromBody] FormBuilder request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _formBuilderService.UpdateFormBuilderbyId(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateFormBuilderbyId ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateFormBuilder")]
        public IActionResult UpdateFormBuilder([FromBody] FormBuilderDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _formBuilderService.UpdateFormBuilder(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateFormBuilder ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteFormBuilder")]
        public IActionResult DeleteFormBuilder([FromBody] FormBuilderDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _formBuilderService.DeleteFormBuilder(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteFormBuilder ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetFormBuilder")]
        public IActionResult GetFormBuilder(int Id)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _formBuilderService.GetFormBuilder(Id);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetFormBuilder==>" + exception.StackTrace, Id);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
