using VendingMachineAPI.Presentation.ViewModels;

namespace VendingMachineAPI.BusinessLogic.Services
{
    public interface IAuthManager
    {
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
    }
}
