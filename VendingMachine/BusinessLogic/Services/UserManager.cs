using Microsoft.AspNetCore.Identity;
using Serilog;
using VendingMachineAPI.InfraStructure.DataServices;
using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Presentation.ViewModels;
using VendingMachineAPI.Utils;
namespace VendingMachineAPI.BusinessLogic.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IRoleManager _roleManager;
        private readonly IRoleRepository _roleRepository;

        public UserManager(IUsersRepository usersRepository, IProductsRepository productsRepository, IRoleManager roleManager, IRoleRepository roleRepository)
        {
            _usersRepository = usersRepository;
            _productsRepository = productsRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        public virtual async Task<UserModel?> GetByIdAsync(string id)
        {
            var user = await _usersRepository.FindOneAsync(s => s.Id == id);
            var role = await _roleRepository.GetRoleByUserIdAsync(user?.Id);
            return user?.ToDTO(role?.Name);
        }

        public virtual async Task<UserModel?> GetByUserNameAsync(string userName)
        {
            var user = await _usersRepository.FindOneAsync(s => s.UserName == userName);
            var role = await _roleRepository.GetRoleByUserIdAsync(user?.Id);
            return user?.ToDTO(role?.Name);
        }
        public async Task<UserModel> CreateAsync(UserModel model)
        {
            var user = await _usersRepository.FindOneAsync(s => s.UserName == model.UserName);
            if (user is not null)
            {
                Log.Logger.Error($"Username:{model.UserName} is already registered!");
                throw new Exception("Username is already registered!");
            }

            user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Deposit = model.Deposit,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await _usersRepository.CreateUserAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description} , ";

                Log.Logger.Error("user couldn't create successfully with errors", errors);
                throw new Exception(errors);
            }

            await _roleManager.AddUserRoleAsync(user, model.Role);

            return model;
        }

        public async Task<UserModel> UpdateAsync(UserModel entity, string currentUserId)
        {
            var user = await _usersRepository.GetUserById(currentUserId);
            await UpdatePasswordIfChanged(user, entity.Password);

            await UpdateRoleIfChanged(entity, currentUserId);

            await _usersRepository.UpdateAsync(user.UpdateData(entity));

            return entity;
        }

        private async Task UpdateRoleIfChanged(UserModel entity, string currentUserId)
        {
            var role = await _roleRepository.GetRoleByUserIdAsync(currentUserId);
            if (role?.Name != entity.Role)
            {
                var newRole = await _roleRepository.GetRoleByNameAsync(entity.Role);
                await _roleManager.UpdateUserRoleAsync(currentUserId, role.Id, newRole?.Id);
            }
        }
        private async Task UpdatePasswordIfChanged(ApplicationUser entity, string password)
        {
            bool checkPassword = await _usersRepository.CheckPasswordAsync(entity, password);
            if (!checkPassword)
            {
                var token = await _usersRepository.ChangePasswordAsync(entity);
                var result = await _usersRepository.ResetPasswordAsync(entity, token, password);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to reset password: " + result.Errors.FirstOrDefault());
                }
            }
        }
        public async Task SaveChangesAsync() => await _usersRepository.SaveChangesAsync();

        public async Task DeleteAsync(string id)
        {
            var user = await _usersRepository.GetUserById(id);
            await _productsRepository.DeleteAllProductsBySellerAsync(id);
            await _usersRepository.DeleteAsync(user);
        }

    }
}
