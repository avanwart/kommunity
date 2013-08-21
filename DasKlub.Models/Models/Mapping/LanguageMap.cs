using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class LanguageMap : EntityTypeConfiguration<Language>
    {
        public LanguageMap()
        {
            // Primary Key
            this.HasKey(t => t.languageID);

            // Properties
            this.Property(t => t.languageType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.languageName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Language");
            this.Property(t => t.languageID).HasColumnName("languageID");
            this.Property(t => t.languageType).HasColumnName("languageType");
            this.Property(t => t.languageName).HasColumnName("languageName");

            // Relationships
            this.HasMany(t => t.UserAccounts)
                .WithMany(t => t.Languages)
                .Map(m =>
                    {
                        m.ToTable("LanguageUserAccount");
                        m.MapLeftKey("languageID");
                        m.MapRightKey("userAccountID");
                    });


        }
    }
}
