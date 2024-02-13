using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public interface IUsersRepository
    {
        IQueryable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate);
        IQueryable<ApplicationUser> GetAll();
        Task<ApplicationUser> GetUserById(string id);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> UpdateAsync(ApplicationUser entity);
        Task DeleteAsync(ApplicationUser entity);
        Task<ApplicationUser?> FindOneAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task SaveChangesAsync();
        Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password);
        Task<string> ChangePasswordAsync(ApplicationUser applicationUser);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser applicationUser, string token, string password);
    }
}
