using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RST.Shared;
using RST.Shared.Enums;
using ServiceDemo;

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        IDemoService _demoService;
        public DemoController(IDemoService demoservice)
        {
            _demoService = demoservice;
        }

        [HttpPost]
        [Route("GetAll")]
        //[AllowAnonymous]
        public IActionResult GetAll(string searchString)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _demoService.GetAll(searchString);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
            }
            return new JsonResult(response);
        }
    }
}
