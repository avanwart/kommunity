using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class WallMessageMap : EntityTypeConfiguration<WallMessage>
    {
        public WallMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.wallMessageID);

            // Properties
            this.Property(t => t.message)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("WallMessage");
            this.Property(t => t.wallMessageID).HasColumnName("wallMessageID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.message).HasColumnName("message");
            this.Property(t => t.isRead).HasColumnName("isRead");
            this.Property(t => t.fromUserAccountID).HasColumnName("fromUserAccountID");
            this.Property(t => t.toUserAccountID).HasColumnName("toUserAccountID");
        }
    }
}
