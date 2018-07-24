using System;
using System.Collections.Generic;
using System.Linq;
using CheckoutKataAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CheckoutKataAPI.Filters
{
    public static class ResultHelper
    {
        public static Result<T> CreateErrorResult<T>(string errorMessage, T data = default(T))
        {
            var result = new Result<T>(false, data);
            result.AddMessage(null, errorMessage);
            return result;
        }

        public static Result<T> CreateErrorResult<T>(Exception error, T data = default(T))
        {
            var result = new Result<T>(false, data);
            result.AddMessage(error.GetType().Name, error.Message);
            return result;
        }

        public static Result<T> CreateErrorResult<T>(IEnumerable<MessageInfo> messages, T data = default(T))
        {
            var result = new Result<T>(false, data);
            foreach (var messageInfo in messages)
            {
                result.AddMessage(messageInfo.Field, messageInfo.Message);
            }

            return result;
        }

        public static Result<T> CreateErrorResult<T>(ModelStateDictionary modelState, T data = default(T))
        {
            var result = new Result<T>(false, data);
            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        result.AddMessage(state.Key, error.ErrorMessage);
                    }
                    else if (error.Exception != null)
                    {
                        result.AddMessage(state.Key, error.Exception.Message);
                    }
                }
            }
            modelState.Clear();
            return result;
        }

        public static Result<T> CreateSuccessResult<T>(T data = default(T))
        {
            return new Result<T>(true, data);
        }
    }
}