using AppsWave.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppsWave.Services.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppsWave.Entites.User>(options)
    {
        public DbSet<AppsWave.Entites.User> Users { get; set; }
        //public DbSet<AppsWave.Entites.Product> Products { get; set; }
        //public DbSet<AppsWave.Entites.Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminRoleId = "role-admin";
            var customerRoleId = "role-customer";
            var user1Id = "user-1";
            var user2Id = "user-2";

            var adminRole = new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var customerRole = new IdentityRole
            {
                Id = customerRoleId,
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            };

            modelBuilder.Entity<IdentityRole>().HasData(adminRole, customerRole);

            var hasher = new PasswordHasher<User>();

            var user1 = new User
            {
                Id = user1Id,
                UserName = "MJamal",
                NormalizedUserName = "MJAMAL",
                Email = "MohammadJamal22@gmail.com",
                NormalizedEmail = "MOHAMMADJAMAL22@GMAIL.COM",
                EmailConfirmed = true,
                FullName = "Mohammad Jamal",
                SecurityStamp = "stamp-1" 
            };
            user1.PasswordHash = hasher.HashPassword(user1, "Password123!");

            var user2 = new User
            {
                Id = user2Id,
                UserName = "TLutfi",
                NormalizedUserName = "TLUTFI",
                Email = "TalaLutfi03@gmail.com",
                NormalizedEmail = "TALALUTFI03@GMAIL.COM",
                EmailConfirmed = true,
                FullName = "Tala Lutfi",
                SecurityStamp = "stamp-2"
            };
            user2.PasswordHash = hasher.HashPassword(user2, "Password123!");

            modelBuilder.Entity<User>().HasData(user1, user2);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = user1Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = customerRoleId,
                    UserId = user2Id
                }
            );
        }
        }
}
