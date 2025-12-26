
using MyEccommerce.Route.Domain.Exceptions.Auth;
using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;

namespace MyEccommerce.Route.Web.HandleErrors
{
    public class GlobalErrorHandling
    {
        private readonly ILogger<GlobalErrorHandling> _logger;
        private readonly RequestDelegate _next;

        public GlobalErrorHandling(ILogger<GlobalErrorHandling> logger,RequestDelegate next)
        {
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context) {

            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Response.ContentType = "application/json";
                    var Response = new ErrorDetails()
                    {
                        ErrorMessage = $"Path: {context.Request.Path} is not found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                  await  context.Response.WriteAsJsonAsync(Response);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";

                var Response=new ErrorDetails() { ErrorMessage = ex.Message };

                Response.StatusCode = ex switch
                {
                    BadRequestException => StatusCodes.Status500InternalServerError,
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    ForbiddenException=>StatusCodes.Status403Forbidden,
                    AlreadyExistException=>StatusCodes.Status409Conflict,
                    ValidationException => HandleValidationError((ValidationException)ex, Response),
                    _ => StatusCodes.Status500InternalServerError
                };
                context.Response.StatusCode = Response.StatusCode;
                await context.Response.WriteAsJsonAsync(Response);
            }
        
        
        }

        private int HandleValidationError(ValidationException ex,ErrorDetails errorDetails)
        {
           errorDetails.Errors= ex.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
    
}
