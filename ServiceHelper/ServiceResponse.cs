using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHelper
{
    public class ServiceResponse<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        //public IEnumerable<T> LstData { get; set; }
    }
}
