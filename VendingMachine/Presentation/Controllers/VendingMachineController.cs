using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using System.Drawing;
using VendingMachineAPI.BusinessLogic.Services;
using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Presentation.ViewModels;
using VendingMachineAPI.Utils;

namespace VendingMachineAPI.Presentation.Controllers
{
    [Authorize(Roles = "buyer")]
    [ApiController]
    [Route("[controller]")]
    [ArgumentsValidationFilter]
    public class VendingMachineController : ControllerBase
    {
        private readonly IVendingMachineManager _vendingMachineManager;

        public VendingMachineController(IVendingMachineManager vendingMachineManager)
        {
            _vendingMachineManager = vendingMachineManager;
        }

        [HttpPost("Deposit")]
        public async Task<IActionResult> DepositAsync([FromBody] List<Coins> coins)
        {
            Log.Logger.Information($"trying to deposit coins for current buyer with id = {Helper.CurrentUserId()} with body info", coins);

            Log.Logger.Information("check model state for body");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Log.Logger.Information("model state for body is valid");


            foreach (var coin in coins)
            {
                await _vendingMachineManager.UpdateAsync(coin);
            }
            Log.Logger.Information($"deposit coins for current buyer with id = {Helper.CurrentUserId()} is successfully");
            return Ok();
        }

        [HttpPost("Buy")]
        public async Task<IActionResult> BuyAsync(int productId,int amount)
        {
            Log.Logger.Information($"trying to Buy product with id={productId} and amount = {amount} for current buyer with id = {Helper.CurrentUserId()} is successfully");
            var buyModel = await _vendingMachineManager.BuyAsync(productId, amount,Helper.CurrentUserId());
            Log.Logger.Information($"Buying product with id={productId} and amount = {amount} for current buyer with id = {Helper.CurrentUserId()}");

            return Ok(buyModel);
        }

        [HttpPost("Reset")]
        public async Task<IActionResult> ResetAsync()
        {
            Log.Logger.Information($"trying to reset deposit for current buyer with id = {Helper.CurrentUserId()}");
            var coins = await _vendingMachineManager.ResetAsync(Helper.CurrentUserId());
            Log.Logger.Information($"resetting deposit for current buyer with id = {Helper.CurrentUserId()}trying to Buy product with id= remaining coins",coins);

            return Ok(coins);
        }
    }
}
