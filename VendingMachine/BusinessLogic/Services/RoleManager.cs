using Microsoft.AspNetCore.Identity;
using Serilog;
using VendingMachineAPI.InfraStructure.DataServices;
using VendingMachineAPI.InfraStructure.Entity;
namespace VendingMachineAPI.BusinessLogic.Services
{
    public class RoleManager : IRoleManager
    {
        private readonly IRoleRepository _roleRepository;

        public RoleManager(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IdentityRole<string>?> AddUserRoleAsync(ApplicationUser user, string role)
        {
            var roleResult = await _roleRepository.GetRoleByNameAsync(role);
            if (roleResult == null)
            {
                IdentityRole tmpRole = new IdentityRole()
                {
                    Name = role,
                    NormalizedName = role,
                };
                var creatingRoleResult = await _roleRepository.CreateRoleAsync(tmpRole);
                if (!creatingRoleResult.Succeeded)
                {
                    Log.Logger.Error($"Role {role} can not be created!.");
                    throw new Exception($"Role {role} can not be created!.");
                }
            }

            var addingRoleResult = await _roleRepository.AssignRoleToUserAsync(user, role);
            if (!addingRoleResult.Succeeded)
            {
                Log.Logger.Error($"Role {role} can not be assigned to the user!.");
                throw new Exception($"Role {role} can not be assigned to the user!.");
            }
            return roleResult;
        }
        
        public async Task<IdentityUserRole<string>?> UpdateUserRoleAsync(string userId, string roleId, string NewRoleId)
        {
            await _roleRepository.DeleteUserRolesAsync(userId, roleId);

            var userRoles = new IdentityUserRole<string>
            {
                RoleId = NewRoleId,
                UserId = userId
            };

            await _roleRepository.AddUserRolesAsync(userRoles);
            return userRoles;
        }
    }
}
