using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public interface IVendingMachineRepository
    {
        Task<VendingMachineCoins> AddAsync(VendingMachineCoins entity);
        IQueryable<VendingMachineCoins> GetAll();
        Task<VendingMachineCoins> UpdateAsync(VendingMachineCoins entity);
        Task SaveChangesAsync();
        Task<VendingMachineCoins> DeleteAsync(VendingMachineCoins entity);
        Task<VendingMachineCoins?> FindOneAsync(Expression<Func<VendingMachineCoins, bool>> predicate);
        Task<bool> IsExistAsync(Expression<Func<VendingMachineCoins, bool>> predicate);
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
    }
}
