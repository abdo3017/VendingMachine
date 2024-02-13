using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public interface IProductsRepository
    {
        Task<Products> AddAsync(Products entity);
        Task<Products> UpdateAsync(Products entity);
        Task<Products> DeleteAsync(Products entity);
        Task<Products?> GetByIdAsync(int id);
        IQueryable<Products> GetAll();
        IQueryable<Products> Find(Expression<Func<Products, bool>> predicate);
        Task<Products?> FindOneAsync(Expression<Func<Products, bool>> predicate);
        Task<bool> IsExistAsync(Expression<Func<Products, bool>> predicate);
        Task SaveChangesAsync();
        Task DeleteAllProductsBySellerAsync(string sellerId);
    }
}
