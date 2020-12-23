using erp_project.Entities.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace erp_project.Entities
{
    /// <summary>
    /// สำหรับเชื่อมต่อและ Setting Database
    /// </summary>
    public partial class DBConnect : DbContext
    {
        /// <summary>
        /// สำหรับค้นหาตัวแปรของ appsetting.json
        /// </summary>
        public readonly IConfiguration Configuration;

        public DBConnect(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DBConnect()
        {
        }

        /// <summary>
        /// ตั้งค่าเพิ่มเติมเมื่อเกิด Event การสร้าง Model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Tables (ตาราง)
            modelBuilder.Entity<ProductAddonDetails>(entity =>
            {
                entity.HasKey(e => new { e.AddonId, e.AttributeId, e.ValueId });

                entity.HasIndex(e => e.AttributeId)
                    .HasName("IX_FK_ProductAddonDetail_ToProductAttribute");

                entity.HasIndex(e => e.ValueId)
                    .HasName("IX_FK_ProductAddonDetail_ToProductAttributeValue");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Addon)
                    .WithMany(p => p.ProductAddonDetails)
                    .HasForeignKey(d => d.AddonId)
                    .HasConstraintName("FK_ProductAddonDetail_ToProductAddon");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.ProductAddonDetails)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductAddonDetail_ToProductAttribute");
            });

            modelBuilder.Entity<ProductAddons>(entity =>
            {
                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_FK_Table_ToProduct");

                entity.Property(e => e.AddonActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.AddonStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductAddons)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Table_ToProduct");
            });

            modelBuilder.Entity<ProductAttributeValues>(entity =>
            {
                entity.HasIndex(e => e.AttributeId)
                    .HasName("IX_FK_ProductAtributeValue_ToProductAttribute");

                entity.Property(e => e.ValueActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.ProductAttributeValues)
                    .HasForeignKey(d => d.AttributeId)
                    .HasConstraintName("FK_ProductAtributeValue_ToProductAttribute");
            });

            modelBuilder.Entity<ProductAttributes>(entity =>
            {
                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_FK_ProductAttribute_ToProduct");

                entity.Property(e => e.AttributeActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductAttributes)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductAttribute_ToProduct");
            });

            modelBuilder.Entity<ProductUnit>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.ProductActive).HasDefaultValueSql("((1))");
            });
            #endregion
            #region Views (วิว)

            #endregion
            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// ตั้งค่าการเชื่อมต่อ Connection String ของ Database
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ConnectDatabase"));
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}