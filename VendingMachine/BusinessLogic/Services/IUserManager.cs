using System.Linq.Expressions;
using VendingMachineAPI.Presentation.ViewModels;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public interface IUserManager
    {
        Task<UserModel?> GetByIdAsync(string id);
        Task<UserModel?> GetByUserNameAsync(string userName);
        Task<UserModel> UpdateAsync(UserModel entity, string id);
        Task DeleteAsync(string id);
        Task<UserModel> CreateAsync(UserModel model);
        Task SaveChangesAsync();
    }
}
