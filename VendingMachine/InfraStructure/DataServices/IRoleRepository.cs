using Microsoft.AspNetCore.Identity;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public interface IRoleRepository
    {
        Task<IdentityRole?> GetRoleByUserIdAsync(string userId);
        Task<IdentityRole?> GetRoleByNameAsync(string role);
        Task<IdentityResult?> CreateRoleAsync(IdentityRole role);
        Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string role);
        Task SaveChangesAsync();
        Task DeleteUserRolesAsync(string userId, string roleId);
        Task AddUserRolesAsync(IdentityUserRole<string> userRoles);
    }
}