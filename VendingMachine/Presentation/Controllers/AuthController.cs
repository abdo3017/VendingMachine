using Microsoft.AspNetCore.Mvc;
using Serilog;
using VendingMachineAPI.BusinessLogic.Services;
using VendingMachineAPI.Presentation.ViewModels;
using VendingMachineAPI.Utils;


namespace VendingMachine.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ArgumentsValidationFilter]

    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("Token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            Log.Logger.Information("authenticating user with body info", model);
            
            Log.Logger.Information("check model state for body");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Log.Logger.Information("model state for body is valid");

            Log.Logger.Information("Get Token for user");
            var result = await _authManager.GetTokenAsync(model);
           
            Log.Logger.Information("check user is authenticated");
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            Log.Logger.Information("user is authenticated successfully with token", result);

            return Ok(result);
        }
    }
}