using System;
using System.Data.SqlClient;
using System.Net;
using CheckoutKataAPI.Constants;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CheckoutKataAPI.Exceptions;

namespace CheckoutKataAPI.Filters
{
    /// <summary>
    /// Common exception handling. Send 200 with an exception messages for business rules
    /// </summary>
    //TODO: add critical exception logging
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            IActionResult result = null;

            switch (context.Exception)
            {
                case AppValidationException exception:
                    result = new JsonResult(ResultHelper.CreateErrorResult<object>(exception.Messages))
                    {
                        StatusCode = (int) HttpStatusCode.OK
                    };
                    break;
                default:
                    result = new JsonResult(ResultHelper.CreateErrorResult<object>(MessageConstants.DEFAULT_ERROR_MESSAGE))
                    {
                        StatusCode = (int) HttpStatusCode.InternalServerError
                    };
                    break;
            }
            if (result != null)
            {
                context.Result = result;
            }
        }
    }
}