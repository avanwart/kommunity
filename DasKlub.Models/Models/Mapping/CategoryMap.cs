using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.categoryID);

            // Properties
            this.Property(t => t.categoryKey)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.name)
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Category");
            this.Property(t => t.categoryID).HasColumnName("categoryID");
            this.Property(t => t.categoryKey).HasColumnName("categoryKey");
            this.Property(t => t.departmentID).HasColumnName("departmentID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}
