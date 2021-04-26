using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RST.Shared.Enums;
using RST.Shared;
using Infrastructure.Grid;
//using ADO;
//changed
using ADO.NET;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RST.Admin.Web.Api.Dto.Grid;
using System.Net;
using System.Reflection;

namespace RST.Admin.Web.Api.Controllers.Common
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class GridController : Controller
    {
        private readonly ILogger<GridController> _logger;
        private IGridHandler _gridHandler;
        private IConfiguration _configuration;

        public GridController(IGridHandler gridHandler, ILogger<GridController> logger, IConfiguration configuration)
        {
            _gridHandler = gridHandler;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("GridDataRetrieval")]
        public IActionResult GridDataRetrieval([FromBody] DataGridRequest<object> request)
        {
            DataGridResponse<ICollection> response = null;
            if (string.IsNullOrEmpty(request.GridGuid))
            {
                response = new DataGridResponse<ICollection>
                {
                    State = ResponseState.DataExpired
                };
            }
            else
            {
                try
                {
                    response = _gridHandler.GetGridData(request);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error Retriving data from cache");
                    response = new DataGridResponse<ICollection>() { State = ResponseState.Error };
                    response.Messages.Add(e.Message);
                }
            }
            return Json(response);
        }

        [HttpPost]
        [Route("GridDataUpdate")]
        public IActionResult GridDataUpdate([FromBody] DataGridUpdateDataRequest request)
        {
            OperationResponse<bool> response = null;
            if (string.IsNullOrEmpty(request.GridGuid))
            {
                response = new OperationResponse<bool>
                {
                    State = ResponseState.DataExpired
                };
            }
            else
            {
                try
                {
                    response = _gridHandler.UpdateGridData(request.GridGuid, request.DataToUpdate, request.KeyField);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error update data to cache");
                    response = new OperationResponse<bool>() { State = ResponseState.Error };
                    response.Messages.Add(e.Message);
                }
            }
            return Json(response);
        }

        [HttpGet]
        [Route("GetFilterSet")]
        public IActionResult GetFilterSet(string gridId, string filedName)
        {
            OperationResponse<ICollection> response = null;
            if (string.IsNullOrEmpty(gridId))
            {
                response = new OperationResponse<ICollection>
                {
                    State = ResponseState.DataExpired
                };
            }
            else
            {
                try
                {
                    response = _gridHandler.GetSetFilter(gridId, filedName);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error Get SetFilter");
                    response = new OperationResponse<ICollection>() { State = ResponseState.Error };
                    response.Messages.Add(e.Message);
                }
            }
            return Json(response);

        }

        [HttpPost]
        [Route("GetMetaData")]
        [AllowAnonymous]
        public IActionResult GetMetaData([FromBody] DataGridRequest<bool> request)
        {
            var result = new DataGridResponse<bool>();
            try
            {
                result.HeaderMetaData = _gridHandler.GetColumnMetaData(request.ColumnHeaders);
            }
            catch (Exception e)
            {
                result.State = ResponseState.Error;
                result.Messages.Add("Error Loading MetaData");
                _logger.LogError(e, "Error Loading MetaData");
            }
            return Json(result);
        }

        [HttpPost]
        [Route("SetMetaData")]

        public IActionResult SetMetaData([FromBody] List<ColumnMetaData> request)
        {
            var result = new OperationResponse<List<ColumnMetaData>>();
            try
            {
                _gridHandler.UpdateColumnMetaData(request);
                result.Data = request;
            }
            catch (Exception e)
            {
                result.State = ResponseState.Error;
                result.Messages.Add("Error Setting new MetaData");
                _logger.LogError(e, "Error Setting MetaData");
            }
            return Json(result);
        }
        [HttpPost]
        [Route("RefreshMetaData")]

        public IActionResult RefreshMetaData()
        {
            var result = new OperationResponse<bool>();
            try
            {
                _gridHandler.RefreshMetaData();
                result.Data = true;
            }
            catch (Exception e)
            {
                result.State = ResponseState.Error;
                result.Messages.Add("Error Refreshing MetaData");
                _logger.LogError(e, "Error Refreshing MetaData");
            }
            return Json(result);
        }

        [HttpPost]
        [Route("SetGridState")]
        public IActionResult SetGridState([FromBody] GridSetStateDto request)
        {
            var result = new OperationResponse<bool>();
            var connectionFactory =
                new AppConfigConnectionFactory(_configuration.GetConnectionString("CosDB"), "System.Data.SqlClient");
            var context = new AdoNetContext(connectionFactory);
            try
            {

                _gridHandler.SetGridState(request.UserName, request.GridKey, request.State, context);
                result.Data = true;
            }
            catch (Exception e)
            {
                result.State = ResponseState.Error;
                result.Messages.Add("Error Setting grid state");
                _logger.LogError(e, "Error Setting grid state");
            }
            finally
            {
                context.Dispose();
            }
            return Json(result);
        }

        [HttpPost]
        [Route("GetGridState")]
        public IActionResult GetGridState([FromBody] GridGetStateDto request)
        {
            var result = new OperationResponse<string>();
            var connectionFactory =
                new AppConfigConnectionFactory(_configuration.GetConnectionString("CosDB"), "System.Data.SqlClient");
            var context = new AdoNetContext(connectionFactory);
            try
            {
                result.Data = _gridHandler.GetGridState(request.UserName, request.GridKey, context);
            }
            catch (Exception e)
            {
                result.State = ResponseState.Error;
                result.Messages.Add("Error getting grid state");
                _logger.LogError(e, "Error getting grid state");
            }
            finally
            {
                context.Dispose();
            }
            return Json(result);
        }
        [HttpPost]
        [Route("ResetGridState")]
        public IActionResult ResetGridState([FromBody] GridGetStateDto request)
        {
            var result = new OperationResponse<bool>();
            var connectionFactory =
                new AppConfigConnectionFactory(_configuration.GetConnectionString("CosDB"), "System.Data.SqlClient");
            var context = new AdoNetContext(connectionFactory);
            try
            {
                result.Data = _gridHandler.ResetGridState(request.UserName, request.GridKey, context);
            }
            catch (Exception e)
            {
                result.State = ResponseState.Error;
                result.Messages.Add("Error resetting grid state");
                _logger.LogError(e, "Error resetting grid state");
            }
            finally
            {
                context.Dispose();
            }
            return Json(result);
        }
        [HttpPost]
        [Route("ExportToExcel")]
        public IActionResult ExportToExcel([FromBody] ExportRequest request)
        {
            IEnumerable data = null;
            if (!string.IsNullOrEmpty(request.CacheId) && request.CacheId != "00000000-0000-0000-0000-000000000000")
            {
                request.LocalData = _gridHandler.GetCachedData(request.CacheId, request.ColumnFilters, request.SortColumns);
            }

            //if (request.LocalData != null)
            //{
            //    //var excelStream = ExportBidAsExcel(request.ColumnHeaders, request.LocalData);
            //    var excelStream = ExportBidAsExcel(request.ColumnHeaders, request.LocalData);
            //    excelStream.Position = 0;
            //    Response.StatusCode = (int)HttpStatusCode.OK;
            //    return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //        $"Data-Excel-{DateTime.Now:dd-MM-yyyy}.xlsx");
            //}
            return NoContent();
        }


        //private Stream ExportBidAsExcel(List<ExportColumn> columns, ICollection<dynamic> data)
        //{
        //    var stream = new MemoryStream();
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add($"Data");
        //        for (int i = 0; i < columns.Count; i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = columns[i].HeaderName;
        //        }
        //        //Now Iterate the data 
        //        var index = 0;
        //        foreach (var record in data)
        //        {
        //            for (int j = 0; j < columns.Count; j++)
        //            {
        //                object value = null;
        //                if (record is JObject)
        //                {
        //                    value = ((JObject)record).GetValue(columns[j].FieldId).ToString();
        //                }
        //                else
        //                {
        //                    value = GetReflectedValue(record, columns[j].FieldId);
        //                }

        //                if (value != null)
        //                    worksheet.Cells[2 + index, 1 + j].Value = value;
        //            }
        //            index++;
        //        }
        //        worksheet.Cells[1, 1, index, columns.Count].AutoFitColumns();
        //        package.SaveAs(stream);
        //    }
        //    return stream;
        //}

        private object GetReflectedValue(object obj, string propertyName)
        {
            object result = null;
            if (propertyName.Contains("."))
            {
                var fields = propertyName.Split('.');
                result = obj;
                foreach (var field in fields)
                {
                    var property = result.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        result = property.GetValue(result);
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            else
            {
                var property = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    result = property.GetValue(obj);
                }
            }

            return result;
        }

        private string GetProperPropertyName(string propertyName)
        {
            return char.ToUpper(propertyName[0]) + propertyName.Substring(1);

        }
    }
}
