using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using VendingMachineAPI.BusinessLogic.Services;
using VendingMachineAPI.Presentation.ViewModels;
using VendingMachineAPI.Utils;

namespace VendingMachineAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [ArgumentsValidationFilter]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

       
        [HttpGet("GetByUserName")]
        public async Task<IActionResult> GetByUserNameAsync(string userName)
        {
            Log.Logger.Information($"getting user with username = {userName}");
            var product = await _userManager.GetByUserNameAsync(userName);
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserModel model)
        {

            Log.Logger.Information($"adding user with username = {model.UserName} with body info", model);

            Log.Logger.Information("check model state for body");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Log.Logger.Information("model state for body is valid");


            var newUser = await _userManager.CreateAsync(model);
            Log.Logger.Information($"user added successfully with username = {model.UserName} with body info", model);

            return Ok(newUser);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(string userName, [FromBody] UserModel user)
        {
            Log.Logger.Information($"updating user with username = {user.UserName} with body info", user);

            Log.Logger.Information("check model state for body");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Log.Logger.Information("model state for body is valid");

            Log.Logger.Information($"check user with username = {user.UserName}");
            var currentUser =await _userManager.GetByUserNameAsync(userName);

            if (currentUser is null)
            {
                Log.Logger.Information($"user Not found with user name = {userName}");
                return NotFound($"user Not found with user name = {userName}");
            }

            var updatedUser = await _userManager.UpdateAsync(user, Helper.CurrentUserId());
            Log.Logger.Information($"user added successfully with username = {updatedUser.UserName} with body info", updatedUser);

            return Ok(updatedUser);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string userName)
        {
            Log.Logger.Information($"deleting user with username = {userName} with body info");

            Log.Logger.Information($"check user with username = {userName}");
            var currentUser = await _userManager.GetByUserNameAsync(userName);

            if (currentUser is null)
            {
                Log.Logger.Information($"user Not found with user name = {userName}");
                return NotFound($"user Not found with user name = {userName}");
            }

            await _userManager.DeleteAsync(Helper.CurrentUserId());
            Log.Logger.Information($"user added successfully with username = {currentUser.UserName} with body info", currentUser);

            return Ok(currentUser);
        }
    }
}
