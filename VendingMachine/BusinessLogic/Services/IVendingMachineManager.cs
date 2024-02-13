using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Presentation.ViewModels;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public interface IVendingMachineManager
    {
        Task<VendingMachineCoins> AddAsync(VendingMachineCoins entity);
        IEnumerable<VendingMachineCoins> GetAll();
        Task UpdateAsync(Coins entity);
        Task SaveChangesAsync();
        Task<VendingMachineCoins> DeleteAsync(VendingMachineCoins entity);
        Task<VendingMachineCoins?> FindOneAsync(Expression<Func<VendingMachineCoins, bool>> predicate);
        Task<bool> IsExistAsync(Expression<Func<VendingMachineCoins, bool>> predicate);
        Task<BuyModel?> BuyAsync(int productId, int amount, string currentUserId);
        Task<List<Coins>> ResetAsync(string currentUserId);
    }
}
