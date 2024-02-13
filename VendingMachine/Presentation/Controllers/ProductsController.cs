using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using VendingMachineAPI.BusinessLogic.Services;
using VendingMachineAPI.Presentation.ViewModels;
using VendingMachineAPI.Utils;

namespace VendingMachineAPI.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ArgumentsValidationFilter]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsManager _productsManager;

        public ProductsController(IProductsManager productsManager)
        {
            _productsManager = productsManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.Logger.Information("get all products in vending machine");
            return Ok(_productsManager.GetAll());
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            Log.Logger.Information($"getting product with id = {id} in vending machine");
            var product = await _productsManager.GetByIdAsync(id);

            return Ok(product);
        }

        [Authorize(Roles = "seller")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]ProductModel product)
        {
            Log.Logger.Information($"adding product for current seller with id = {Helper.CurrentUserId()} with body info", product);

            Log.Logger.Information("check model state for body");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Log.Logger.Information("model state for body is valid");

            var addedProduct = await _productsManager.AddAsync(product, Helper.CurrentUserId());
            Log.Logger.Information($"product added successfully for current seller with id = {Helper.CurrentUserId()}", product);
            
            return Ok(addedProduct);
        }

        [Authorize(Roles = "seller")]
        [HttpPut]
        public async Task<IActionResult> PutAsync(int id, [FromBody] ProductModel productModel)
        {

            Log.Logger.Information($"updating product for current seller with id = {Helper.CurrentUserId()} with id = {id}");

            Log.Logger.Information("check model state for body");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Log.Logger.Information("model state for body is valid");

            Log.Logger.Information($"check product with id = {id} Is Exist in vending machine");

            var checkProduct =await _productsManager.IsExistAsync(id, Helper.CurrentUserId());

            if (!checkProduct)
            {
                Log.Logger.Information("Product Not found with id = {id}");

                return NotFound($"Product Not found with id = {id}");
            }
            Log.Logger.Information($"product with id = {id} Exists in vending machine");
            if (id != productModel.Id)
            {
                Log.Logger.Information($"the Id = {id} incorrect in the body");
                return BadRequest($"the Id = {id} incorrect in the body");
            }

            var updatedProduct = await _productsManager.UpdateAsync(productModel, Helper.CurrentUserId());
            Log.Logger.Information($"product updated successfully for current seller with id = {Helper.CurrentUserId()}", updatedProduct);
            
            return Ok(updatedProduct);
        }

        [Authorize(Roles = "seller")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Logger.Information($"deleting product for current seller with id = {Helper.CurrentUserId()} with id = {id}");

            Log.Logger.Information($"check product with id = {id} Is Exist in vending machine");
            var checkProduct = await _productsManager.IsExistAsync(id, Helper.CurrentUserId());

            if (!checkProduct)
            {
                Log.Logger.Information("Product Not found with id = {id}");

                return NotFound($"Product Not found with id = {id}");
            }

            var deletedProduct = await _productsManager.DeleteAsync(id);
            Log.Logger.Information($"product deleted successfully for current seller with id = {Helper.CurrentUserId()}", deletedProduct);
            return Ok(deletedProduct);
        }
    }
}
