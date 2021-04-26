using Infrastructure.Grid;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.MetaData
{
    public interface IMetaDataRepository
    {
        List<ColumnMetaData> GetColumnsMetaData();

        void UpdateColumnMetaData(List<ColumnMetaData> data);

        void SetGridState(string userName, string gridKey, string columnsState, IDataContext conext);

        bool ResetGridState(string userName, string gridKey, IDataContext conext);
        string GetGridState(string userName, string gridKey, IDataContext conext);
    }
}
