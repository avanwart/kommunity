using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SiteCommentMap : EntityTypeConfiguration<SiteComment>
    {
        public SiteCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.siteCommentID);

            // Properties
            // Table & Column Mappings
            this.ToTable("SiteComment");
            this.Property(t => t.siteCommentID).HasColumnName("siteCommentID");
            this.Property(t => t.detail).HasColumnName("detail");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}
