using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ClickLogMap : EntityTypeConfiguration<ClickLog>
    {
        public ClickLogMap()
        {
            // Primary Key
            HasKey(t => t.clickLogID);

            // Properties
            Property(t => t.ipAddress)
                .HasMaxLength(25);

            Property(t => t.clickType)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.referringURL)
                .HasMaxLength(255);

            Property(t => t.currentURL)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("ClickLog");
            Property(t => t.clickLogID).HasColumnName("clickLogID");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
            Property(t => t.clickType).HasColumnName("clickType");
            Property(t => t.referringURL).HasColumnName("referringURL");
            Property(t => t.currentURL).HasColumnName("currentURL");
            Property(t => t.productID).HasColumnName("productID");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}