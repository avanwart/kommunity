using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class YouAreMap : EntityTypeConfiguration<YouAre>
    {
        public YouAreMap()
        {
            // Primary Key
            HasKey(t => t.youAreID);

            // Properties
            Property(t => t.typeLetter)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("YouAre");
            Property(t => t.youAreID).HasColumnName("youAreID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.typeLetter).HasColumnName("typeLetter");
            Property(t => t.name).HasColumnName("name");
        }
    }
}