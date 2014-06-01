using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class vwUserSearchFilterMap : EntityTypeConfiguration<vwUserSearchFilter>
    {
        public vwUserSearchFilterMap()
        {
            // Primary Key
            HasKey(t => new {t.userAccountID, t.defaultLanguage, t.showOnMap});

            // Properties
            Property(t => t.userAccountID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.country)
                .IsFixedLength()
                .HasMaxLength(2);

            Property(t => t.defaultLanguage)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("vwUserSearchFilter");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.youAreID).HasColumnName("youAreID");
            Property(t => t.relationshipStatusID).HasColumnName("relationshipStatusID");
            Property(t => t.interestedInID).HasColumnName("interestedInID");
            Property(t => t.country).HasColumnName("country");
            Property(t => t.latitude).HasColumnName("latitude");
            Property(t => t.longitude).HasColumnName("longitude");
            Property(t => t.birthDate).HasColumnName("birthDate");
            Property(t => t.defaultLanguage).HasColumnName("defaultLanguage");
            Property(t => t.isOnline).HasColumnName("isOnline");
            Property(t => t.lastActivityDate).HasColumnName("lastActivityDate");
            Property(t => t.showOnMap).HasColumnName("showOnMap");
        }
    }
}