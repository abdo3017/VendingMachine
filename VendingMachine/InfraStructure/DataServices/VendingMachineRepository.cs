using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.Database;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public class VendingMachineRepository : IVendingMachineRepository
    {
        private readonly ApplicationDbContext _context;

        public VendingMachineRepository(ApplicationDbContext context) => _context = context;


        public virtual async Task<VendingMachineCoins> AddAsync(VendingMachineCoins entity)
        {
            await _context.VendingMachineCoins.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual IQueryable<VendingMachineCoins> GetAll() => _context.VendingMachineCoins;

        public async Task<VendingMachineCoins> UpdateAsync(VendingMachineCoins entity)
        {
            _context.VendingMachineCoins.Update(entity);
            await SaveChangesAsync();
            return entity;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel) => await _context.Database.BeginTransactionAsync(isolationLevel);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<VendingMachineCoins> DeleteAsync(VendingMachineCoins entity)
        {
            _context.VendingMachineCoins.Remove(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<VendingMachineCoins?> FindOneAsync(Expression<Func<VendingMachineCoins, bool>> predicate) => await _context.VendingMachineCoins.FirstOrDefaultAsync(predicate);
        public async Task<bool> IsExistAsync(Expression<Func<VendingMachineCoins, bool>> predicate) => await _context.VendingMachineCoins.AnyAsync(predicate);

    }
}
