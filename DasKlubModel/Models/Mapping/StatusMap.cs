using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusMap : EntityTypeConfiguration<Status>
    {
        public StatusMap()
        {
            // Primary Key
            this.HasKey(t => t.statusID);

            // Properties
            this.Property(t => t.statusDescription)
                .HasMaxLength(250);

            this.Property(t => t.statusCode)
                .IsFixedLength()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Status");
            this.Property(t => t.statusID).HasColumnName("statusID");
            this.Property(t => t.statusDescription).HasColumnName("statusDescription");
            this.Property(t => t.statusCode).HasColumnName("statusCode");
        }
    }
}
