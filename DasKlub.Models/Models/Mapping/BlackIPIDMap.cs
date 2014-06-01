using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class BlackIPIDMap : EntityTypeConfiguration<BlackIPID>
    {
        public BlackIPIDMap()
        {
            // Primary Key
            HasKey(t => t.blackIPID1);

            // Properties
            Property(t => t.ipAddress)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("BlackIPID");
            Property(t => t.blackIPID1).HasColumnName("blackIPID");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}