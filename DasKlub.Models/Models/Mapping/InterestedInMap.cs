using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class InterestedInMap : EntityTypeConfiguration<InterestedIn>
    {
        public InterestedInMap()
        {
            // Primary Key
            HasKey(t => t.interestedInID);

            // Properties
            Property(t => t.typeLetter)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("InterestedIn");
            Property(t => t.interestedInID).HasColumnName("interestedInID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.typeLetter).HasColumnName("typeLetter");
            Property(t => t.name).HasColumnName("name");
        }
    }
}