namespace VendingMachineAPI.Presentation.ViewModels
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public decimal Deposit { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
