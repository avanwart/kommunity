using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class AcknowledgementMap : EntityTypeConfiguration<Acknowledgement>
    {
        public AcknowledgementMap()
        {
            // Primary Key
            this.HasKey(t => t.acknowledgementID);

            // Properties
            this.Property(t => t.acknowledgementType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Acknowledgement");
            this.Property(t => t.acknowledgementID).HasColumnName("acknowledgementID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.acknowledgementType).HasColumnName("acknowledgementType");

            // Relationships
            this.HasRequired(t => t.StatusUpdate)
                .WithMany(t => t.Acknowledgements)
                .HasForeignKey(d => d.statusUpdateID);

        }
    }
}
