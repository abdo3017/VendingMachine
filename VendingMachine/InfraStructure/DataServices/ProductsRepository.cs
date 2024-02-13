using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.Database;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductsRepository(ApplicationDbContext context) => _context = context;

        public virtual async Task<Products> AddAsync(Products entity)
        {
            await _context.Products.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual IQueryable<Products> Find(Expression<Func<Products, bool>> predicate)
            => _context.Products.Where(predicate);


        public virtual async Task<Products?> GetByIdAsync(int id)
            => await _context.Products.FindAsync(id);

        public virtual IQueryable<Products> GetAll() => _context.Products;

        public async Task<Products> UpdateAsync(Products entity)
        {
            _context.Products.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<Products> DeleteAsync(Products entity)
        {
            _context.Products.Remove(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAllProductsBySellerAsync(string sellerId)
        {
            Find(s => s.SellerId == sellerId).ExecuteDelete();
            await SaveChangesAsync();
        }

        public async Task<Products?> FindOneAsync(Expression<Func<Products, bool>> predicate)
            => await _context.Products.FirstOrDefaultAsync(predicate);
        public async Task<bool> IsExistAsync(Expression<Func<Products, bool>> predicate)
            => await _context.Products.AnyAsync(predicate);


    }
}
