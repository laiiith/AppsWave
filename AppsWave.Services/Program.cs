using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text; 

namespace AppsWave.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<AppsWave.Services.Services.Auth.IAuthService, AppsWave.Services.Services.Auth.AuthService>();
            builder.Services.AddScoped<AppsWave.Services.Services.Auth.IJwtTokenGenerator, AppsWave.Services.Services.Auth.JwtTokenGenerator>();

            builder.Services.AddDbContext<AppsWave.Services.Data.AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.Configure<AppsWave.Entites.JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
            builder.Services.AddIdentity<AppsWave.Entites.User, IdentityRole>().AddEntityFrameworkStores<AppsWave.Services.Data.AppDbContext>().AddDefaultTokenProviders();
                //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options=>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description= "Enter the Bearer Authorization string as following: `Bearer Generate-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string> {} },
                });
            });

            var jwtOpt = builder.Configuration.GetSection("ApiSettings:JwtOptions");
             
            var secret = jwtOpt.GetValue<string>("Secret");
            var issuer = jwtOpt.GetValue<string>("Issuer");
            var audience = jwtOpt.GetValue<string>("Audience");
            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x=>
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
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            ApplyPendingMigrations(app);

            app.Run();

        }
        private static void ApplyPendingMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppsWave.Services.Data.AppDbContext>();
            if (db.Database.GetPendingMigrations().Any()) db.Database.Migrate();
        }
    }
}
