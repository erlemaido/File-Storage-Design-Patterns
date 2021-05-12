using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductStateType> ProductStatusTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductStateType>().HasData(
                new ProductStateType
                {
                    ProductStateTypeCode = 1,
                    Title = "Ootel"
                },
                new ProductStateType
                {
                    ProductStateTypeCode = 2,
                    Title = "Aktiivne"
                },
                new ProductStateType
                {
                    ProductStateTypeCode = 3,
                    Title = "Mitteaktiivne"
                },
                new ProductStateType
                {
                    ProductStateTypeCode = 4,
                    Title = "Lõpetatud"
                });
        }
    }
}