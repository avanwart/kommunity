using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class WishListMap : EntityTypeConfiguration<WishList>
    {
        public WishListMap()
        {
            // Primary Key
            HasKey(t => new {t.productID, t.createdByUserID});

            // Properties
            Property(t => t.productID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.createdByUserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("WishList");
            Property(t => t.productID).HasColumnName("productID");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
        }
    }
}