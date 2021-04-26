using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Frontend.Web.Api.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelCategories;
using ModelNotification;
using ModelProduct;
using ModelProductImages;
using RST.Shared;
using RST.Shared.Enums;
using ServiceNotification;
using ServiceProduct;
using ServiceProductImage;

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductImageService _productImageService;
        private ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;
        INotificationService _notificationService;
        public ProductController(IProductService productService,
            INotificationService notificationService,
            ILogger<ProductController> logger,
            IConfiguration configuration,
             IProductImageService productImageService)
        {
            _productService = productService;
            _notificationService = notificationService;
            _logger = logger;
            _configuration = configuration;
            _productImageService = productImageService;
        }

        /// <summary>
        /// Image Upload
        /// </summary>
        #region Image Upload
        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload()
        {
            var response = new OperationResponse<ProductImages>();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    int MaxId = _productImageService.GetMaxProductImageId();

                    var file = Request.Form.Files[0];


                    string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
                    string ThumbnailImagePath = _configuration["ImagePathConfiguration:ThumbnailImagePath"];

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
                    fileName = (MaxId + 1).ToString() + fileExt;

                    ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, fileName);

                    ProductImages request = new ProductImages();
                    request.ImageName = fileName;
                    request.ImagePath = Path.Combine(OriginalImagePath, fileName);
                    request.ThumbnailPath = Path.Combine(ThumbnailImagePath, fileName);
                    request.Description = "";
                    request.DisplayOrder = 1;
                    request.IsDisplay = true;

                    response.Data = _productImageService.AddProductImages(request);
                }
                else
                {
                    response.Messages = new List<string>();
                    response.Messages.Insert(0, @"Please upload logo.");
                }

            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductCategory ==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateImage")]
        public IActionResult UpdateImage(string ImageName, string ModuleName, int RowId)
        {
            var response = new OperationResponse<ProductImages>();
            try
            {
                string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
                string ThumbnailImagePath = _configuration["ImagePathConfiguration:ThumbnailImagePath"];

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), OriginalImagePath);
                    var fullPath = Path.Combine(pathToSave, ImageName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, ImageName);
                    }
                    else
                    {
                        int MaxId = _productImageService.GetMaxProductImageId();

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
                        fileName = (MaxId + 1).ToString() + fileExt;

                        ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, fileName);

                        ProductImages request = new ProductImages();
                        request.ImageName = fileName;
                        request.ImagePath = Path.Combine(OriginalImagePath, fileName);
                        request.ThumbnailPath = Path.Combine(ThumbnailImagePath, fileName);
                        request.Description = "";
                        request.DisplayOrder = 1;
                        request.IsDisplay = true;

                        response.Data = _productImageService.AddProductImages(request);

                        // Update Image in Module as well
                        _productImageService.UpdateImage(request.RowId, ModuleName, RowId);
                    }
                }
                else
                {
                    response.Messages.Add("Please upload Image");
                }

            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateImage ==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UploadMultipleFiles")]
        public IActionResult UploadMultipleFiles(int ProductId)
        {
            var response = new OperationResponse<ProductImages>();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var item in Request.Form.Files)
                    {
                        int MaxId = _productImageService.GetMaxProductImageId();

                        var file = item;

                        string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
                        string ThumbnailImagePath = _configuration["ImagePathConfiguration:ThumbnailImagePath"];

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
                        fileName = (MaxId + 1).ToString() + fileExt;

                        ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, fileName);

                        //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                        //var fullPath = Path.Combine(pathToSave, fileName);
                        //using (var stream = new FileStream(fullPath, FileMode.Create))
                        //{
                        //    file.CopyTo(stream);
                        //}

                        ProductImages request = new ProductImages();
                        request.ImageName = fileName;
                        request.ImagePath = Path.Combine(OriginalImagePath, fileName);
                        request.ThumbnailPath = Path.Combine(ThumbnailImagePath, fileName);
                        request.Description = "";
                        request.DisplayOrder = 1;
                        request.IsDisplay = true;

                        response.Data = _productImageService.AddProductImages(request);



                    }
                }
                else
                {
                    response.Messages.Add("Please upload Image");
                }

            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductCategory ==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Brand
        /// </summary>
        #region Brand
        [HttpPost]
        [Route("GetBrands")]
        public IActionResult GetBrands(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetBrands(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetBrands==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Product Attribute Parent
        /// </summary>
        #region Product Attribute Parent
        [HttpPost]
        [Route("GetProductAttributeParent")]
        public IActionResult GetProductAttributeParent(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProductAttributeParent(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductAttributeParent==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Product Attribute
        /// </summary>
        #region ProductAttribute
        [HttpPost]
        [Route("GetProductAttribute")]
        public IActionResult GetProductAttribute(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProductAttribute(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductAttribute==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Product
        /// </summary>
        #region Product
        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct([FromBody] Product request)
        {
            var response = new OperationResponse<int>();
            try
            {
                response.Data = _productService.AddProduct(request);
                _notificationService.AddNotification(new Notification() { ProductId = response.Data, TextPrompt = NotificationTextPrompt.NewProduct });
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProduct ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] ProductDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.UpdateProduct(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProduct ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct([FromBody] ProductDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.DeleteProduct(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProduct ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetFilteredProductList")]
        public IActionResult GetFilteredProductList([FromBody] ProductFilterRequest request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetFilteredProductList(request.ProductName, request.year, request.CategoryId, request.ParentSubCategoryId,
                    request.SubCategoryId, request.ProductTypeId, request.BrandId, request.PriceRangeMin, request.PriceRangeMax,
                    request.SortBy, request.IsAsc, request.PageNumber, request.NoOfRecord);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProduct==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetMinMaxPriceRange")]
        public IActionResult GetMinMaxPriceRange([FromBody] ProductFilterRequest request)
        {
            var response = new OperationResponse<ProductRange>();
            try
            {
                response.Data = _productService.GetMinMaxPriceRange(request.year, request.CategoryId, request.ParentSubCategoryId,
                    request.SubCategoryId, request.ProductTypeId, request.BrandId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetMinMaxPriceRange==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductDetailbyId")]
        public IActionResult GetProductDetailbyId(int ProductId)
        {
            var response = new OperationResponse<ProductDetails>();
            try
            {
                response.Data = _productService.GetProductDetailbyId(ProductId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductDetailbyId==>" + exception.StackTrace, ProductId);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Product Mapping
        /// </summary>
        #region Product Mapping
        [HttpPost]
        [Route("GetProductAttributeMapping")]
        public IActionResult GetProductAttributeMapping(int ProductId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProductAttributeMapping(ProductId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductAttributeMapping==>" + exception.StackTrace, ProductId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductAttributeProductList")]
        public IActionResult UpdateProductAttributeProductList(ProductAttribute_ProductMappingDTO request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _productService.UpdateProductAttributeProductList(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting UpdateProductAttributeProductList==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductAttributeProduct")]
        public IActionResult UpdateProductAttributeProduct([FromBody] ProductAttribute_ProductDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.UpdateProductAttributeProduct(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProduct ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductAttributeMapping")]
        public IActionResult DeleteProductAttributeMapping([FromBody] ProductAttribute_ProductDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.DeleteProductAttributeMapping(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductAttribute ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        #region Product-color

        #endregion

        #region Product-Images
        [HttpPost]
        [Route("GetProductImageMapping")]
        public IActionResult GetProductImageMapping(int ProductId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productImageService.GetProductImage_Product(ProductId);

            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductImageMapping==>" + exception.StackTrace, ProductId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductImageProduct")]
        public IActionResult DeleteProductImageProduct([FromBody] ProductImageProductDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productImageService.DeleteProductImage_Product_dyId(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProduct ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }
        #endregion



        [HttpPost]
        [Route("GetRecommendedProducts")]
        public IActionResult GetRecommendedProducts(int ProductId, int PageNo, int RecordCount)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetRecommendedProducts(ProductId, PageNo, RecordCount);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetRecommendedProducts==>" + exception.StackTrace, ProductId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetFeaturedProducts")]
        public IActionResult GetFeaturedProducts(int ProductId, int PageNo, int RecordCount)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetFeaturedProductList(ProductId, PageNo, RecordCount);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetFeaturedProductList==>" + exception.StackTrace, ProductId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetSimilerProducts")]
        public IActionResult GetSimilerProducts(int ProductId, int PageNo, int RecordCount)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetSimilerProductList(ProductId, PageNo, RecordCount);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetSimilerProductList==>" + exception.StackTrace, ProductId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetHomePageProducts")]
        public IActionResult GetHomePageProducts(int PageNo, int RecordCount)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetHomePageProducts(PageNo, RecordCount);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetHomePageProducts==>" + exception.StackTrace, PageNo, RecordCount);
            }
            return new JsonResult(response);
        }
        #endregion

        [HttpPost]
        [Route("GetProductSuggestion")]
        public IActionResult GetProductSuggestion(string Suggestion)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProductSuggestion(Suggestion);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductSuggestion==>" + exception.StackTrace, Suggestion);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetAttributehierarchy")]
        public IActionResult GetAttributehierarchy([FromBody] AttributeRequest request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetAttributehierarchy(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetAttributehierarchy==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

    }
}
