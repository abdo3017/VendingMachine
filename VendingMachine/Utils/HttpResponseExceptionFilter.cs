using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data;

namespace VendingMachineAPI.Utils
{
    public class HttpResponseExceptionFilter : ExceptionFilterAttribute
    {
        private bool isFullTrace = false;

        public HttpResponseExceptionFilter(IConfiguration configuration)
        {
            if (configuration["FullTrace"] == "1")
                isFullTrace = true;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is Exception)
            {
                var GenericResponse = new ErrorResponse();
                GenericResponse.error = new Error("500", new Message(context.Exception.Message + context.Exception.InnerException?.Message));
                Log.Logger.Error("internal server error with msg", GenericResponse.error);

                if (isFullTrace)
                {
                    GenericResponse.innerError = new InnerError(context.Exception.StackTrace);
                    Log.Logger.Error("internal server error with details", GenericResponse.innerError);

                }

                context.Result = new ObjectResult(GenericResponse)
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;
            }
        }
    }

}
