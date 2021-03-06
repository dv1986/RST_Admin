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
using ModelCommon;
using RST.Shared;
using RST.Shared.Enums;
using ServiceCategories;

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
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


        [HttpPost]
        [Route("GetCategoryhierarchy")]
        public IActionResult GetCategoryhierarchy()
        {
            var response = new OperationResponse<Categoryhierarchy>();
            try
            {
                response.Data = _categoriesService.GetCategoryhierarchy();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetCategoryhierarchy==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
    }
}
