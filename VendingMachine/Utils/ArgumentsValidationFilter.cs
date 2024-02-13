using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using System.Text;

namespace VendingMachineAPI.Utils
{
    public class ArgumentsValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            StringBuilder metaDataErrors = new StringBuilder();
            StringBuilder modelBindingException = new StringBuilder();
            if (context.HttpContext.Request.Method == "POST"
                || context.HttpContext.Request.Method == "PUT")
            {
                foreach (var modelStateEntry in context.ModelState.Values)
                {
                    if (modelStateEntry.ValidationState == ModelValidationState.Invalid)
                    {
                        foreach (var error in modelStateEntry.Errors)
                        {
                            if (!string.IsNullOrEmpty(error.ErrorMessage))
                            {
                                metaDataErrors.AppendLine();
                                metaDataErrors.Append(error.ErrorMessage);
                            }
                            if (error.Exception is not null)
                            {
                                modelBindingException.Append(error.Exception.Message);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(metaDataErrors.ToString()) || !string.IsNullOrEmpty(modelBindingException.ToString()))
                {
                    StringBuilder responseMessage = new StringBuilder();
                    if (!string.IsNullOrEmpty(metaDataErrors.ToString()))
                        responseMessage.Append($"MetaData Validation Errors:{metaDataErrors.ToString()}");
                    if (!string.IsNullOrEmpty(modelBindingException.ToString()))
                        responseMessage.Append($"Serialization Exception: {modelBindingException}");
                    Log.Logger.Error(responseMessage.ToString());

                    throw new Exception(responseMessage.ToString());
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context) { }

    }

}
