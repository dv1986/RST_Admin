using ModelCodeGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServiceCodeGenerator
{
    public interface ICodeGeneratorService
    {
        bool SpExists(string SPName);

        List<ProcedureParameter> GetSpParameters(string SPName);
        List<OutputColumn> GetOutPutColumns(string SPName, List<ProcedureParameter> procedureParameters);
        DbType GetDbType(string type);
        Type GetType(DbType type);
        GeneratedCode GenerateCode(string ProcedureName, List<ProcedureParameter> ProcedureParameters);
        string GenerateDataInsertFunction(string SPName);
        string GenerateDataUpdateFunction(string SPName);
        string GenerateDataDeleteFunction(string SPName);
    }
}
