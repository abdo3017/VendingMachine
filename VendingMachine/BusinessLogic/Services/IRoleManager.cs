using Microsoft.AspNetCore.Identity;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public interface IRoleManager
    {
        Task<IdentityRole<string>?> AddUserRoleAsync(ApplicationUser user, string role);
        Task<IdentityUserRole<string>?> UpdateUserRoleAsync(string userId, string roleId, string NewRoleId);
    }
}