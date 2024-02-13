using System.ComponentModel.DataAnnotations;

namespace VendingMachineAPI.Presentation.ViewModels
{
    public class TokenRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
