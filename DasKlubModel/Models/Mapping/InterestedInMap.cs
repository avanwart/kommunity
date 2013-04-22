using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class InterestedInMap : EntityTypeConfiguration<InterestedIn>
    {
        public InterestedInMap()
        {
            // Primary Key
            this.HasKey(t => t.interestedInID);

            // Properties
            this.Property(t => t.typeLetter)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("InterestedIn");
            this.Property(t => t.interestedInID).HasColumnName("interestedInID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.typeLetter).HasColumnName("typeLetter");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
