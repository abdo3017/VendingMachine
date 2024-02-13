namespace VendingMachineAPI.Presentation.ViewModels
{
    public class BuyModel
    {
        public decimal TotalAmount { get; set; }
        public List<Coins> Coins { get; set; } = new List<Coins>();
    }
}
