using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class LanguageMap : EntityTypeConfiguration<Language>
    {
        public LanguageMap()
        {
            // Primary Key
            HasKey(t => t.languageID);

            // Properties
            Property(t => t.languageType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            Property(t => t.languageName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Language");
            Property(t => t.languageID).HasColumnName("languageID");
            Property(t => t.languageType).HasColumnName("languageType");
            Property(t => t.languageName).HasColumnName("languageName");

            // Relationships
            HasMany(t => t.UserAccounts)
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