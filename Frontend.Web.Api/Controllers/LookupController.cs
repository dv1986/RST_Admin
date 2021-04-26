using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLookup;
using RST.Shared;
using RST.Shared.Enums;
using ServiceLookup;

namespace Frontend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class LookupController : Controller
    {
        ILookupService _lookupService;
        private ILogger<LookupController> _logger;
        public LookupController(ILookupService lookupService, ILogger<LookupController> logger)
        {
            _lookupService = lookupService;
            _logger = logger;
        }

        #region Country
        [HttpPost]
        [Route("GetCountry")]
        public IActionResult GetCountry(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetCountry(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetCountry==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetCountryLookup")]
        public IActionResult GetCountryLookup(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetCountryLookup(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetCountryLookup==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

        #region State
        [HttpPost]
        [Route("GetState")]
        public IActionResult GetState(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetState(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetState==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetStateLookup")]
        public IActionResult GetStateLookup(int CountryId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetStateLookup(CountryId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetStateLookup==>" + exception.StackTrace, CountryId);
            }
            return new JsonResult(response);
        }
        #endregion

        #region City
        [HttpPost]
        [Route("GetCity")]
        public IActionResult GetCity(int CountryId = 0, int StateId = 0)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetCity(CountryId, StateId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetCity==>" + exception.StackTrace, CountryId, StateId);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("GetCityLookup")]
        public IActionResult GetCityLookup(int StateId)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetCityLookup(StateId);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetCityLookup==>" + exception.StackTrace, StateId);
            }
            return new JsonResult(response);
        }

        #endregion
    }
}
