using VendingMachineAPI.InfraStructure.DataServices;
using VendingMachineAPI.Presentation.ViewModels;
using VendingMachineAPI.Utils;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public class ProductsManager : IProductsManager
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsManager(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<ProductModel> AddAsync(ProductModel product, string userId)
        {
            var addedProduct = await _productsRepository.AddAsync(product.ToEntity(userId));
            return addedProduct.ToDTO();
        }

        public async Task<ProductModel> DeleteAsync(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            var deletedProduct = await _productsRepository.DeleteAsync(product);
            return deletedProduct.ToDTO();
        }

        public IEnumerable<ProductModel> GetAll()
        {
            var products = _productsRepository.GetAll()
                .Select(s => s.ToDTO())
                .ToList();

            return products;
        }

        public async Task<ProductModel?> GetByIdAsync(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            return product.ToDTO();
        }

        public async Task SaveChangesAsync()
        {
            await _productsRepository.SaveChangesAsync();
        }

        public async Task<ProductModel> UpdateAsync(ProductModel product, string userId)
        {
            var updatedProduct = await _productsRepository.UpdateAsync(product.ToEntity(userId));
            return updatedProduct.ToDTO();
        }
        public async Task<bool> IsExistAsync(int id, string userId)
        {
            return await _productsRepository.IsExistAsync(s => s.Id == id && s.SellerId == userId);
        }
        public async Task<ProductModel?> FindOneAsync(int id, string userId)
        {
            var product = await _productsRepository.FindOneAsync(s => s.Id == id && s.SellerId == userId);
            return product.ToDTO();
        }
    }
}
