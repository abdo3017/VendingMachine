using System.ComponentModel.DataAnnotations;

namespace VendingMachineAPI.Presentation.ViewModels
{
    public class Coins
    {
        [AllowedValues(5,10,20,50,100)]
        public int Cent { get; set; }
        public int Amount { get; set; }
    }
}
