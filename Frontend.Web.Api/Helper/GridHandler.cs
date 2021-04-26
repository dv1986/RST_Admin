using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using Infrastructure.Grid;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RST.Shared;
using RST.Shared.Enums;

namespace Frontend.Web.Api.Helper
{
    public class GridHandler : IGridHandler
    {
        private IMemoryCache _cache;
        private ILogger<GridHandler> _logger;
        private MetaDataHelper _metaDataHelper;



        public GridHandler(IMemoryCache cache, ILogger<GridHandler> logger, MetaDataHelper metaDataHelper)
        {
            _cache = cache;
            _logger = logger;
            _metaDataHelper = metaDataHelper;

        }


        public DataGridResponse<ICollection> CacheData<T>(ICollection data, DataGridRequest<T> request)
        {
            var response = new DataGridResponse<ICollection>();
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
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Storing Grid cache data");
                response.State = ResponseState.Error;
                response.Messages.Add(e.Message);
            }

            if (response.State == 0)
            {
                return GetGridData(request);
            }
            return response;
        }

        public DataGridResponse<ICollection> GetGridData<T>(DataGridRequest<T> request)
        {
            var response = new DataGridResponse<ICollection>();
            ICollection data;
            List<SummaryColumn> SummaryColumns = new List<SummaryColumn>();
            if (_cache.TryGetValue(request.GridGuid, out data))
            {
                if (data != null)
                {
                    if (request.ColumnHeaders.Count > 0)
                    {
                        response.HeaderMetaData = _metaDataHelper.GetColumnsMetaData(request.ColumnHeaders);
                       
                        foreach(ColumnMetaData metaData in response.HeaderMetaData)
                        {
                            if(!string.IsNullOrEmpty(metaData.Aggregate))
                            {
                                SummaryColumns.Add(new SummaryColumn() { ColumnName = metaData.Key, Operation = metaData.Aggregate,Field=metaData.Field });
                            }
                        }
                        if(SummaryColumns.Count>0)
                            SummaryColumns.Add(new SummaryColumn() { ColumnName = "#", Operation = "count",Field= "index" }); 
                    }
                    var queryResult = GenerateFilterQuery(data, request.ColumnFilters);
                    List<GridSummary> gridSummary = GetGridSummary(queryResult, SummaryColumns);
                    if (request.SortColumns != null && request.SortColumns.Count > 0)
                    {
                        try
                        {
                            queryResult = queryResult.OrderBy(string.Join(",", request.SortColumns.Select(s => s.ToString())));
                        }
                        catch (Exception ex)
                        {
                            //ignore as the grid layout might change and the previsouly-sorted-by column might not exist at all.
                        }
                    }
                    if (queryResult.Count() > 0)
                    {
                        response.Data = queryResult.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToDynamicList();
                        response.TotlaDataCount = queryResult.Count();
                        response.Summary = gridSummary;
                    }
                    else
                    {
                        response.Data = queryResult.ToDynamicList();
                        response.TotlaDataCount = queryResult.Count();
                        response.Summary = gridSummary;
                    }
                    response.GridGuid = request.GridGuid;

                    
                }
            }
            else
            {
                response.State = ResponseState.DataExpired;
            }
            return response;
        }


        public OperationResponse<bool> UpdateGridData(string gridGuid, dynamic dataToUpdate, string keyField)
        {

            var response = new OperationResponse<bool>();
            ICollection data;

            if (_cache.TryGetValue(gridGuid, out data))
            {

                var type = data.ToDynamicList()[0].GetType();
                var objectAsString = JsonConvert.SerializeObject(dataToUpdate);
                var newData = JsonConvert.DeserializeObject(objectAsString, type);
                var objValue = newData.GetType().GetProperty(keyField).GetValue(newData, null);
                //var item = data.ToDynamicList()
                //    .FirstOrDefault(o => o.GetType().GetProperty(keyField).GetValue(o, null) == objValue);
                var found = false;
                foreach (var item in data)
                {
                    if (item.GetType().GetProperty(keyField).GetValue(item, null).ToString() == objValue.ToString())
                    {
                        found = true;
                        foreach (var property in item.GetType().GetProperties())
                        {
                            var val = newData.GetType().GetProperty(property.Name).GetValue(newData, null);
                            if (property.CanWrite && !property.Name.Equals(keyField))
                                property.SetValue(item, val);
                        }
                        break;
                    }

                }
                _cache.Set(gridGuid, data);
                response.Data = true;

            }
            else
            {
                response.State = ResponseState.DataExpired;
            }

            return response;
        }


        public OperationResponse<ICollection> GetSetFilter(string gridGuid, string fieldName)
        {
            var response = new OperationResponse<ICollection>();
            ICollection data;
            if (_cache.TryGetValue(gridGuid, out data))
            {
                if (data != null)
                {
                    response.Data = data.AsQueryable().Select($"{fieldName}==null?\"\":{fieldName}.ToString()").Distinct().ToDynamicList();
                }
            }
            else
            {
                response.State = ResponseState.DataExpired;
            }

            return response;
        }

        private string FirstCharacterToLower(string str)
        {
            if (String.IsNullOrEmpty(str) || Char.IsLower(str, 0))
                return str;

            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        private string GenerateFilterQuery(List<ColumnFilter> filters, ref List<dynamic> paramsList)
        {
            var query = new StringBuilder(" 1=1 ");
            var counter = 0;
            foreach (var filter in filters)
            {
                switch (filter.FilterType)
                {
                    case "number":
                        break;
                    case "set":
                        string test = filter.Filter[1];
                        query.Append($"AND ({filter.FieldName} = @{counter}) ");
                        paramsList.Add(test);
                        break;
                }
            }
            return query.ToString();
        }


        private IQueryable GenerateFilterQuery(ICollection data, List<ColumnFilter> filters)
        {
            var result = data.AsQueryable();

            foreach (var filter in filters)
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
            }
            return result;
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
        private List<GridSummary>  GetGridSummary(IQueryable data,List<SummaryColumn> SummaryColumns)
        {
            List<GridSummary> GridSummary = new List<GridSummary>();
            if (SummaryColumns!=null && SummaryColumns.Count > 0)
            {

                foreach (SummaryColumn column in SummaryColumns)
                {
                    if(column.ColumnName=="#")
                    {
                        GridSummary.Add(new GridSummary() { ColumnName = column.ColumnName, AggVal = data.Count(),Field=column.Field,Aggregate="count" });
                        continue;
                    }
                    if (column.Operation == "sum")
                    {
                        try
                        {
                            decimal total=GetTotal(data, column.Field);
                            GridSummary.Add(new GridSummary() { ColumnName = column.ColumnName, AggVal = total,Aggregate=column.Operation,Field=column.Field});
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (column.Operation == "average")
                    {
                        try
                        {
                            decimal total = (GetTotal(data, column.Field) / Convert.ToDecimal(data.Select(column.Field).Count()));
                            GridSummary.Add(new GridSummary() { ColumnName = column.ColumnName, AggVal = total, Aggregate = column.Operation, Field = column.Field });
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (column.Operation == "count")
                    {
                        try
                        {
                            decimal total = Convert.ToDecimal(data.Select(column.Field).Count());
                            GridSummary.Add(new GridSummary() { ColumnName = column.ColumnName, AggVal = total, Aggregate = column.Operation, Field = column.Field });
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return GridSummary;
        }
        private decimal GetTotal(IQueryable data,string field)
        {
            var dataList = data.Select(field).AsEnumerable().ToList();
            decimal sum = 0;
            foreach (var val in dataList)
            {
                if (val != null && !string.IsNullOrEmpty(Convert.ToString(val)))
                {
                    sum += Convert.ToDecimal(val);
                }
            }
            return sum;
        }
        public List<T> GetGridDataFromCache<T>(string gridGuid)
        {
            List<T> data;
            if (_cache.TryGetValue(gridGuid, out data))
            {
                return data;
            }
            return null;
        }

        public List<ColumnMetaData> GetColumnMetaData(List<ColumnHeader> columnHeaders)
        {
            return _metaDataHelper.GetColumnsMetaData(columnHeaders);
        }


        public void UpdateColumnMetaData(List<ColumnMetaData> data)
        {
            _metaDataHelper.SetColumnsMetaData(data);
        }

        public string GetGridState(string userName, string gridKey, IDataContext conext)
        {
            return _metaDataHelper.GetGridState(userName, gridKey, conext);
        }

        public void SetGridState(string userName, string gridKey, string state, IDataContext context)
        {
            _metaDataHelper.SetGridState(userName, gridKey, state, context);
        }
        public bool ResetGridState(string userName, string gridKey, IDataContext context)
        {
            return _metaDataHelper.ResetGridState(userName, gridKey, context);
        }
        public void RefreshMetaData()
        {
            _metaDataHelper.RefreshMetaData();
        }

        public ICollection<dynamic> GetCachedData(string dataCacheId, List<ColumnFilter> filters, List<SortColumn> sortColumns)
        {
            ICollection data;
            if (_cache.TryGetValue(dataCacheId, out data))
            {
                if (data != null)
                {
                    var queryResult = GenerateFilterQuery(data, filters);
                    if (sortColumns != null && sortColumns.Count > 0)
                    {
                        queryResult = queryResult.OrderBy(string.Join(",", sortColumns.Select(s => s.ToString())));
                    }
                    return queryResult.ToDynamicList();
                }
            }

            return null;
        }

        public void SetCacheData(string gridCacheId, ICollection data)
        {
            _cache.Set(gridCacheId, data, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal,
                SlidingExpiration = TimeSpan.FromHours(8)
            });
        }

        public void UpdateCache<T>(string cacheId, List<T> data, Func<T,T,bool> indexFinderPredicate)
        {
            var cacheLine
                = GetGridDataFromCache<T>(cacheId);
            if (cacheLine != null && cacheLine.Count > 0)
            {
                foreach (var item in data)
                {
                    var foundBinIndex = cacheLine.FindIndex(x=>indexFinderPredicate(x, item));
                    if (foundBinIndex != -1)
                    {
                        cacheLine[foundBinIndex] = item;
                    }
                }
                SetCacheData(cacheId, cacheLine);
            }
        }

        public void UpdateCache<T>(string cacheId, T data, Func<T, T, bool> indexFinderPredicate)
        {
            var cacheLine = GetGridDataFromCache<T>(cacheId);
            if (cacheLine != null && cacheLine.Count > 0)
            {
                var foundBinIndex = cacheLine.FindIndex(x=>indexFinderPredicate(x, data));
                if (foundBinIndex != -1)
                {
                    cacheLine[foundBinIndex] = data;
                }
                SetCacheData(cacheId, cacheLine);
            }
        }

        
    }
}