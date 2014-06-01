using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class WallMessageMap : EntityTypeConfiguration<WallMessage>
    {
        public WallMessageMap()
        {
            // Primary Key
            HasKey(t => t.wallMessageID);

            // Properties
            Property(t => t.message)
                .IsRequired();

            // Table & Column Mappings
            ToTable("WallMessage");
            Property(t => t.wallMessageID).HasColumnName("wallMessageID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.message).HasColumnName("message");
            Property(t => t.isRead).HasColumnName("isRead");
            Property(t => t.fromUserAccountID).HasColumnName("fromUserAccountID");
            Property(t => t.toUserAccountID).HasColumnName("toUserAccountID");
        }
    }
}