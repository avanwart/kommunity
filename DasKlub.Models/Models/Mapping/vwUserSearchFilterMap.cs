using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class vwUserSearchFilterMap : EntityTypeConfiguration<vwUserSearchFilter>
    {
        public vwUserSearchFilterMap()
        {
            // Primary Key
            this.HasKey(t => new { t.userAccountID, t.defaultLanguage, t.showOnMap });

            // Properties
            this.Property(t => t.userAccountID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.country)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.defaultLanguage)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("vwUserSearchFilter");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.youAreID).HasColumnName("youAreID");
            this.Property(t => t.relationshipStatusID).HasColumnName("relationshipStatusID");
            this.Property(t => t.interestedInID).HasColumnName("interestedInID");
            this.Property(t => t.country).HasColumnName("country");
            this.Property(t => t.latitude).HasColumnName("latitude");
            this.Property(t => t.longitude).HasColumnName("longitude");
            this.Property(t => t.birthDate).HasColumnName("birthDate");
            this.Property(t => t.defaultLanguage).HasColumnName("defaultLanguage");
            this.Property(t => t.isOnline).HasColumnName("isOnline");
            this.Property(t => t.lastActivityDate).HasColumnName("lastActivityDate");
            this.Property(t => t.showOnMap).HasColumnName("showOnMap");
        }
    }
}
