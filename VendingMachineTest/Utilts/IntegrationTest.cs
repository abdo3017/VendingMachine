using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using VendingMachine;
using VendingMachineAPI.InfraStructure.Database;

namespace VendingMachineTest.Utilts
{
    
    [Trait("Category", "Integration")]
    public abstract class IntegrationTest:  IClassFixture<ApiWebApplicationFactory<Program>>
    {
        protected string url;
        protected readonly ApiWebApplicationFactory<Program> _factory;
        protected readonly ApplicationDbContext context;
        protected readonly HttpClient _client;

        public IntegrationTest(ApiWebApplicationFactory<Program> fixture)
        {

            _factory = fixture;
            _client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(_factory.Configuration.GetConnectionString("DefaultConnection"));
            context = new ApplicationDbContext(optionsBuilder.Options);

        }
    }
}
