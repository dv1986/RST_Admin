using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelCategories;
using RST.Shared;
using RST.Shared.Enums;
using ServiceCategories;

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : Controller
    {
        ICategoriesService _categoriesService;
        private ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;
        public CategoriesController(ICategoriesService categoriesService,
            ILogger<ProductController> logger,
            IConfiguration configuration)
        {
            _categoriesService = categoriesService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Category Parent
        /// </summary>
        #region Category Parent
        [HttpPost]
        [Route("AddProductCategoryParent")]
        public IActionResult AddProductCategoryParent([FromBody] ProductCategoryParent request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductCategoryParent(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductCategoryParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductCategoryParent")]
        public IActionResult UpdateProductCategoryParent([FromBody] ProductCategoryParentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductCategoryParent(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductCategoryParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductCategoryParent")]
        public IActionResult DeleteProductCategoryParent([FromBody] ProductCategoryParentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductCategoryParent(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductCategoryParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductCategoryParent")]
        public IActionResult GetProductCategoryParent(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductCategoryParent(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductCategoryParent==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// Category
        /// </summary>
        #region Category
        [HttpPost]
        [Route("AddProductCategory")]
        public IActionResult AddProductCategory([FromBody] ProductCategory request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductCategory(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("UpdateProductCategory")]
        public IActionResult UpdateProductCategory([FromBody] ProductCategoryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductCategory(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductCategory")]
        public IActionResult DeleteProductCategory([FromBody] ProductCategoryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductCategory(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductCategory")]
        public IActionResult GetProductCategory(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductCategory(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductCategory==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetCategoryLookup")]
        public IActionResult GetCategoryLookup(int CategoryParentId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetCategoryLookup(CategoryParentId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetCategoryLookup==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// Sub-Category
        /// </summary>
        #region Sub-Category
        [HttpPost]
        [Route("AddProductSubCategory")]
        public IActionResult AddProductSubCategory([FromBody] ProductSubCategory request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductSubCategory(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductSubCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductSubCategory")]
        public IActionResult UpdateProductSubCategory([FromBody] ProductSubCategoryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductSubCategory(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductSubCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductSubCategory")]
        public IActionResult DeleteProductSubCategory([FromBody] ProductSubCategoryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductSubCategory(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductSubCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductSubCategory")]
        public IActionResult GetProductSubCategory(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductSubCategory(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductSubCategory==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetSubCategoryLookup")]
        public IActionResult GetSubCategoryLookup(int SubCategoryParentId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetSubCategoryLookup(SubCategoryParentId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetSubCategoryLookup==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// Sub-Category Parent
        /// </summary>
        #region Sub-Category Parent
        [HttpPost]
        [Route("AddProductSubCategoryParent")]
        public IActionResult AddProductSubCategoryParent([FromBody] ProductSubCategoryParent request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductSubCategoryParent(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductSubCategoryParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductSubCategoryParent")]
        public IActionResult UpdateProductSubCategoryParent([FromBody] ProductSubCategoryParentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductSubCategoryParent(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductSubCategoryParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductSubCategoryParent")]
        public IActionResult DeleteProductSubCategoryParent([FromBody] ProductSubCategoryParentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductSubCategoryParent(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductSubCategoryParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductSubCategoryParent")]
        public IActionResult GetProductSubCategoryParent(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductSubCategoryParent(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductSubCategoryParent==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetSubCategoryParentLookup")]
        public IActionResult GetSubCategoryParentLookup(int CategoryId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetSubCategoryParentLookup(CategoryId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetSubCategoryParentLookup==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Product-Type
        /// </summary>
        #region Product-Type
        [HttpPost]
        [Route("AddProductType")]
        public IActionResult AddProductType([FromBody] ProductType request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductType(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductType ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductType")]
        public IActionResult UpdateProductType([FromBody] ProductTypeDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductType(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductType ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductType")]
        public IActionResult DeleteProductType([FromBody] ProductTypeDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductType(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductType ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetProductType")]
        public IActionResult GetProductType(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductType(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductType==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductTypeLookup")]
        public IActionResult GetProductTypeLookup(int SubCategoryId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductTypeLookup(SubCategoryId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductTypeLookup==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion


        /// <summary>
        /// Product-Feature-Category
        /// </summary>
        #region Product-Feature-Category
        [HttpPost]
        [Route("AddProductsFeaturesCategory")]
        public IActionResult AddProductsFeaturesCategory([FromBody] ProductsFeaturesCategory request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductsFeaturesCategory(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductsFeaturesCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductsFeaturesCategory")]
        public IActionResult UpdateProductsFeaturesCategory([FromBody] ProductsFeaturesCategoryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductsFeaturesCategory(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductsFeaturesCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductsFeaturesCategory")]
        public IActionResult DeleteProductsFeaturesCategory([FromBody] ProductsFeaturesCategoryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductsFeaturesCategory(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductsFeaturesCategory ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductsFeaturesCategory")]
        public IActionResult GetProductsFeaturesCategory(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductsFeaturesCategory(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductsFeaturesCategory==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion



        /// <summary>
        /// Product-Feature
        /// </summary>
        #region Product-Feature-Category
        [HttpPost]
        [Route("AddProductFeatures")]
        public IActionResult AddProductFeatures([FromBody] ProductFeatures request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _categoriesService.AddProductFeatures(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductFeatures ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductFeatures")]
        public IActionResult UpdateProductFeatures([FromBody] ProductFeaturesDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.UpdateProductFeatures(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductFeatures ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductFeatures")]
        public IActionResult DeleteProductFeatures([FromBody] ProductFeaturesDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _categoriesService.DeleteProductFeatures(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductFeatures ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductFeatures")]
        public IActionResult GetProductFeatures(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _categoriesService.GetProductFeatures(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductFeatures==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
