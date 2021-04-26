using RST.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Infrastructure.Pivot
{
    public interface IPivotHandler
    {
        DataPivotResponse<ICollection> CacheData<T>(ICollection data, DataPivotRequest<T> request);
        DataPivotResponse<ICollection> GetPivotData<T>(DataPivotRequest<T> request);
        DataPivotResponse<DataTable> GetPivotDataV2<T>(DataPivotRequest<object> request);
        DataPivotResponse<ICollection> GetPivotAllData<T>(DataPivotRequest<T> request);
        OperationResponse<List<string>> GetSetFilter(FilterValuesRequest request);
    }
}
