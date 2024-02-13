using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.DataServices;
using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Presentation.ViewModels;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public interface IProductsManager
    {
        Task<ProductModel> AddAsync(ProductModel product,string userId);
        IEnumerable<ProductModel> GetAll();
        Task<ProductModel?> GetByIdAsync(int id);
        Task<ProductModel> UpdateAsync(ProductModel product, string userId);
        Task<ProductModel> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id, string userId);
        Task<ProductModel?> FindOneAsync(int id, string userId);
        Task SaveChangesAsync();

    }
}
