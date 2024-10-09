using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public sealed class MsDbContext : ProjectDbContext
    {
        public MsDbContext(DbContextOptions<MsDbContext> options, IConfiguration configuration)
            : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DArchMsContext")));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.GroupId, ug.UserId });

            modelBuilder.Entity<UserClaim>()
                .HasKey(ug => new { ug.ClaimId, ug.UserId });

            modelBuilder.Entity<GroupClaim>()
                .HasKey(ug => new { ug.GroupId, ug.ClaimId });

            // City ve Country arasındaki ilişki
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany() // Country'nin birçok City'si olabilir
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Address ve City arasındaki ilişki
            modelBuilder.Entity<Address>()
                .HasOne(a => a.City)
                .WithMany() // City'nin birçok Address'i olabilir
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Address ve Country arasındaki ilişki
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Country)
                .WithMany() // Country'nin birçok Address'i olabilir
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Storage ve Address arasındaki ilişki
            modelBuilder.Entity<Storage>()
                .HasOne(s => s.Address)
                .WithMany() // Address'in birçok Storage'ı olabilir
                .HasForeignKey(s => s.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer ve Address arasındaki ilişki
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Address)
                .WithMany() // Address'in birçok Customer'ı olabilir
                .HasForeignKey(c => c.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ve Color arasındaki ilişki
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Color)
                .WithMany() // Color'ın birçok Product'ı olabilir
                .HasForeignKey(p => p.ColorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ve Category arasındaki ilişki
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany() // Category'nin birçok Product'ı olabilir
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Product ve Storage arasındaki ilişki
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Storage)
                .WithMany() // Storage'ın birçok Product'ı olabilir
                .HasForeignKey(p => p.StorageId)
                .OnDelete(DeleteBehavior.NoAction);

            // Order ve Customer arasındaki ilişki
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany() // Customer'ın birçok Order'ı olabilir
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            // ProductOrder ve Product arasındaki ilişki
            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Product)
                .WithMany(p => p.ProductOrders) // Product'ın birçok ProductOrder'ı olabilir
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // ProductOrder ve Order arasındaki ilişki
            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Order)
                .WithMany(o => o.ProductOrders) // Order'ın birçok ProductOrder'ı olabilir
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Enum tipleri için ValueConverter
            modelBuilder.Entity<Product>()
                .Property(p => p.SizeCode)
                .HasConversion<int>(); // SizeCode enum'ını int olarak saklar

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryStatus)
                .HasConversion<int>(); // DeliveryStatus enum'ını int olarak saklar
        }

    }
}