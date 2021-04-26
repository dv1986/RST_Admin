using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelSEO;
using RST.Shared;
using RST.Shared.Enums;
using ServiceSEO;

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SEOController : Controller
    {
        ISeoService _seoService;
        private ILogger<SEOController> _logger;
        public SEOController(ISeoService seoService, ILogger<SEOController> logger)
        {
            _seoService = seoService;
            _logger = logger;
        }

        #region SEO
        [HttpPost]
        [Route("AddSeoContent")]
        public IActionResult AddSeoContent([FromBody] SeoContent request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _seoService.AddSeoContent(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddSeoContent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateSeoContent")]
        public IActionResult UpdateSeoContent([FromBody] SeoContentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _seoService.UpdateSeoContent(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateSeoContent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteSeoContent")]
        public IActionResult DeleteSeoContent([FromBody] SeoContentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _seoService.DeleteSeoContent(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteSeoContent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetSeoContent")]
        public IActionResult GetSeoContent(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _seoService.GetSeoContent(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetSeoContent==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
