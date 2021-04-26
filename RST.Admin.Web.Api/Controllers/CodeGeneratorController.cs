using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelCodeGenerator;
using RST.Shared;
using RST.Shared.Enums;
using ServiceCodeGenerator;

namespace RST.Admin.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class CodeGeneratorController : Controller
    {
        private ILogger<LookupController> _logger;
        private ICodeGeneratorService codeGeneratorService;
        public CodeGeneratorController(ICodeGeneratorService codeGeneratorService, ILogger<LookupController> logger)
        {
            this.codeGeneratorService = codeGeneratorService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("GetProcedureParameters")]
        public IActionResult GetProcedureParameters(string ProcedureName)
        {
            var response = new OperationResponse<List<ProcedureParameter>>();
            try
            {
                if (codeGeneratorService.SpExists(ProcedureName))
                {
                    response.Data = codeGeneratorService.GetSpParameters(ProcedureName);
                }
                else
                {
                    response.Data = null;
                    response.State = ResponseState.Error;
                    response.Messages.Add("Sorry the procedure does not exist");
                }
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error Getting Procedure Params");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("GenerateCode")]
        public IActionResult GenerateCode([FromBody] CodeGenerateRequest request)
        {
            var response = new OperationResponse<GeneratedCode>();
            try
            {
                response.Data = codeGeneratorService.GenerateCode(request.ProcedureName, request.Parameters);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error Getting Procedure Params");
            }

            return Json(response);
        }

        [HttpGet]
        [Route("GenerateDataInsertFunction")]
        public IActionResult GenerateDataInsertFunction(string ProcedureName, string type)
        {
            var response = new OperationResponse<string>();
            try
            {
                if (codeGeneratorService.SpExists(ProcedureName))
                {
                    if (type == "I")
                        response.Data = codeGeneratorService.GenerateDataInsertFunction(ProcedureName);
                    if (type == "U")
                        response.Data = codeGeneratorService.GenerateDataUpdateFunction(ProcedureName);
                    if (type == "D")
                        response.Data = codeGeneratorService.GenerateDataDeleteFunction(ProcedureName);
                }
                else
                {
                    response.Data = null;
                    response.State = ResponseState.Error;
                    response.Messages.Add("Sorry the procedure does not exist");
                }
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error Getting Procedure Params");
            }

            return Json(response);
        }
    }
}
