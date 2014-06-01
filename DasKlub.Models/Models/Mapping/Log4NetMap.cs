using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class Log4NetMap : EntityTypeConfiguration<Log4Net>
    {
        public Log4NetMap()
        {
            // Primary Key
            HasKey(t => new {t.Id, t.Date, t.Thread, t.Level, t.Logger, t.Message});

            // Properties
            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Thread)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Level)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.Logger)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(4000);

            Property(t => t.Exception)
                .HasMaxLength(2000);

            Property(t => t.Location)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("Log4Net");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.Thread).HasColumnName("Thread");
            Property(t => t.Level).HasColumnName("Level");
            Property(t => t.Logger).HasColumnName("Logger");
            Property(t => t.Message).HasColumnName("Message");
            Property(t => t.Exception).HasColumnName("Exception");
            Property(t => t.Location).HasColumnName("Location");
        }
    }
}