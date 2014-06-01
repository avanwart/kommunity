using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RssResourceMap : EntityTypeConfiguration<RssResource>
    {
        public RssResourceMap()
        {
            // Primary Key
            HasKey(t => t.rssResourceID);

            // Properties
            Property(t => t.rssResourceURL)
                .HasMaxLength(400);

            Property(t => t.resourceName)
                .HasMaxLength(150);

            Property(t => t.providerKey)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("RssResource");
            Property(t => t.rssResourceID).HasColumnName("rssResourceID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.rssResourceURL).HasColumnName("rssResourceURL");
            Property(t => t.resourceName).HasColumnName("resourceName");
            Property(t => t.providerKey).HasColumnName("providerKey");
            Property(t => t.isEnabled).HasColumnName("isEnabled");
            Property(t => t.artistID).HasColumnName("artistID");
        }
    }
}