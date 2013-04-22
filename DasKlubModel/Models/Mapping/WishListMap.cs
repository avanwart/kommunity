using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class WishListMap : EntityTypeConfiguration<WishList>
    {
        public WishListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.productID, t.createdByUserID });

            // Properties
            this.Property(t => t.productID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.createdByUserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("WishList");
            this.Property(t => t.productID).HasColumnName("productID");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}
