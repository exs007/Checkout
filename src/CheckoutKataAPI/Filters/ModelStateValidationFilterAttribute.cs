using System.Linq;
using CheckoutKataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CheckoutKataAPI.Filters
{
    /// <summary>
    /// Generate common request structure for the model state errors
    /// </summary>
    public class ModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new Result<object>(false);
                foreach (var keyValue in context.ModelState)
                {
                    if (keyValue.Value.Errors != null && keyValue.Value.Errors.Count > 0)
                    {
                        var messages = keyValue.Value.Errors.Where(p => !string.IsNullOrEmpty(p.ErrorMessage))
                            .Select(p => new MessageInfo()
                            {
                                Field = keyValue.Key,
                                Message = p.ErrorMessage
                            });
                        result.AddMessages(messages);
                    }
                }

                context.Result = new JsonResult(result);
            }
        }
    }
}