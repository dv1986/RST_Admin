using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCodeGenerator
{
    public class CodeGenerateRequest
    {
        public string ProcedureName { get; set; }
        public List<ProcedureParameter> Parameters { get; set; }
    }
}
