using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ErrorLogMap : EntityTypeConfiguration<ErrorLog>
    {
        public ErrorLogMap()
        {
            // Primary Key
            this.HasKey(t => t.errorLogID);

            // Properties
            this.Property(t => t.url)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ErrorLog");
            this.Property(t => t.errorLogID).HasColumnName("errorLogID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.message).HasColumnName("message");
            this.Property(t => t.url).HasColumnName("url");
            this.Property(t => t.responseCode).HasColumnName("responseCode");
        }
    }
}
