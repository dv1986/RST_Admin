using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelSpecifications;
using RST.Shared;
using RST.Shared.Enums;
using ServiceSpecification;

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class SpecificationController : Controller
    {
        ISpecificationService _specificationService;
        private ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;
        public SpecificationController(ISpecificationService specificationService, ILogger<ProductController> logger, IConfiguration configuration)
        {
            _specificationService = specificationService;
            _logger = logger;
            _configuration = configuration;
        }


        /// <summary>
        /// Colors
        /// </summary>
        #region Colors
        [HttpPost]
        [Route("GetColors")]
        public IActionResult GetColors(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _specificationService.GetColors(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetColors==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// MeasureDimension
        /// </summary>
        #region MeasureDimension
        [HttpPost]
        [Route("GetMeasureDimension")]
        public IActionResult GetMeasureDimension(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _specificationService.GetMeasureDimension(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetMeasureDimension==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// ProductFabric
        /// </summary>
        #region ProductFabric
        [HttpPost]
        [Route("GetProductFabric")]
        public IActionResult GetProductFabric(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _specificationService.GetProductFabric(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductFabric==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// ProductTag
        /// </summary>
        #region ProductTag
        [HttpPost]
        [Route("GetProductTag")]
        public IActionResult GetProductTag(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _specificationService.GetProductTag(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductTag==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// ProductSizeType
        /// </summary>
        #region ProductSizeType
        [HttpPost]
        [Route("GetProductSizeType")]
        public IActionResult GetProductSizeType(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _specificationService.GetProductSizeType(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductSizeType==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
