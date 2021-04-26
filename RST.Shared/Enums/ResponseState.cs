using System;
using System.Collections.Generic;
using System.Text;

namespace RST.Shared.Enums
{
    public enum ResponseState
    {
        Success = 0,
        SuccessChange = 1,
        Warning = 2,
        Error = 3,
        ValidationError = 4,
        DataExpired = 5,
        NoOperation = 6
    }
}
