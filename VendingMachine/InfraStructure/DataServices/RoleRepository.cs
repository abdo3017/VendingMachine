using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachineAPI.InfraStructure.Database;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineAPI.InfraStructure.DataServices
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityRole?> GetRoleByUserIdAsync(string userId)
        {
            var identityRole = from user in _context.Users
                               join userRole in _context.UserRoles on user.Id equals userRole.UserId
                               join role in _context.Roles on userRole.RoleId equals role.Id
                               where user.Id == userId
                               select role;
            return await identityRole.AsNoTracking().FirstOrDefaultAsync();
        }
        
        public async Task<IdentityRole?> GetRoleByIdAsync(string Id)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == Id);
        }
        
        public async Task<IdentityResult?> CreateRoleAsync(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }
        
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task AddUserRolesAsync(IdentityUserRole<string> userRoles)
        {
            await _context.UserRoles.AddAsync(userRoles);
            await SaveChangesAsync();
        }

        public async Task DeleteUserRolesAsync(string userId, string roleId)
        {
            await _context.UserRoles.Where(s => s.UserId == userId && s.RoleId == roleId).ExecuteDeleteAsync();
            await SaveChangesAsync();
        }
        
        public async Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string role) => await _userManager.AddToRoleAsync(user, role);
        
        public async Task<IdentityRole?> GetRoleByNameAsync(string role)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(s => string.Equals(s.NormalizedName, role, StringComparison.OrdinalIgnoreCase));
        }

    }
}
