using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VendingMachineAPI.InfraStructure.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Deposit { get; set; }
    }
}
