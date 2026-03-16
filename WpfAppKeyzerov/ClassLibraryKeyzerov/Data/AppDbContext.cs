using Microsoft.EntityFrameworkCore;
using Keyzerov.Core.Models;

namespace Keyzerov.Core.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerType> PartnerTypes { get; set; }
        public DbSet<SalesHistory> SalesHistory { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("app");

            modelBuilder.Entity<PartnerType>(entity =>
            {
                entity.ToTable("partner_types_keyzerov");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("partners_keyzerov");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
                entity.Property(e => e.TypeId).HasColumnName("type_id");
                entity.Property(e => e.INN).HasColumnName("inn").HasMaxLength(20);
                entity.Property(e => e.Logo).HasColumnName("logo");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(500);
                entity.Property(e => e.DirectorName).HasColumnName("director_name").HasMaxLength(200);
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(50);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(e => e.SalesPlaces).HasColumnName("sales_places").HasMaxLength(500);

                entity.HasOne(e => e.Type)
                      .WithMany()
                      .HasForeignKey(e => e.TypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SalesHistory>(entity =>
            {
                entity.ToTable("sales_history_keyzerov");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PartnerId).HasColumnName("partner_id");
                entity.Property(e => e.ProductName).HasColumnName("product_name").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.SaleDate).HasColumnName("sale_date");

                entity.HasOne(e => e.Partner)
                      .WithMany(p => p.SalesHistory)
                      .HasForeignKey(e => e.PartnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}