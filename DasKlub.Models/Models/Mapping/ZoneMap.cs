using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ZoneMap : EntityTypeConfiguration<Zone>
    {
        public ZoneMap()
        {
            // Primary Key
            HasKey(t => t.zoneID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Zone");
            Property(t => t.zoneID).HasColumnName("zoneID");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.name).HasColumnName("name");
        }
    }
}