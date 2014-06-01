using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContestMap : EntityTypeConfiguration<Contest>
    {
        public ContestMap()
        {
            // Primary Key
            HasKey(t => t.contestID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(100);

            Property(t => t.contestKey)
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("Contest");
            Property(t => t.contestID).HasColumnName("contestID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.deadLine).HasColumnName("deadLine");
            Property(t => t.description).HasColumnName("description");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.beginDate).HasColumnName("beginDate");
            Property(t => t.contestKey).HasColumnName("contestKey");
        }
    }
}