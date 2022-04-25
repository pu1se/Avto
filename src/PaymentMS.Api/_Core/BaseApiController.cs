using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PaymentMS.Api.Controllers
{
    [Authorize]
    public abstract class BaseApiController : Controller
    {
        //implement later: move HttpResponse to middle-ware
        protected async Task<IActionResult> HttpResponse<TResult>(Func<Task<CallListDataResult<TResult>>> serviceMethod)
        {
            var result = await serviceMethod();

            if (!result.IsSuccess)
                return ErrorDescription(result);

            return Ok(result.Data);
        }

        protected async Task<IActionResult> HttpResponse<TResult>(Func<Task<CallDataResult<TResult>>> serviceMethod)
        {
            var result = await serviceMethod();

            if (!result.IsSuccess)
                return ErrorDescription(result);

            return Ok(result.Data);
        }

        protected async Task<IActionResult> HttpResponse(Func<Task<CallResult>> serviceMethod)
        {
            var result = await serviceMethod();
            if (!result.IsSuccess)
                return ErrorDescription(result);

            return Ok();
        }

        private IActionResult ErrorDescription(CallResult result)
        {
            if (result.ErrorType == ErrorType.ValidationError400 || result.ErrorType == ErrorType.ModelWasNotInitialized400)
                return BadRequest(result.ValidationErrors);

            if (result.ErrorType == ErrorType.NotFoundError404)
                return NotFound(result.ErrorMessage);

            if (result.ErrorType == ErrorType.ThirdPartyApiError417)
                return StatusCode(417, result.ErrorMessage);

            return StatusCode(500, result.ErrorMessage);
        }

        protected string GetBaseHostUrl()
        {
            return $"{Request.Scheme}://{Request.Host.Value}";
        }
    }
}
