using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace api.Filter
{
    public class LogResultAttribute : ActionFilterAttribute
    {
       
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var result = objectResult.Value;
                Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} returned data: {result}");
            }
            else
            {
                Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} returned non-object result.");
            }

            base.OnActionExecuted(context);
        }
    }
}
