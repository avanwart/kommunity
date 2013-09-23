using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DasKlub.Models.Models;
using DasKlubModel.Models.Mapping;

namespace DasKlubModel.Models
{
    public partial class DasKlubDBContext : DbContext
    {
        static DasKlubDBContext()
        {
            Database.SetInitializer<DasKlubDBContext>(null);
        }

        public DasKlubDBContext()
            : base("Name=DasKlubDBContext")
        {
        }

        public DbSet<Acknowledgement> Acknowledgements { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistEvent> ArtistEvents { get; set; }
        public DbSet<ArtistProperty> ArtistProperties { get; set; }
        public DbSet<BlackIPID> BlackIPIDs { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
        public DbSet<ClickAudit> ClickAudits { get; set; }
        public DbSet<ClickLog> ClickLogs { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentComment> ContentComments { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestVideo> ContestVideos { get; set; }
        public DbSet<ContestVideoVote> ContestVideoVotes { get; set; }
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCycle> EventCycles { get; set; }
        public DbSet<HostedVideoLog> HostedVideoLogs { get; set; }
        public DbSet<InterestedIn> InterestedIns { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Log4Net> Log4Net { get; set; }
        public DbSet<MultiProperty> MultiProperties { get; set; }
        public DbSet<PhotoItem> PhotoItems { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistVideo> PlaylistVideos { get; set; }
        public DbSet<ProfileLog> ProfileLogs { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<RelationshipStatu> RelationshipStatus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RSSItem> RSSItems { get; set; }
        public DbSet<RssResource> RssResources { get; set; }
        public DbSet<SiteComment> SiteComments { get; set; }
        public DbSet<SiteDomain> SiteDomains { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SizeType> SizeTypes { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongProperty> SongProperties { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusComment> StatusComments { get; set; }
        public DbSet<StatusCommentAcknowledgement> StatusCommentAcknowledgements { get; set; }
        public DbSet<StatusUpdate> StatusUpdates { get; set; }
        public DbSet<StatusUpdateNotification> StatusUpdateNotifications { get; set; }
        public DbSet<UserAccountEntity> UserAccounts { get; set; }
        public DbSet<UserAccountDetailEntity> UserAccountDetails { get; set; }
        public DbSet<UserAccountMet> UserAccountMets { get; set; }
        public DbSet<UserAccountVideo> UserAccountVideos { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<UserPhoto> UserPhotoes { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoLog> VideoLogs { get; set; }
        public DbSet<VideoRequest> VideoRequests { get; set; }
        public DbSet<VideoSong> VideoSongs { get; set; }
        public DbSet<VideoVote> VideoVotes { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<WallMessage> WallMessages { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<YouAre> YouAres { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<vwUserSearchFilter> vwUserSearchFilters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AcknowledgementMap());
            modelBuilder.Configurations.Add(new ArtistMap());
            modelBuilder.Configurations.Add(new ArtistEventMap());
            modelBuilder.Configurations.Add(new ArtistPropertyMap());
            modelBuilder.Configurations.Add(new BlackIPIDMap());
            modelBuilder.Configurations.Add(new BlockedUserMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ChatRoomMap());
            modelBuilder.Configurations.Add(new ChatRoomUserMap());
            modelBuilder.Configurations.Add(new ClickAuditMap());
            modelBuilder.Configurations.Add(new ClickLogMap());
            modelBuilder.Configurations.Add(new ColorMap());
            modelBuilder.Configurations.Add(new ContentMap());
            modelBuilder.Configurations.Add(new ContentCommentMap());
            modelBuilder.Configurations.Add(new ContentTypeMap());
            modelBuilder.Configurations.Add(new ContestMap());
            modelBuilder.Configurations.Add(new ContestVideoMap());
            modelBuilder.Configurations.Add(new ContestVideoVoteMap());
            modelBuilder.Configurations.Add(new DirectMessageMap());
            modelBuilder.Configurations.Add(new ErrorLogMap());
            modelBuilder.Configurations.Add(new EventMap());
            modelBuilder.Configurations.Add(new EventCycleMap());
            modelBuilder.Configurations.Add(new HostedVideoLogMap());
            modelBuilder.Configurations.Add(new InterestedInMap());
            modelBuilder.Configurations.Add(new LanguageMap());
            modelBuilder.Configurations.Add(new Log4NetMap());
            modelBuilder.Configurations.Add(new MultiPropertyMap());
            modelBuilder.Configurations.Add(new PhotoItemMap());
            modelBuilder.Configurations.Add(new PlaylistMap());
            modelBuilder.Configurations.Add(new PlaylistVideoMap());
            modelBuilder.Configurations.Add(new ProfileLogMap());
            modelBuilder.Configurations.Add(new PropertyTypeMap());
            modelBuilder.Configurations.Add(new RelationshipStatuMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RSSItemMap());
            modelBuilder.Configurations.Add(new RssResourceMap());
            modelBuilder.Configurations.Add(new SiteCommentMap());
            modelBuilder.Configurations.Add(new SiteDomainMap());
            modelBuilder.Configurations.Add(new SizeMap());
            modelBuilder.Configurations.Add(new SizeTypeMap());
            modelBuilder.Configurations.Add(new SongMap());
            modelBuilder.Configurations.Add(new SongPropertyMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new StatusCommentMap());
            modelBuilder.Configurations.Add(new StatusCommentAcknowledgementMap());
            modelBuilder.Configurations.Add(new StatusUpdateMap());
            modelBuilder.Configurations.Add(new StatusUpdateNotificationMap());
            modelBuilder.Configurations.Add(new UserAccountMap());
            modelBuilder.Configurations.Add(new UserAccountDetailMap());
            modelBuilder.Configurations.Add(new UserAccountMetMap());
            modelBuilder.Configurations.Add(new UserAccountVideoMap());
            modelBuilder.Configurations.Add(new UserAddressMap());
            modelBuilder.Configurations.Add(new UserConnectionMap());
            modelBuilder.Configurations.Add(new UserPhotoMap());
            modelBuilder.Configurations.Add(new VenueMap());
            modelBuilder.Configurations.Add(new VideoMap());
            modelBuilder.Configurations.Add(new VideoLogMap());
            modelBuilder.Configurations.Add(new VideoRequestMap());
            modelBuilder.Configurations.Add(new VideoSongMap());
            modelBuilder.Configurations.Add(new VideoVoteMap());
            modelBuilder.Configurations.Add(new VoteMap());
            modelBuilder.Configurations.Add(new WallMessageMap());
            modelBuilder.Configurations.Add(new WishListMap());
            modelBuilder.Configurations.Add(new YouAreMap());
            modelBuilder.Configurations.Add(new ZoneMap());
            modelBuilder.Configurations.Add(new vwUserSearchFilterMap());
        }
    }
}
