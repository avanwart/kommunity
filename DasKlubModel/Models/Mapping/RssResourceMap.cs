using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RssResourceMap : EntityTypeConfiguration<RssResource>
    {
        public RssResourceMap()
        {
            // Primary Key
            this.HasKey(t => t.rssResourceID);

            // Properties
            this.Property(t => t.rssResourceURL)
                .HasMaxLength(400);

            this.Property(t => t.resourceName)
                .HasMaxLength(150);

            this.Property(t => t.providerKey)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("RssResource");
            this.Property(t => t.rssResourceID).HasColumnName("rssResourceID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.rssResourceURL).HasColumnName("rssResourceURL");
            this.Property(t => t.resourceName).HasColumnName("resourceName");
            this.Property(t => t.providerKey).HasColumnName("providerKey");
            this.Property(t => t.isEnabled).HasColumnName("isEnabled");
            this.Property(t => t.artistID).HasColumnName("artistID");
        }
    }
}
