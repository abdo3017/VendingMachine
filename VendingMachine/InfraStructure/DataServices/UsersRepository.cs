using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using VendingMachineAPI.BusinessLogic.Services;
using VendingMachineAPI.InfraStructure.Database;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate) => _context.Users.Where(predicate).AsNoTracking();

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password) => await _userManager.CreateAsync(user, password);

        public IQueryable<ApplicationUser> GetAll() => _context.Users;
        public async Task<ApplicationUser> GetUserById(string id) => await _userManager.FindByIdAsync(id);

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
        {
            await _userManager.UpdateAsync(entity);
            return entity;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task DeleteAsync(ApplicationUser entity)
        {
            await _userManager.DeleteAsync(entity);
        }

        public async Task<ApplicationUser?> FindOneAsync(Expression<Func<ApplicationUser, bool>> predicate) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(predicate);

        public async Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password)
        {
            return await _userManager.CheckPasswordAsync(applicationUser, password);
        }

        public async Task<string> ChangePasswordAsync(ApplicationUser applicationUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser applicationUser, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(applicationUser, token, password);
        }
    }
}
