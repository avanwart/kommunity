using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ClickLogMap : EntityTypeConfiguration<ClickLog>
    {
        public ClickLogMap()
        {
            // Primary Key
            this.HasKey(t => t.clickLogID);

            // Properties
            this.Property(t => t.ipAddress)
                .HasMaxLength(25);

            this.Property(t => t.clickType)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.referringURL)
                .HasMaxLength(255);

            this.Property(t => t.currentURL)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ClickLog");
            this.Property(t => t.clickLogID).HasColumnName("clickLogID");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.clickType).HasColumnName("clickType");
            this.Property(t => t.referringURL).HasColumnName("referringURL");
            this.Property(t => t.currentURL).HasColumnName("currentURL");
            this.Property(t => t.productID).HasColumnName("productID");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}
