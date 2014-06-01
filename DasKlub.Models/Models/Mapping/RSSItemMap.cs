using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RSSItemMap : EntityTypeConfiguration<RSSItem>
    {
        public RSSItemMap()
        {
            // Primary Key
            HasKey(t => t.rssItemID);

            // Properties
            Property(t => t.title)
                .IsRequired();

            Property(t => t.languageName)
                .HasMaxLength(5);

            // Table & Column Mappings
            ToTable("RSSItem");
            Property(t => t.rssItemID).HasColumnName("rssItemID");
            Property(t => t.rssResourceID).HasColumnName("rssResourceID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.authorName).HasColumnName("authorName");
            Property(t => t.commentsURL).HasColumnName("commentsURL");
            Property(t => t.description).HasColumnName("description");
            Property(t => t.pubDate).HasColumnName("pubDate");
            Property(t => t.title).HasColumnName("title");
            Property(t => t.languageName).HasColumnName("languageName");
            Property(t => t.artistID).HasColumnName("artistID");
            Property(t => t.link).HasColumnName("link");
            Property(t => t.guidLink).HasColumnName("guidLink");

            // Relationships
            HasRequired(t => t.RssResource)
                .WithMany(t => t.RSSItems)
                .HasForeignKey(d => d.rssResourceID);
        }
    }
}