using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Cache;
using Infrastructure.Grid;
using Infrastructure.MetaData;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Frontend.Web.Api.Helper
{
    public class MetaDataHelper
    {
        private IMetaDataRepository _metaDataRepository;
        private IMemoryCache _cache;
        private Object thisLock = new Object();

        public MetaDataHelper(IMetaDataRepository metaDataRepository, IMemoryCache cache)
        {
            _metaDataRepository = metaDataRepository;
            _cache = cache;

        }

        public List<ColumnMetaData> GetColumnsMetaData(List<ColumnHeader> columnHeaders)
        {
            List<ColumnMetaData> metaDataAllList = null;
            lock (thisLock)
            {
                metaDataAllList = _cache.Get<List<ColumnMetaData>>(CacheKeys.TitleMetaData.ToString());
                if (metaDataAllList == null || metaDataAllList.Count == 0)
                {
                    metaDataAllList = _metaDataRepository.GetColumnsMetaData();
                    _cache.Set(CacheKeys.TitleMetaData.ToString(), metaDataAllList, new MemoryCacheEntryOptions
                    {
                        Priority = CacheItemPriority.High,
                        AbsoluteExpiration = DateTimeOffset.Now.AddDays(4)
                    });
                }
            }
            var result = new List<ColumnMetaData>();
            foreach (var columnHeader in columnHeaders)
            {
                var metaData = metaDataAllList.FirstOrDefault(m => m.Key == columnHeader.ColumnName.Trim());
                if (metaData == null)
                {
                    metaData = new ColumnMetaData { Key = columnHeader.ColumnName, Name = columnHeader.ColumnName, Title = "" };
                }
                if (string.IsNullOrEmpty(metaData.Name))
                {
                    metaData.Name = columnHeader.ColumnName;
                }
                metaData.Field = columnHeader.Field;
                //if (!result.Any(fn => fn.ColumnName == metaData.ColumnName && fn.Field == metaData.Field))
                     result.Add(metaData);
            }
            return result;
        }

        public void SetColumnsMetaData(List<ColumnMetaData> data)
        {

            _metaDataRepository.UpdateColumnMetaData(data);
            //Now reset the cache
            _cache.Remove(CacheKeys.TitleMetaData.ToString());
        }
        public void RefreshMetaData()
        {
            lock (thisLock)
            {
                _cache.Remove(CacheKeys.TitleMetaData.ToString());
                List<ColumnMetaData> metaDataAllList = _metaDataRepository.GetColumnsMetaData();
                _cache.Set(CacheKeys.TitleMetaData.ToString(), metaDataAllList, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.High,
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(4)
                });
            }
        }
        public string GetGridState(string userName, string gridKey, IDataContext context)
        {
            return _metaDataRepository.GetGridState(userName, gridKey, context);
        }
        public bool ResetGridState(string userName, string gridKey, IDataContext context)
        {
            return _metaDataRepository.ResetGridState(userName, gridKey, context);
        }
        public void SetGridState(string userName, string gridKey, string columnsState, IDataContext context)
        {
            _metaDataRepository.SetGridState(userName, gridKey, columnsState, context);
        }
    }
}