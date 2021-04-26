using RST.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RST.Shared
{
    public class OperationResponse<T>
    {
        public ResponseState State { get; set; } = ResponseState.Success;

        public List<string> Messages { get; set; } = new List<string>();

        public T Data { get; set; }

        //public IEnumerable<T> DataLst { get; set; }
    }
}
