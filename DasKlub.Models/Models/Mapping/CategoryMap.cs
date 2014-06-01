using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            HasKey(t => t.categoryID);

            // Properties
            Property(t => t.categoryKey)
                .IsRequired()
                .HasMaxLength(75);

            Property(t => t.name)
                .HasMaxLength(50);

            Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("Category");
            Property(t => t.categoryID).HasColumnName("categoryID");
            Property(t => t.categoryKey).HasColumnName("categoryKey");
            Property(t => t.departmentID).HasColumnName("departmentID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.description).HasColumnName("description");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}