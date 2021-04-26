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

namespace RST.Admin.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [Route("AddCountry")]
        public IActionResult AddCountry([FromBody] Country request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _lookupService.AddCountry(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddCountry ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateCountry")]
        public IActionResult UpdateCountry([FromBody] CountryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.UpdateCountry(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateCountry ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteCountry")]
        public IActionResult DeleteCountry([FromBody] CountryDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.DeleteCountry(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in DeleteCountry ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


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
        [Route("AddState")]
        public IActionResult AddState([FromBody] State request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _lookupService.AddState(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddState ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateState")]
        public IActionResult UpdateState([FromBody] StateDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.UpdateState(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateState ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteState")]
        public IActionResult DeleteState([FromBody] StateDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.DeleteState(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in DeleteState ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


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
        [Route("AddCity")]
        public IActionResult AddCity([FromBody] City request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _lookupService.AddCity(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddCity ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateCity")]
        public IActionResult UpdateCity([FromBody] CityDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.UpdateCity(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateCity ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteCity")]
        public IActionResult DeleteCity([FromBody] CityDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.DeleteCity(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in DeleteCity ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


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

        #region Menu
        [HttpPost]
        [Route("AddMenu")]
        public IActionResult AddMenu([FromBody] Menu request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _lookupService.AddMenu(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddMenu ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateMenu")]
        public IActionResult UpdateMenu([FromBody] MenuDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.UpdateMenu(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateMenu ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteMenu")]
        public IActionResult DeleteMenu([FromBody] MenuDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.DeleteMenu(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in DeleteMenu ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetMenu")]
        public IActionResult GetMenu(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetMenu(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetMenu==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

        #region SubMenu
        [HttpPost]
        [Route("AddSubMenu")]
        public IActionResult AddSubMenu([FromBody] SubMenu request)
        {
            var response = new OperationResponse<bool>();
            try
            {
                response.Data = _lookupService.AddSubMenu(request);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in AddSubMenu ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("UpdateSubMenu")]
        public IActionResult UpdateSubMenu([FromBody] SubMenuDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.UpdateSubMenu(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in UpdateSubMenu ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DeleteSubMenu")]
        public IActionResult DeleteSubMenu([FromBody] SubMenuDTO request)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                var result = _lookupService.DeleteSubMenu(request.Tasks);
                if (result.Any(fn => !string.IsNullOrEmpty(fn.Message)))
                {
                    response.State = ResponseState.ValidationError;
                    response.Data = result.ToList();
                    return new JsonResult(response);
                }
                else
                    response.State = ResponseState.Success;
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message);
                _logger.LogError(exception, "Error in DeleteSubMenu ==>" + exception.StackTrace, request);
            }
            return new JsonResult(response);
        }


        [HttpPost]
        [Route("GetSubMenu")]
        public IActionResult GetSubMenu(string SearchStr)
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetSubMenu(SearchStr);
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetSubMenu==>" + exception.StackTrace, SearchStr);
            }
            return new JsonResult(response);
        }
        #endregion

        #region Subscription
        [HttpPost]
        [Route("GetSubscriptionList")]
        public IActionResult GetSubscriptionList()
        {
            var response = new OperationResponse<ICollection>();
            try
            {
                response.Data = _lookupService.GetSubscriptionList();
            }
            catch (Exception exception)
            {
                response.State = ResponseState.Error;
                response.Messages.Add(exception.Message + " StackTrace==> " + exception.StackTrace);
                _logger.LogError(exception, "Error Getting GetSubscriptionList==>" + exception.StackTrace);
            }
            return new JsonResult(response);
        }
        #endregion
    }
}
