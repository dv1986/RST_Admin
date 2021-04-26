using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Content.Web.API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelProductImages;
using RST.Shared;
using RST.Shared.Enums;
using ServiceHelper;
using ServiceProductImage;

namespace Content.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploaderController : Controller
    {
        private readonly IConfiguration _configuration;
        IProductImageService _productImageService;
        public FileUploaderController(
            IConfiguration configuration,
            IProductImageService productImageService)
        {
            _configuration = configuration;
            _productImageService = productImageService;
        }

        public static string PostApi(string ApiUrl, Microsoft.AspNetCore.Http.IFormFile formFiles)
        {
            var request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            string postData = "{FileName:abc}";
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/form-data";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        public static string GetApi(string ApiUrl)
        {

            var responseString = "";
            var request = (HttpWebRequest)WebRequest.Create(ApiUrl);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (var response1 = request.GetResponse())
            {
                using (var reader = new StreamReader(response1.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;

        }

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
                    //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), OriginalImagePath);
                    //var fullPath = Path.Combine(pathToSave, ImageName);
                    var fullPath = Path.Combine(OriginalImagePath, ImageName);

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
                if (Request.Form.ContainsKey("ProductId"))
                {
                    ProductId = Convert.ToInt32(Request.Form["ProductId"].ToString());
                }

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

                        ProductImages request = new ProductImages();
                        request.ImageName = fileName;
                        request.ImagePath = Path.Combine(OriginalImagePath, fileName);
                        request.ThumbnailPath = Path.Combine(ThumbnailImagePath, fileName);
                        request.Description = "";
                        request.DisplayOrder = 1;
                        request.IsDisplay = true;

                        response.Data = _productImageService.AddProductImages(request);

                        ProductImageProduct request2 = new ProductImageProduct();
                        request2.ProductId = Convert.ToInt32(ProductId);
                        request2.ProductImageId = response.Data.RowId;
                        _productImageService.AddProductImage_Product(request2);
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
            }
            return new JsonResult(response);
        }


        [HttpGet]
        [Route("GetImage")]
        public IActionResult GetImage(string fileName)
        {
            //var response = new OperationResponse<string>();
            string response = "";
            try
            {
                string OriginalImagePath = _configuration["ImagePathConfiguration:OriginalImagePath"];
                response = ServiceHelper.Helper.Base64ToImage(Path.Combine(OriginalImagePath, fileName));
            }
            catch (Exception exception)
            {
                response = null;
                //response.State = ResponseState.Error;
                //response.Messages.Add(exception.Message);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("UploadMultipleFilesNew")]
        public IActionResult UploadMultipleFilesNew()
        {
            var response = new OperationResponse<bool>();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var item in Request.Form.Files)
                    {
                        int MaxId = _productImageService.GetMaxProductImageId();

                        var file = item;

                        string Temp1ImagePath = _configuration["ImagePathConfiguration:Temp1ImagePath"];
                        string Temp2ImagePath = _configuration["ImagePathConfiguration:Temp2ImagePath"];

                        Random r = new Random();
                        var x = r.Next(0, 1000000);
                        string number = x.ToString("000000");

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fileExt = fileName.Remove(0, fileName.LastIndexOf('.'));
                        fileName = number + fileExt;



                        ImageCompressHelper.CompressImageNew(Temp1ImagePath, Temp2ImagePath, file, fileName);

                        response.Data = true;
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
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
