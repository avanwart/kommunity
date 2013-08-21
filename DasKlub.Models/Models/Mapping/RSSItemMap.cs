using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RSSItemMap : EntityTypeConfiguration<RSSItem>
    {
        public RSSItemMap()
        {
            // Primary Key
            this.HasKey(t => t.rssItemID);

            // Properties
            this.Property(t => t.title)
                .IsRequired();

            this.Property(t => t.languageName)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("RSSItem");
            this.Property(t => t.rssItemID).HasColumnName("rssItemID");
            this.Property(t => t.rssResourceID).HasColumnName("rssResourceID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.authorName).HasColumnName("authorName");
            this.Property(t => t.commentsURL).HasColumnName("commentsURL");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.pubDate).HasColumnName("pubDate");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.languageName).HasColumnName("languageName");
            this.Property(t => t.artistID).HasColumnName("artistID");
            this.Property(t => t.link).HasColumnName("link");
            this.Property(t => t.guidLink).HasColumnName("guidLink");

            // Relationships
            this.HasRequired(t => t.RssResource)
                .WithMany(t => t.RSSItems)
                .HasForeignKey(d => d.rssResourceID);

        }
    }
}
