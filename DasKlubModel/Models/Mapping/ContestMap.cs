using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContestMap : EntityTypeConfiguration<Contest>
    {
        public ContestMap()
        {
            // Primary Key
            this.HasKey(t => t.contestID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(100);

            this.Property(t => t.contestKey)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Contest");
            this.Property(t => t.contestID).HasColumnName("contestID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.deadLine).HasColumnName("deadLine");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.beginDate).HasColumnName("beginDate");
            this.Property(t => t.contestKey).HasColumnName("contestKey");
        }
    }
}
