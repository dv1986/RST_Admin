using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHelper
{
    public class BaseService
    {
        public virtual ServiceResponse<T> SetResultStatus<T>(T objData, IEnumerable<T> objLstData, string Message, bool IsSuccess) where T : class
        {
            ServiceResponse<T> objReturn = new ServiceResponse<T>();
            objReturn.Data = objData;
            //objReturn.LstData = objLstData;
            objReturn.Message = Message;
            objReturn.IsSuccess = IsSuccess;
            return objReturn;
        }
    }
}
