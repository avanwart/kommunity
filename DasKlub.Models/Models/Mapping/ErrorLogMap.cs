using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ErrorLogMap : EntityTypeConfiguration<ErrorLog>
    {
        public ErrorLogMap()
        {
            // Primary Key
            HasKey(t => t.errorLogID);

            // Properties
            Property(t => t.url)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("ErrorLog");
            Property(t => t.errorLogID).HasColumnName("errorLogID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.message).HasColumnName("message");
            Property(t => t.url).HasColumnName("url");
            Property(t => t.responseCode).HasColumnName("responseCode");
        }
    }
}