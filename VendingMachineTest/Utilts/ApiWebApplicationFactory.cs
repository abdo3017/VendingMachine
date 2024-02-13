using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendingMachineAPI.InfraStructure.Database;
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineTest.Utilts
{
    public class ApiWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json")
                  .AddEnvironmentVariables()
                  .Build();
                config.AddConfiguration(Configuration);
            });

            builder.ConfigureTestServices(services =>
            {
                #region Authuntication 
                //services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>()
                //.AddEntityFrameworkStores<ApplicationDbContext>()
                //.AddDefaultTokenProviders();
                // has permission
                services.AddAuthorization(options =>
                {
                    // AuthConstants.Scheme is just a scheme we define. I called it ""bearer""
                    options.DefaultPolicy = new AuthorizationPolicyBuilder("bearer")
                        .RequireAuthenticatedUser()
                        .Build();
                });
                // for login
                services.AddAuthentication("bearer")
                    .AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthenticationHandler>(
                        "bearer", null);
                #endregion

                
            });
        }
    }
}
