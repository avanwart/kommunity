using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class Log4NetMap : EntityTypeConfiguration<Log4Net>
    {
        public Log4NetMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Date, t.Thread, t.Level, t.Logger, t.Message });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Thread)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Level)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Logger)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.Exception)
                .HasMaxLength(2000);

            this.Property(t => t.Location)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Log4Net");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Thread).HasColumnName("Thread");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Logger).HasColumnName("Logger");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Exception).HasColumnName("Exception");
            this.Property(t => t.Location).HasColumnName("Location");
        }
    }
}
