using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Infrastructure.Grid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelCategories;
using ModelCommon;
using ModelNotification;
using ModelProduct;
using ModelProductImages;
using RST.Admin.Web.Api.Helper;
using RST.Shared;
using RST.Shared.Enums;
using ServiceNotification;
using ServiceProduct;
using ServiceProductImage;

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductImageService _productImageService;
        private ILogger<ProductController> _logger;
        private IGridHandler _gridHandler;
        private readonly IConfiguration _configuration;
        INotificationService _notificationService;
        public ProductController(IProductService productService,
            INotificationService notificationService,
            ILogger<ProductController> logger,
            IConfiguration configuration,
            IProductImageService productImageService,
            IGridHandler gridHandler)
        {
            _productService = productService;
            _notificationService = notificationService;
            _logger = logger;
            _configuration = configuration;
            _productImageService = productImageService;
            _gridHandler = gridHandler;
        }

        /// <summary>
        /// Image Upload
        /// </summary>
        //#region Image Upload
        //[HttpPost]
        //[Route("Upload")]
        //public IActionResult Upload()
        //{
        //    var response = new OperationResponse<ProductImages>();
        //    try
        //    {
        //        if (Request.Form.Files.Count > 0)
        //        {
        //            int MaxId = _productImageService.GetMaxProductImageId();

        //            var file = Request.Form.Files[0];


        //            string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
        //            string ThumbnailImagePath = _configuration["ImagePathConfiguration:ThumbnailImagePath"];

        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
        //            fileName = (MaxId + 1).ToString() + fileExt;

        //            ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, fileName);

        //            ProductImages request = new ProductImages();
        //            request.ImageName = fileName;
        //            request.ImagePath = Path.Combine(OriginalImagePath, fileName);
        //            request.ThumbnailPath = Path.Combine(ThumbnailImagePath, fileName);
        //            request.Description = "";
        //            request.DisplayOrder = 1;
        //            request.IsDisplay = true;

        //            response.Data = _productImageService.AddProductImages(request);
        //        }
        //        else
        //        {
        //            response.Messages = new List<string>();
        //            response.Messages.Insert(0, @"Please upload logo.");
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        response.State = ResponseState.Error;
        //        response.Messages.Add(exception.Message);
        //        _logger.LogError(exception, "Error in AddProductCategory ==>" + exception.StackTrace);
        //    }
        //    return new JsonResult(response);
        //}

        //[HttpPost]
        //[Route("UpdateImage")]
        //public IActionResult UpdateImage(string ImageName, string ModuleName, int RowId)
        //{
        //    var response = new OperationResponse<ProductImages>();
        //    try
        //    {
        //        string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
        //        string ThumbnailImagePath = _configuration["ImagePathConfiguration:ThumbnailImagePath"];

        //        if (Request.Form.Files.Count > 0)
        //        {
        //            var file = Request.Form.Files[0];
        //            //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), OriginalImagePath);
        //            //var fullPath = Path.Combine(pathToSave, ImageName);
        //            var fullPath = Path.Combine(OriginalImagePath, ImageName);

        //            if (System.IO.File.Exists(fullPath))
        //            {
        //                ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, ImageName);
        //            }
        //            else
        //            {
        //                int MaxId = _productImageService.GetMaxProductImageId();

        //                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //                var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
        //                fileName = (MaxId + 1).ToString() + fileExt;

        //                ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, fileName);

        //                ProductImages request = new ProductImages();
        //                request.ImageName = fileName;
        //                request.ImagePath = Path.Combine(OriginalImagePath, fileName);
        //                request.ThumbnailPath = Path.Combine(ThumbnailImagePath, fileName);
        //                request.Description = "";
        //                request.DisplayOrder = 1;
        //                request.IsDisplay = true;

        //                response.Data = _productImageService.AddProductImages(request);

        //                // Update Image in Module as well
        //                _productImageService.UpdateImage(request.RowId, ModuleName, RowId);
        //            }
        //        }
        //        else
        //        {
        //            response.Messages.Add("Please upload Image");
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        response.State = ResponseState.Error;
        //        response.Messages.Add(exception.Message);
        //        _logger.LogError(exception, "Error in UpdateImage ==>" + exception.StackTrace);
        //    }
        //    return new JsonResult(response);
        //}

        //[HttpPost]
        //[Route("UploadMultipleFiles")]
        //public IActionResult UploadMultipleFiles(int ProductId)
        //{
        //    var response = new OperationResponse<ProductImages>();
        //    try
        //    {
        //        if (Request.Form.Files.Count > 0)
        //        {
        //            foreach (var item in Request.Form.Files)
        //            {
        //                int MaxId = _productImageService.GetMaxProductImageId();

        //                var file = item;

        //                string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
        //                string ThumbnailImagePath = _configuration["ImagePathConfiguration:ThumbnailImagePath"];

        //                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //                var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
        //                fileName = (MaxId + 1).ToString() + fileExt;

        //                ImageCompressHelper.CompressImage(OriginalImagePath, ThumbnailImagePath, file, fileName);

        //                ProductImages request = new ProductImages();
        //                request.ImageName = fileName;
        //                request.ImagePath = Path.Combine(OriginalImagePath, fileName);
        //                request.ThumbnailPath= Path.Combine(ThumbnailImagePath, fileName);
        //                request.Description = "";
        //                request.DisplayOrder = 1;
        //                request.IsDisplay = true;

        //                response.Data = _productImageService.AddProductImages(request);

        //                ProductImageProduct request2 = new ProductImageProduct();
        //                request2.ProductId = ProductId;
        //                request2.ProductImageId = response.Data.RowId;
        //                _productImageService.AddProductImage_Product(request2);
        //            }
        //        }
        //        else
        //        {
        //            response.Messages.Add("Please upload Image");
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        response.State = ResponseState.Error;
        //        response.Messages.Add(exception.Message);
        //        _logger.LogError(exception, "Error in AddProductCategory ==>" + exception.StackTrace);
        //    }
        //    return new JsonResult(response);
        //}
        //#endregion

        /// <summary>
        /// Brand
        /// </summary>
        #region Brand
        [HttpPost]
        [Route("AddBrands")]
        public IActionResult AddBrands([FromBody] Brands request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _productService.AddBrands(request);
                //ServiceHelper.Helper.CompressImage();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddBrands ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);

        }

        [HttpPost]
        [Route("UpdateBrands")]
        public IActionResult UpdateBrands([FromBody] BrandsDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.UpdateBrands(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateBrands ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteBrands")]
        public IActionResult DeleteBrands([FromBody] BrandsDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.DeleteBrands(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteBrands ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

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
        [Route("AddProductAttributeParent")]
        public IActionResult AddProductAttributeParent([FromBody] ProductAttributeParent request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _productService.AddProductAttributeParent(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductAttributeParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductAttributeParent")]
        public IActionResult UpdateProductAttributeParent([FromBody] ProductAttributeParentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.UpdateProductAttributeParent(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductAttributeParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductAttributeParent")]
        public IActionResult DeleteProductAttributeParent([FromBody] ProductAttributeParentDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.DeleteProductAttributeParent(request.Tasks);
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
                _logger.LogError(exception, "Error in DeleteProductAttributeParent ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        //[HttpPost]
        //[Route("GetProductAttributeParent")]
        //public IActionResult GetProductAttributeParent(string SearchStr)
        //{
        //    var response = new OperationResponse<ICollection>();
        //    try
        //    {
        //        response.Data = _productService.GetProductAttributeParent(SearchStr);
        //    }
        //    catch (Exception exception)
        //    {
        //        response.State = ResponseState.Error;
        //        response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
        //        _logger.LogError(exception, "Error Getting GetProductAttributeParent==>" + exception.StackTrace, SearchStr);
        //    }
        //    return new JsonResult(response);
        //}

        [HttpPost]
        [Route("GetProductAttributeParent")]
        public IActionResult GetProductAttributeParent([FromBody] DataGridRequest<CommonSearchRequest> request)
        {
            var response = new DataGridResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProductAttributeParent("");
                response = _gridHandler.CacheData(response.Data, request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductAttributeParent==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetProductAttributeParentbyProdcutType")]
        public IActionResult GetProductAttributeParentbyProdcutType(int ProductTypeId)
        {
            var response = new DataGridResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProductAttributeParentbyProdcutType(ProductTypeId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProductAttributeParentbyProdcutType==>" + exception.StackTrace, ProductTypeId);
            }
            return new JsonResult(response);
        }
        #endregion

        /// <summary>
        /// Product Attribute
        /// </summary>
        #region ProductAttribute
        [HttpPost]
        [Route("AddProductAttribute")]
        public IActionResult AddProductAttribute([FromBody] ProductAttribute request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _productService.AddProductAttribute(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddProductAttribute ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateProductAttribute")]
        public IActionResult UpdateProductAttribute([FromBody] ProductAttributeDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.UpdateProductAttribute(request.Tasks);
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
                _logger.LogError(exception, "Error in UpdateProductAttribute ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteProductAttribute")]
        public IActionResult DeleteProductAttribute([FromBody] ProductAttributeDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _productService.DeleteProductAttribute(request.Tasks);
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
        [Route("GetProduct")]
        public IActionResult GetProduct(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetProduct(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetProduct==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

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
        #endregion

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

        #region ProductType Attribute Mapping
        [HttpPost]
        [Route("AddAttributeProductTypeMapping")]
        public IActionResult AddAttributeProductTypeMapping([FromBody] Attribute_ProductTypeMapping request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _productService.AddAttribute_ProductType(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddAttributeProductTypeMapping ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetAllAttributeProductTypeMapping")]
        public IActionResult GetAllAttributeProductTypeMapping()
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetAllAttributeProductTypeMapping();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetAllAttributeProductTypeMapping==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetAttributeProductTypeMapping")]
        public IActionResult GetAttributeProductTypeMapping(int? CategoryParentId, int? CategoryId,
            int? SubCategoryParentId, int? SubCategoryId, int? ProductTypeId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _productService.GetAttributeProductTypeMapping(
                    CategoryParentId, 
                    CategoryId, 
                    SubCategoryParentId, 
                    SubCategoryId, 
                    ProductTypeId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetAttributeProductTypeMapping==>" + exception.StackTrace, ProductTypeId);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
