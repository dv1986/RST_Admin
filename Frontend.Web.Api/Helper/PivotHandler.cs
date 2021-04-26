
using Infrastructure.Grid;
using Infrastructure.Pivot;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RST.Shared;
using RST.Shared.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend.Web.Api.Helper
{
    public class PivotHandler : IPivotHandler
    {
        //private IPivotCacheRepository _pivotCacheRepository;
        //private IPivotSegmentCacheRepository _pivotSegmentCacheRepository;

        //private IPivotRepository _pivotRepository;
        //private IConfiguration _configuration;

        //public PivotHandler(IPivotSegmentCacheRepository pivotSegmentCacheRepository,IPivotCacheRepository pivotCacheRepository,IPivotRepository pivotRepository,IConfiguration configuration)
        //{
        //    _pivotCacheRepository = pivotCacheRepository;
        //    _pivotRepository = pivotRepository;
        //    _configuration = configuration;
        //    _pivotSegmentCacheRepository = pivotSegmentCacheRepository;
        //}
        private IMemoryCache _cache;
        private ILogger<GridHandler> _logger;
        private MetaDataHelper _metaDataHelper;
        private IPivotRepository _pivotRepository;
        private IConfiguration _configuration;
        private string cubeCacheServerUrl = string.Empty;

        public PivotHandler(IMemoryCache cache, ILogger<GridHandler> logger, MetaDataHelper metaDataHelper, IPivotRepository pivotRepository,
           IConfiguration configuration)
        {
            _cache = cache;
            _logger = logger;
            _metaDataHelper = metaDataHelper;
            _pivotRepository = pivotRepository;
            _configuration = configuration;

            this.cubeCacheServerUrl = _configuration.GetValue<string>("cubeCacheServerUrl") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(this.cubeCacheServerUrl))
            {
                throw new Exception("missing required config setting : cubeCacheServerUrl");
            }
        }
        public DataPivotResponse<ICollection> GetPivotAllData<T>(DataPivotRequest<T> request)
        {
            /*var response = new DataPivotResponse<ICollection>();
            string CacheKey = _pivotCacheRepository.GetCacheKey(request.Filters, request.MDXProcedure);
            string TableName = _pivotCacheRepository.GetCachTable(CacheKey, PivotMode.CLIENT);
            if (TableName == null)
            {
                DataTable data = _pivotRepository.GetDataFromDB(request.FieldToProcedureParamLookup, request.MDXProcedure);
                TableName = "pvt_" + Guid.NewGuid().ToString();
                _pivotCacheRepository.CachePivotAllData(TableName,data);
                _pivotCacheRepository.AddCachTable(CacheKey, TableName,PivotMode.CLIENT);
            }
            dynamic pivotResult = _pivotCacheRepository.GetAllDataFromCache(TableName);
            response.Data = pivotResult.Data;
            return response;
            //throw new NotImplementedException();
            //Implementation will go here.*/
            return null;
        }
        /*private async Task<string> GetData<T>(DataPivotRequest<T> request)
        {
            HttpClient client = new HttpClient();
            string jsonstring = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonstring, Encoding.UTF8, "application/JSON");
            var response = await client.PostAsync(request.DataSourceURL, httpContent);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }*/

        public DataPivotResponse<ICollection> CacheData<T>(ICollection data, DataPivotRequest<T> request)
        {
            var response = new DataPivotResponse<ICollection>();
            if (string.IsNullOrEmpty(request.GridGuid))
            {
                request.GridGuid = Guid.NewGuid().ToString();
            }
            //Now try to store the 
            try
            {
                _cache.Set(request.GridGuid, data, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromHours(8)
                });
                //response.Data = data;
                response.GridId = request.GridGuid;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Storing Grid cache data");
                response.State = ResponseState.Error;
                response.Messages.Add(e.Message);
            }

            return response;
        }
        private string GetOperator(string term)
        {
            switch (term)
            {
                case "equals":
                    return "=";
                case "notEqual":
                    return "!=";
                case "lessThan":
                    return "<";
                case "greaterThan":
                    return ">";
                default:
                    return "";
            }
        }

        private string GetDateOperator(string term, string field)
        {
            switch (term)
            {
                case "equals":
                    return $"{field} >= @0 AND {field} <= @1";
                case "notEqual":
                    return $"{field} != @0";
                case "lessThan":
                    return $"{field} < @0";
                case "greaterThan":
                    return $" {field} > @0";
                default:
                    return $"{field} >= @0 AND {field} <= @1";
            }
        }

        private string GetTextOperator(string term, string field)
        {

            switch (term)
            {
                case "contains":
                    return $"{field}.IndexOf(@0,@1) >= 0";
                case "notContains":
                    return $" {field}.IndexOf(@0,@1) < 0";
                case "equals":
                    return $"{field}.Equals(@0,@1)";
                case "notEqual":
                    return $"! {field}.Equals(@0,@1)";
                case "startsWith":
                    return $"{field}.Trim().StartsWith(@0,@1)";
                case "endsWith":
                    return $"{field}.Trim().EndsWith(@0,@1)";
                default:
                    return "";
            }
        }
        private IQueryable GenerateFilterQuery(ICollection data, List<ColumnFilter> filters)
        {
            var result = data.AsQueryable();

            /*foreach (var filter in filters)
            {
                switch (filter.FilterType)
                {
                    case "number":
                        result = result.Where($"{filter.FieldName} {GetOperator(filter.Operation)} @0",
                            filter.Filter[0]);
                        break;
                    case "set":

                        result = result.Where($" @0.Contains({filter.FieldName}==null?\"\":{filter.FieldName}.ToString()) ", filter.Filter);
                        break;
                    case "text":
                        if (filter.Filter[0] == " ")
                        {
                            filter.Filter[0] = "";
                        }
                        result = result.Where(GetTextOperator(filter.Operation, filter.FieldName), filter.Filter[0], StringComparison.OrdinalIgnoreCase);
                        break;
                    case "date":
                        var dateInfo = new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd" };
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                        DateTime fromDate = Convert.ToDateTime(filter.Filter[0], dateInfo);
                        var toDate = DateTime.Now;

                        if (!string.IsNullOrEmpty(filter.Filter[1]))
                        {
                            toDate = DateTime.Parse(filter.Filter[1], dateInfo);
                            toDate = toDate.AddHours(23);
                        }
                        else
                        {
                            toDate = fromDate.AddHours(23);
                        }
                        result = result.Where(GetDateOperator(filter.Operation, filter.FieldName), fromDate, toDate);
                        break;
                    case "repeatedData":
                        {
                            var tempSelect = result.GroupBy($"{filter.FieldName}", "it")
                                .Where($"it.Count() > {filter.Filter[0]} && it.key !=\"-\"").Select("it.key").ToDynamicArray();
                            result = result.Where($" @0.Contains({filter.FieldName}) ", string.Join(",", tempSelect));
                            break;
                        }
                }
            }*/
            return result;
        }
        private static IEnumerable<Dictionary<string, object>> Sort(IEnumerable<Dictionary<string, object>> data, string orderByString)
        {
            var orderBy =
               orderByString.Split(',').Select(
                  s => s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                  .Select(a => new { Field = a[0], Descending = "desc".Equals(a[1], StringComparison.InvariantCultureIgnoreCase) })
                  .ToList();
            if (orderBy.Count == 0)
                return data;
            // First one is OrderBy or OrderByDescending.
            IOrderedEnumerable<Dictionary<string, object>> ordered =
               orderBy[0].Descending ? data.OrderByDescending(d => d[orderBy[0].Field]) : data.OrderBy(d => d[orderBy[0].Field]);
            for (int i = 1; i < orderBy.Count; i++)
            {
                // Rest are ThenBy or ThenByDescending.
                var orderClause = orderBy[i];
                ordered =
                   orderBy[i].Descending ? ordered.ThenByDescending(d => d[orderClause.Field]) : ordered.ThenBy(d => d[orderClause.Field]);
            }
            return ordered;
        }
        public DataPivotResponse<ICollection> GetPivotData<T>(DataPivotRequest<T> request)
        {
            DataPivotResponse<ICollection> gridData = new DataPivotResponse<ICollection>();
            gridData.GridId = request.GridGuid;
            gridData.Data = new List<Dictionary<string, object>>();
            if (request.ColumnHeaders.Count > 0)
            {
                gridData.HeaderMetaData = _metaDataHelper.GetColumnsMetaData(request.ColumnHeaders);
            }
            if (!string.IsNullOrWhiteSpace(request.AlwaysGroupOnColumn))
            {
                if (request.RequestModel.rowGroupCols.Count > 0)
                {
                    if (request.RequestModel.rowGroupCols[request.RequestModel.rowGroupCols.Count - 1].field != request.AlwaysGroupOnColumn)
                        return gridData;
                }
                else
                    return gridData;
            }
            List<Dictionary<string, object>> data = _pivotRepository.ExtractDataFromCube(request);


            int PageIndex = (request.RequestModel.endRow) / (request.RequestModel.endRow - request.RequestModel.startRow);
            int PageSize = (request.RequestModel.endRow - request.RequestModel.startRow);



            var queryResult = data.AsEnumerable();
            if (request.RequestModel.sortModel != null && request.RequestModel.sortModel.Count > 0)
            {
                queryResult = Sort(queryResult, string.Join(",", request.RequestModel.sortModel.Select(s => s.ToString())));
            }
            /*if (request.RequestModel.sortModel != null && request.RequestModel.sortModel.Count > 0)
            {
                try
                {
                    queryResult = queryResult.OrderBy(string.Join(",", request.RequestModel.sortModel.Select(s => s.ToString())));
                }
                catch (Exception ex)
                {
                    //ignore as the grid layout might change and the previsouly-sorted-by column might not exist at all.
                }
            }*/
            if (queryResult.Count() > 0)
            {
                gridData.Data = queryResult.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToDynamicList();
                gridData.TotalDataCount = queryResult.Count();

            }
            else
            {
                gridData.Data = queryResult.ToDynamicList();
                gridData.TotalDataCount = queryResult.Count();

            }
            gridData.GridId = request.GridGuid;


            return gridData;
            /*Task<string> result = GetData(request);
            result.Wait();
            string data = result.Result;
            int PageIndex = (request.RequestModel.endRow) / (request.RequestModel.endRow - request.RequestModel.startRow);
            int PageSize = (request.RequestModel.endRow - request.RequestModel.startRow);
            //use the cache instead.
            DataPivotResponse<ICollection> gridData = JsonConvert.DeserializeObject<DataPivotResponse<ICollection>>(data);
            int a = 0;
            ICollection pivotData;
            if (_cache.TryGetValue(gridData.GridId, out pivotData))
            {
                var queryResult = GenerateFilterQuery(pivotData, request.RequestModel.filterModel);
                if (request.RequestModel.sortModel != null && request.RequestModel.sortModel.Count > 0)
                {
                    try
                    {
                        queryResult = queryResult.OrderBy(string.Join(",", request.RequestModel.sortModel.Select(s => s.ToString())));
                    }
                    catch (Exception ex)
                    {
                        //ignore as the grid layout might change and the previsouly-sorted-by column might not exist at all.
                    }
                }
                if (queryResult.Count() > 0)
                {
                    gridData.Data = queryResult.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToDynamicList();
                    gridData.TotalDataCount = queryResult.Count();
                    
                }
                else
                {
                    gridData.Data = queryResult.ToDynamicList();
                    gridData.TotalDataCount = queryResult.Count();
                    
                }
                gridData.GridId = request.GridGuid;

            }
            return gridData;*/
            //var response = new DataPivotResponse<ICollection>();
            //string CacheKey=_pivotCacheRepository.GetCacheKey(request.Filters,request.MDXProcedure);
            //string TableName = _pivotCacheRepository.GetCachTable(CacheKey,PivotMode.SERVER);
            //List<string> secondaryColumnFields = null;
            //if (TableName != null)
            //{
            //    secondaryColumnFields = _pivotCacheRepository.GetSecondaryColumns(TableName);
            //}
            //else
            //{
            //    DataTable data=_pivotRepository.GetDataFromDB(request.FieldToProcedureParamLookup, request.MDXProcedure);
            //    TableName = "pvt_" + Guid.NewGuid().ToString();
            //    PivotData pivotData=_pivotRepository.TransformData(data, request.RequestModel);
            //    secondaryColumnFields = pivotData.SecondaryColumns;
            //    _pivotCacheRepository.CachePivotData(TableName, pivotData.Data,pivotData.SecondaryColumns, request.RequestModel);
            //    _pivotCacheRepository.AddSecondaryColumns(TableName, secondaryColumnFields);
            //    _pivotCacheRepository.AddCachTable(CacheKey, TableName,PivotMode.SERVER);
            //}
            //dynamic pivotResult=_pivotCacheRepository.GetDataFromCache(TableName, secondaryColumnFields, request.RequestModel);
            //response.Data = pivotResult.Data;
            //response.TotlaDataCount = pivotResult.RowsCount;
            //response.GridId = TableName;
            //response.SecondaryColumns = secondaryColumnFields;
            //return response;

            /*var response = new DataPivotResponse<ICollection>();
            string CacheKey = _pivotSegmentCacheRepository.GetCacheKey(request.Filters, request.MDXProcedure,request.RequestModel);
            string CacheKeyAll = _pivotSegmentCacheRepository.GetCacheKey(request.Filters, request.MDXProcedure, request.FieldToProcedureParamLookup, request.RequestModel);

            string TableName = _pivotSegmentCacheRepository.GetCachTable(CacheKey, PivotMode.SERVER);
            string TableNameAll = _pivotSegmentCacheRepository.GetCachTable(CacheKeyAll, PivotMode.SERVER);

            List<string> secondaryColumnFields = null;
            if(TableNameAll==null)
            {
                DataTable data = _pivotRepository.GetDataFromDB(request.FieldToProcedureParamLookup, request.MDXProcedure);
                TableNameAll = "pvt_" + Guid.NewGuid().ToString();
                PivotData pivotData = _pivotRepository.TransformData(data, request.RequestModel);
                secondaryColumnFields = pivotData.SecondaryColumns;
                _pivotSegmentCacheRepository.CachePivotData(TableNameAll, pivotData.Data, pivotData.SecondaryColumns, request.RequestModel);
                _pivotSegmentCacheRepository.AddSecondaryColumns(TableNameAll, secondaryColumnFields);
                _pivotSegmentCacheRepository.AddCachTable(CacheKeyAll, TableNameAll, PivotMode.SERVER);
            }
            if (TableName != null)
            {
                secondaryColumnFields = _pivotSegmentCacheRepository.GetSecondaryColumns(TableName);
            }
            else
            {
                List<MDXParam> Params = new List<MDXParam>();
                if (request.RequestModel.pivotCols.Count > 0)
                {
                    List<MDXParam> res = request.FieldToProcedureParamLookup.Where(x => x.FieldName.Equals(request.RequestModel.pivotCols[0].field,StringComparison.CurrentCultureIgnoreCase)).ToList<MDXParam>();
                    Params.AddRange(res);
                }

                foreach(PivotColumn pc in request.RequestModel.rowGroupCols)
                {
                    List<MDXParam> param = request.FieldToProcedureParamLookup.Where(x => x.FieldName.Equals(pc.field, StringComparison.CurrentCultureIgnoreCase)).ToList<MDXParam>();
                        Params.AddRange(param);
                }
                DataTable data = _pivotRepository.GetDataFromDB(Params, request.MDXProcedure);
                TableName = "pvt_" + Guid.NewGuid().ToString();
                PivotData pivotData = _pivotRepository.TransformData(data, request.RequestModel);
                secondaryColumnFields = pivotData.SecondaryColumns;
                _pivotSegmentCacheRepository.CachePivotData(TableName, pivotData.Data, pivotData.SecondaryColumns, request.RequestModel);
                _pivotSegmentCacheRepository.AddSecondaryColumns(TableName, secondaryColumnFields);
                _pivotSegmentCacheRepository.AddCachTable(CacheKey, TableName, PivotMode.SERVER);
            }
            dynamic pivotResult = _pivotSegmentCacheRepository.GetDataFromCache(TableName, secondaryColumnFields, request.RequestModel);
            response.Data = pivotResult.Data;
            response.TotlaDataCount = pivotResult.RowsCount;
            response.GridId = TableNameAll;
            response.SecondaryColumns = secondaryColumnFields;
            return response;*/
            return null;

        }


        public DataPivotResponse<DataTable> GetPivotDataV2<T>(DataPivotRequest<object> request)
        {
            DataPivotResponse<DataTable> gridData = new DataPivotResponse<DataTable>();
            gridData.GridId = request.GridGuid;
            if ((request?.ColumnHeaders?.Count ?? 0) > 0)
            {
                gridData.HeaderMetaData = _metaDataHelper.GetColumnsMetaData(request.ColumnHeaders);
            }

            DataTable data = _pivotRepository.ExtractRawDataFromCube(this.cubeCacheServerUrl, request);

            gridData.Data = data;
            gridData.TotalDataCount = data?.Rows?.Count ?? 0;
            gridData.GridId = request.GridGuid;


            return gridData;
        }
        public OperationResponse<List<string>> GetSetFilter(FilterValuesRequest request)
        {
            OperationResponse<List<string>> response = new OperationResponse<List<string>>();
            List<string> filterVals = _pivotRepository.GetFilterValues(request);
            response.Data = filterVals.Distinct().ToList();
            return response;
        }
    }
}
