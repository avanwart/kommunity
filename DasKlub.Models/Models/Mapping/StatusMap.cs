using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusMap : EntityTypeConfiguration<Status>
    {
        public StatusMap()
        {
            // Primary Key
            HasKey(t => t.statusID);

            // Properties
            Property(t => t.statusDescription)
                .HasMaxLength(250);

            Property(t => t.statusCode)
                .IsFixedLength()
                .HasMaxLength(5);

            // Table & Column Mappings
            ToTable("Status");
            Property(t => t.statusID).HasColumnName("statusID");
            Property(t => t.statusDescription).HasColumnName("statusDescription");
            Property(t => t.statusCode).HasColumnName("statusCode");
        }
    }
}