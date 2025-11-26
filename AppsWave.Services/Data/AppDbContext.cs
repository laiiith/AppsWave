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

            modelBuilder.Entity<AppsWave.Entites.User>().HasData(new AppsWave.Entites.User
            { 
                Email = "MohammadJamal22@gmail.com",
                FullName = "Mohammad Jamal",
                UserName = "MJamal",
            });

            modelBuilder.Entity<AppsWave.Entites.User>().HasData(new AppsWave.Entites.User
            {
                Email = "TalaLutfi03@gmail.com",
                FullName = "Tala Lutfi",
                UserName = "TLutfi",
            });

        }
    }
}
