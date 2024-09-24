using ASPNETCoreWithHeadersMiddleware.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPNETCoreWithHeadersMiddleware.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var errors = context.ModelState.Values.Aggregate(string.Empty,
                (current, x) => x.Errors.Aggregate(current, (current1, y) => current1 + (y.ErrorMessage)));
            throw new ApiException(StatusCodes.Status400BadRequest, errors);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
