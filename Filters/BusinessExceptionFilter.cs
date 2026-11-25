using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CharacterHub.Filters;

public class BusinessExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ArgumentException argEx:
                context.Result = new BadRequestObjectResult(new { error = argEx.Message });
                context.ExceptionHandled = true;
                break;

            case KeyNotFoundException notFoundEx:
                context.Result = new NotFoundObjectResult(new { error = notFoundEx.Message });
                context.ExceptionHandled = true;
                break;
        }
    }
}
