using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SiteCommentMap : EntityTypeConfiguration<SiteComment>
    {
        public SiteCommentMap()
        {
            // Primary Key
            HasKey(t => t.siteCommentID);

            // Properties
            // Table & Column Mappings
            ToTable("SiteComment");
            Property(t => t.siteCommentID).HasColumnName("siteCommentID");
            Property(t => t.detail).HasColumnName("detail");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}