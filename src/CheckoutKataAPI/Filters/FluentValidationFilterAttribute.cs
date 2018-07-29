using System;
using System.Data.SqlClient;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CheckoutKataAPI.Exceptions;
using FluentValidation;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Filters
{
    /// <summary>
    /// Execute fluent validators based by a model type and add errors in the model state
    /// </summary>
    public class FluentValidationFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var factory = context.HttpContext.RequestServices.GetService<IValidatorFactory>();

            foreach (var item in context?.ActionArguments)
            {
                if (item.Value == null)
                    continue;

                var validator = factory.GetValidator(item.Value.GetType());

                if (validator == null)
                    continue;

                var model = await validator.ValidateAsync(item.Value);

                if (!model.IsValid)
                {
                    //Set all errors in the model state for ability to handle errors from attribute based validation
                    foreach (var validationError in model.Errors)
                    {
                        context.ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            await next();
        }
    }
}