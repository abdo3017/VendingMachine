using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Presentation.ViewModels;

namespace VendingMachineAPI.Utils
{
    public static class ModelsConverter
    {
        public static Products ToEntity(this ProductModel product, string currentUserId)
        {
            return new Products
            {
                Id = product.Id,
                Name = product.Name,
                AvailableAmount = product.AvailableAmount,
                Cost = product.Cost,
                SellerId = currentUserId
            };
        }

        public static ProductModel ToDTO(this Products product)
        {
            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                AvailableAmount = product.AvailableAmount,
                Cost = product.Cost,
            };
        }


        public static UserModel ToDTO(this ApplicationUser user, string role)
        {
            return new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Deposit = user.Deposit,
                Role = role
            };
        }

        public static ApplicationUser ToEntity(this UserModel user, string currentUserId)
        {
            return new ApplicationUser
            {
                Id = currentUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Deposit = user.Deposit
            };
        }
        public static ApplicationUser UpdateData(this ApplicationUser applicationUser, UserModel user)
        {
            applicationUser.FirstName = user.FirstName;
            applicationUser.LastName = user.LastName;
            applicationUser.UserName = user.UserName;
            applicationUser.Deposit = user.Deposit;
            return applicationUser;
        }
    }
}
