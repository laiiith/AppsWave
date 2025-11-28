using AppsWave.Entites;
using AppsWave.Services.Data;
using AppsWave.Services.Repository;
using AppsWave.Services.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AppsWave.Services
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddScoped<AppsWave.Services.Services.Auth.IAuthService, AppsWave.Services.Services.Auth.AuthService>();
            builder.Services.AddScoped<AppsWave.Services.Services.Auth.IJwtTokenGenerator, AppsWave.Services.Services.Auth.JwtTokenGenerator>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<AppsWave.Services.Data.AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.Configure<AppsWave.Entites.JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
            builder.Services.AddIdentity<AppsWave.Entites.User, IdentityRole>().AddEntityFrameworkStores<AppsWave.Services.Data.AppDbContext>().AddDefaultTokenProviders();

            var jwtOpt = builder.Configuration.GetSection("ApiSettings:JwtOptions");

            var secret = jwtOpt.GetValue<string>("Secret");
            var issuer = jwtOpt.GetValue<string>("Issuer");
            var audience = jwtOpt.GetValue<string>("Audience");
            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                };
            });
            builder.Services.AddAuthorization();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();

                await SeedData(scope.ServiceProvider);
            }

            app.Run();
        }

        private static async Task SeedData(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create Roles
            string[] roles = { "ADMIN", "VISITOR" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed Admin
            var adminEmail = "admin@appswave.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Super Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "ADMIN");
            }

            // Seed Visitor
            var visitorEmail = "visitor@appswave.com";
            var visitorUser = await userManager.FindByEmailAsync(visitorEmail);
            if (visitorUser == null)
            {
                visitorUser = new User
                {
                    UserName = visitorEmail,
                    Email = visitorEmail,
                    FullName = "Test Visitor",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(visitorUser, "Visitor@123");
                await userManager.AddToRoleAsync(visitorUser, "VISITOR");
            }

            // Seed Products
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { ArabicName = "لابتوب ديل", EnglishName = "Dell Laptop", Price = 45000m },
                    new Product { ArabicName = "هاتف سامسونج", EnglishName = "Samsung Phone", Price = 25000m },
                    new Product { ArabicName = "سماعات سوني", EnglishName = "Sony Headphones", Price = 5000m }
                );
                await db.SaveChangesAsync();
            }
        }
    }
}
