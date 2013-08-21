using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RelationshipStatuMap : EntityTypeConfiguration<RelationshipStatu>
    {
        public RelationshipStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.relationshipStatusID);

            // Properties
            this.Property(t => t.typeLetter)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RelationshipStatus");
            this.Property(t => t.relationshipStatusID).HasColumnName("relationshipStatusID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.typeLetter).HasColumnName("typeLetter");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
