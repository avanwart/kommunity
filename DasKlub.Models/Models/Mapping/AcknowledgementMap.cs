using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class AcknowledgementMap : EntityTypeConfiguration<Acknowledgement>
    {
        public AcknowledgementMap()
        {
            // Primary Key
            HasKey(t => t.acknowledgementID);

            // Properties
            Property(t => t.acknowledgementType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("Acknowledgement");
            Property(t => t.acknowledgementID).HasColumnName("acknowledgementID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.acknowledgementType).HasColumnName("acknowledgementType");

            // Relationships
            HasRequired(t => t.StatusUpdate)
                .WithMany(t => t.Acknowledgements)
                .HasForeignKey(d => d.statusUpdateID);
        }
    }
}