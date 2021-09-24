using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace UpSurgeApp.API.Helpers
{
    public static class ModelStateExt
    {
        public static IEnumerable<string> GetErrorList(this ModelStateDictionary modelState)
        {
            return from state in modelState.Values
                   from error in state.Errors
                   select error.ErrorMessage;
        }
    }
}
