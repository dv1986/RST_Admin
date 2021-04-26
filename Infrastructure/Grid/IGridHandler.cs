using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Infrastructure.Repository;
using RST.Shared;

namespace Infrastructure.Grid
{
    public interface IGridHandler
    {
        DataGridResponse<ICollection> CacheData<T>(ICollection data, DataGridRequest<T> request);

        DataGridResponse<ICollection> GetGridData<T>(DataGridRequest<T> request);

        OperationResponse<bool> UpdateGridData(string gridGuid, dynamic dataToUpdate, string keyField);

        OperationResponse<ICollection> GetSetFilter(string gridGuid, string fieldName);
        List<T> GetGridDataFromCache<T>(string gridGuid);
        void UpdateCache<T>(string gridGuid, List<T> data, Func<T, T, bool> indexFinderPredicate);
        void UpdateCache<T>(string cacheId, T data, Func<T, T, bool> indexFinderPredicate);
        List<ColumnMetaData> GetColumnMetaData(List<ColumnHeader> columnHeaders);
        void UpdateColumnMetaData(List<ColumnMetaData> data);
        string GetGridState(string userName, string gridKey, IDataContext conext);
        bool ResetGridState(string userName, string gridKey, IDataContext conext);
        void SetGridState(string userName, string gridKey, string state, IDataContext conext);

        ICollection<dynamic> GetCachedData(string dataCacheId, List<ColumnFilter> filters, List<SortColumn> sortColumns);

        void SetCacheData(string gridCacheId, ICollection data);
        void RefreshMetaData();
    }
}