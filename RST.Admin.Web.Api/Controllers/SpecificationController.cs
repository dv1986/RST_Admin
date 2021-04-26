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

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [Route("AddColors")]
        public IActionResult AddColors([FromBody] Colors request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _specificationService.AddColors(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddColors ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateColors")]
        public IActionResult UpdateColors([FromBody] ColorsDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.UpdateColors(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateColors ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteColors")]
        public IActionResult DeleteColors([FromBody] ColorsDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.DeleteColors(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteColors ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

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
        [Route("AddMeasureDimension")]
        public IActionResult AddMeasureDimension([FromBody] MeasureDimension request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _specificationService.AddMeasureDimension(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddMeasureDimension ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateMeasureDimension")]
        public IActionResult UpdateMeasureDimension([FromBody] MeasureDimensionDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.UpdateMeasureDimension(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateMeasureDimension ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteMeasureDimension")]
        public IActionResult DeleteMeasureDimension([FromBody] MeasureDimensionDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.DeleteMeasureDimension(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteMeasureDimension ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

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
        [Route("AddProductFabric")]
        public IActionResult AddProductFabric([FromBody] ProductFabric request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _specificationService.AddProductFabric(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductFabric ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductFabric")]
        public IActionResult UpdateProductFabric([FromBody] ProductFabricDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.UpdateProductFabric(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductFabric ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductFabric")]
        public IActionResult DeleteProductFabric([FromBody] ProductFabricDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.DeleteProductFabric(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductFabric ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

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
        [Route("AddProductTag")]
        public IActionResult AddProductTag([FromBody] ProductTag request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _specificationService.AddProductTag(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductTag ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductTag")]
        public IActionResult UpdateProductTag([FromBody] ProductTagDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.UpdateProductTag(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductTag ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductTag")]
        public IActionResult DeleteProductTag([FromBody] ProductTagDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.DeleteProductTag(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductTag ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

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
        [Route("AddProductSizeType")]
        public IActionResult AddProductSizeType([FromBody] ProductSizeType request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _specificationService.AddProductSizeType(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductSizeType ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductSizeType")]
        public IActionResult UpdateProductSizeType([FromBody] ProductSizeTypeDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.UpdateProductSizeType(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductSizeType ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductSizeType")]
        public IActionResult DeleteProductSizeType([FromBody] ProductSizeTypeDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _specificationService.DeleteProductSizeType(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductSizeType ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

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
