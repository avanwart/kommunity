using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class BlackIPIDMap : EntityTypeConfiguration<BlackIPID>
    {
        public BlackIPIDMap()
        {
            // Primary Key
            this.HasKey(t => t.blackIPID1);

            // Properties
            this.Property(t => t.ipAddress)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("BlackIPID");
            this.Property(t => t.blackIPID1).HasColumnName("blackIPID");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}
