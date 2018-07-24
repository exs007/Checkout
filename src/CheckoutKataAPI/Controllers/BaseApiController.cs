using CheckoutKataAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Controllers
{
    [FluentValidationFilter]
    [ModelStateValidationFilter]
    [CustomExceptionFilter]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
