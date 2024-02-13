
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using VendingMachineAPI.BusinessLogic.Services;
using VendingMachineAPI.InfraStructure.Database;
using VendingMachineAPI.InfraStructure.DataServices;
using VendingMachineAPI.InfraStructure.Entity;
using VendingMachineAPI.Utils;

namespace VendingMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            // Add services to the container.

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
           
            builder.Services.AddDbContextPool<ApplicationDbContext>(
                option => option.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IProductsManager, ProductsManager>();
            builder.Services.AddScoped<IRoleManager, RoleManager>();
            builder.Services.AddScoped<IVendingMachineManager, VendingMachineManager>();
            builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IVendingMachineRepository, VendingMachineRepository>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers(options=>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();

            });

            // Adding Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))
                };
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VendingMachine API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // To Enable authorization using Swagger (JWT)  
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            //Add support to logging with SERILOG
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();



            var app = builder.Build();

            app.UseExceptionHandler(options => {});

            Helper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
            StaticServiceProvider.Provider = app.Services;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VendingMachine v1"));
            }

            //uncomment the below line if you need to log process of API request
            //app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
