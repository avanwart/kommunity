using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RelationshipStatuMap : EntityTypeConfiguration<RelationshipStatu>
    {
        public RelationshipStatuMap()
        {
            // Primary Key
            HasKey(t => t.relationshipStatusID);

            // Properties
            Property(t => t.typeLetter)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("RelationshipStatus");
            Property(t => t.relationshipStatusID).HasColumnName("relationshipStatusID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.typeLetter).HasColumnName("typeLetter");
            Property(t => t.name).HasColumnName("name");
        }
    }
}