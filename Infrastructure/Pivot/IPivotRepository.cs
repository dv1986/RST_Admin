using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.Pivot
{
    public interface IPivotRepository
    {
        List<string> GetFilterValues(FilterValuesRequest request);
        List<Dictionary<string, object>> ExtractDataFromCube<T>(DataPivotRequest<T> request);
        DataTable ExtractRawDataFromCube<T>(string cubeCacheServerUrl, DataPivotRequest<T> request);
        /*DataTable GetDataFromDB(List<MDXParam> QueryParamters, string storeProcedure);
        PivotData TransformData(DataTable ResultData, PivotRequestModel request);*/
    }
}
