using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ZoneMap : EntityTypeConfiguration<Zone>
    {
        public ZoneMap()
        {
            // Primary Key
            this.HasKey(t => t.zoneID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Zone");
            this.Property(t => t.zoneID).HasColumnName("zoneID");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
