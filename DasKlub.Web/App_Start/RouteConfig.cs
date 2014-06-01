using System.Web.Mvc;
using System.Web.Routing;

namespace DasKlub.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*favicon}", new {favicon = "(.*/)?favicon.ico(/.*)"});

            #region store

            routes.MapRoute(
                "store_index",
                "store",
                new
                {
                    controller = "Store",
                    action = "Index",
                }
                );

            #endregion

            #region video

            routes.MapRoute(
                "live_video_detail",
                "video/cat/{cat}",
                new
                {
                    controller = "Video",
                    action = "ForumCategory",
                    id = ""
                }
                );


            routes.MapRoute(
                "video_items_post",
                "video/items",
                new
                {
                    controller = "Video",
                    action = "Items",
                }
                );

            routes.MapRoute(
                "video_items",
                "video/items/{pageNumber}",
                new
                {
                    controller = "Video",
                    action = "Items",
                }
                );

            routes.MapRoute(
                "video_contests",
                "video/contests",
                new
                {
                    controller = "Video",
                    action = "Contests"
                }
                );

            routes.MapRoute(
                "video_contest",
                "video/contest/{key}",
                new
                {
                    controller = "Video",
                    action = "Contest"
                }
                );

            #endregion

            #region find users

            routes.MapRoute(
                "find_users_items",
                "findusers/items/{pageNumber}",
                new
                {
                    controller = "FindUsers",
                    action = "Items",
                }
                );

            routes.MapRoute(
                "find_users_post",
                "findusers/items",
                new
                {
                    controller = "FindUsers",
                    action = "Items",
                }
                );

            routes.MapRoute(
                "browse_user_filter",
                "findusers",
                new
                {
                    controller = "FindUsers",
                    action = "Index",
                    id = ""
                }
                );

            #endregion

            #region account

            routes.MapRoute(
                "statusupdates_items",
                "account/statusupdates/{pageNumber}",
                new
                {
                    controller = "Account",
                    action = "StatusUpdates",
                }
                );


            routes.MapRoute(
                "status_update",
                "account/statusupdate/{statusUpdateID}",
                new
                {
                    controller = "Account",
                    action = "StatusUpdate"
                }
                );


            routes.MapRoute(
                "profile_user_visitors",
                "account/profilevisitors/{userName}",
                new
                {
                    controller = "Account",
                    action = "ProfileVisitors"
                }
                );


            routes.MapRoute(
                "user_visitors",
                "account/visitors",
                new
                {
                    controller = "Account",
                    action = "Visitors"
                }
                );


            routes.MapRoute(
                "user_cyber_contacts",
                "account/CyberAssociates/{userName}",
                new
                {
                    controller = "Account",
                    action = "CyberAssociates"
                }
                );


            routes.MapRoute(
                "user_irl_contacts",
                "account/irlcontacts/{userName}",
                new
                {
                    controller = "Account",
                    action = "IRLContacts"
                }
                );


            routes.MapRoute(
                "user_wish_list",
                "account/wishlist/{userName}",
                new
                {
                    controller = "Account",
                    action = "WishList",
                    id = ""
                }
                );

            //routes.MapRoute(
            //"delete_account_photo",
            //"account/photodelete/{number}",
            //new
            //{
            //    controller = "Account",
            //    action = "PhotoDelete",
            //    id = ""
            //}
            //);

            #endregion

            routes.MapRoute(
                "market_brand",
                "market/brand/{brandKey}",
                new
                {
                    controller = "Market",
                    action = "Brands",
                    id = ""
                }
                );


            routes.MapRoute(
                "shopping_cart",
                "market/shoppingcart",
                new
                {
                    controller = "Market",
                    action = "ShoppingCart",
                    id = ""
                }
                );


            routes.MapRoute(
                "shopping_cart_removeitem",
                "market/removeitem/{productVariationID}",
                new
                {
                    controller = "Market",
                    action = "RemoveItem",
                    id = ""
                }
                );


            routes.MapRoute(
                "content_branding",
                "SiteContent",
                new
                {
                    controller = "Home",
                    action = "SiteContent"
                }
                );


            routes.MapRoute(
                "user_list",
                "users",
                new
                {
                    controller = "Users",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "lesson_index",
                "lessons",
                new
                {
                    controller = "Lessons",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "contact_index",
                "contact",
                new
                {
                    controller = "Home",
                    action = "Contact"
                }
                );

            #region news

            routes.MapRoute(
                "news_index",
                "news",
                new
                {
                    controller = "News",
                    action = "Index"
                }
                );

            routes.MapRoute(
                "news_index_lang",
                "news/lang/{lang}",
                new
                {
                    controller = "News",
                    action = "Lang"
                }
                );


            routes.MapRoute(
                "news_items_lang",
                "news/langitems/{lang}/{pageNumber}",
                new
                {
                    controller = "News",
                    action = "LangItems",
                }
                );


            routes.MapRoute(
                "video_log",
                "news/videolog",
                new
                {
                    controller = "News",
                    action = "VideoLog"
                }
                );


            routes.MapRoute(
                "news_items",
                "news/items/{pageNumber}",
                new
                {
                    controller = "News",
                    action = "Items",
                }
                );


            routes.MapRoute(
                "news_delete",
                "news/deletecomment",
                new
                {
                    controller = "News",
                    action = "DeleteComment",
                }
                );

            routes.MapRoute(
                "news_items_post",
                "news/items",
                new
                {
                    controller = "News",
                    action = "Items",
                }
                );


            routes.MapRoute(
                "news_tag",
                "news/tag/{key}",
                new
                {
                    controller = "News",
                    action = "Tag",
                }
                );


            routes.MapRoute(
                "news_tag_items_post",
                "news/tagitems/{key}",
                new
                {
                    controller = "News",
                    action = "TagItems",
                }
                );

            routes.MapRoute(
                "news_tag_items",
                "news/tagitems/{key}/{pageNumber}",
                new
                {
                    controller = "News",
                    action = "TagItems",
                }
                );

            routes.MapRoute(
                "news_detail",
                "news/{key}",
                new
                {
                    controller = "News",
                    action = "Detail",
                }
                );

            #endregion

            routes.MapRoute(
                "market_brand_index",
                "market/BrandList",
                new
                {
                    controller = "Market",
                    action = "BrandList"
                }
                );

            #region mail

            routes.MapRoute(
                "mail_items",
                "account/mailitems/{pageNumber}",
                new
                {
                    controller = "Account",
                    action = "MailItems",
                }
                );

            routes.MapRoute(
                "reply_mail_items",
                "account/replymailitems/{pageNumber}",
                new
                {
                    controller = "Account",
                    action = "ReplyMailItems",
                }
                );
            routes.MapRoute(
                "oubtox_mail_items",
                "account/outboxmailitems/{pageNumber}",
                new
                {
                    controller = "Account",
                    action = "OutboxMailItems",
                }
                );

            #endregion

            routes.MapRoute(
                "search_results",
                "search",
                new
                {
                    controller = "Search",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "tour_feeds_index",
                "feeds/tour",
                new
                {
                    controller = "Feeds",
                    action = "Tour"
                }
                );


            routes.MapRoute(
                "video_index",
                "video",
                new
                {
                    controller = "Video",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "buy_index",
                "buy",
                new
                {
                    controller = "Buy",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "band_index",
                "bands",
                new
                {
                    controller = "Bands",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "band_filter",
                "video/bands/{firstLetter}",
                new
                {
                    controller = "Video",
                    action = "Filter"
                }
                );


            routes.MapRoute(
                "message_reply",
                "account/reply/{userName}",
                new
                {
                    controller = "Account",
                    action = "Reply"
                }
                );


            routes.MapRoute(
                "user_filter",
                "video/users/{firstLetter}",
                new
                {
                    controller = "Video",
                    action = "UserFilter"
                }
                );


            routes.MapRoute(
                "market_detail",
                "market/{productKey}",
                new {controller = "Market", action = "Detail", id = ""}
                );


            routes.MapRoute(
                "market_index",
                "market",
                new
                {
                    controller = "Market",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "map_index",
                "map",
                new
                {
                    controller = "Map",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "radio_index",
                "radio",
                new
                {
                    controller = "Radio",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "kalendar_index",
                "kalendar",
                new
                {
                    controller = "Kalendar",
                    action = "Index"
                }
                );


            routes.MapRoute(
                "live_video",
                "video/live",
                new {controller = "Video", action = "Live", id = ""}
                );


            routes.MapRoute(
                "about_us",
                "about",
                new {controller = "Home", action = "About", id = ""}
                );


            routes.MapRoute(
                "video_detail",
                "video/{videoContext}",
                new {controller = "Video", action = "Detail", id = ""}
                );


            routes.MapRoute(
                "users_handler",
                "Handler",
                new
                {
                    controller = "Account",
                    action = "Handler"
                }
                );


            routes.MapRoute(
                "wall_user_detail",
                "{userName}/WallMessages/{pageNumber}",
                new {controller = "Profile", action = "WallMessages"}
                );


            routes.MapRoute(
                "wall_user_delete",
                "{userName}/DeleteWallItem/{wallItemID}",
                new {controller = "Profile", action = "DeleteWallItem"}
                );

            #region photos

            routes.MapRoute(
                "photo_list",
                "photos",
                new
                {
                    controller = "Photos",
                    action = "Index"
                }
                );

            routes.MapRoute(
                "photo_list_page",
                "photos/PhotoItems",
                new
                {
                    controller = "Photos",
                    action = "PhotoItems",
                    id = ""
                }
                );


            routes.MapRoute(
                "photo_list_page2",
                "photos/PhotoItems/{pageNumber}",
                new
                {
                    controller = "Photos",
                    action = "PhotoItems",
                }
                );


            routes.MapRoute(
                "photo_item",
                "photos/{photoItemID}",
                new
                {
                    controller = "Photos",
                    action = "Detail",
                    id = ""
                }
                );


            routes.MapRoute(
                "photo_delete",
                "photos/delete/{photoItemID}",
                new
                {
                    controller = "Photos",
                    action = "Delete",
                    id = ""
                }
                );

            #endregion

            routes.MapRoute(
                "user_news",
                "{userName}/news",
                new
                {
                    controller = "Profile",
                    action = "UserNews"
                }
                );


            routes.MapRoute(
                "user_photos",
                "{userName}/userphotos",
                new
                {
                    controller = "Profile",
                    action = "UserPhotos"
                }
                );


            routes.MapRoute(
                "user_photo_item",
                "{userName}/userphoto/{photoItemID}",
                new
                {
                    controller = "Profile",
                    action = "UserPhoto"
                }
                );

            #region forum

            routes.MapRoute("delete_forum_post", "forum/deleteforumpost/{forumPostID}",
                new
                {
                    controller = "Forum",
                    action = "DeleteForumPost"
                }
                );


            routes.MapRoute("delete_sub_forum", "forum/deletesubforum/{forumSubCategoryID}",
                new
                {
                    controller = "Forum",
                    action = "DeleteSubForum"
                }
                );

            routes.MapRoute("forum_create", "forum/create",
                new
                {
                    controller = "Forum",
                    action = "CreateForum"
                }
                );


            routes.MapRoute("forum", "forum",
                new
                {
                    controller = "Forum",
                    action = "Index"
                }
                );

            routes.MapRoute("sub_forum_create", "forum/{key}/create",
                new
                {
                    controller = "Forum",
                    action = "CreateSubCategory"
                }
                );

            routes.MapRoute("sub_forum_edit", "forum/{key}/{subKey}/edit",
                new
                {
                    controller = "Forum",
                    action = "EditSubCategory"
                }
                );

            routes.MapRoute("sub_forum_post", "forum/{key}/{subKey}",
                new
                {
                    controller = "Forum",
                    action = "ForumPost"
                }
                );

            routes.MapRoute("sub_forum_post_edit", "forum/{key}/{subKey}/{forumPostID}/edit",
                new
                {
                    controller = "Forum",
                    action = "EditForumPost"
                }
                );


            routes.MapRoute("sub_forum_page1", "forum/{key}/{pageNumber}",
                new
                {
                    controller = "Forum",
                    action = "SubCategory"
                }
                );

            routes.MapRoute("sub_forum_page2", "forum/{key}/page/{pageNumber}",
                new
                {
                    controller = "Forum",
                    action = "SubCategory"
                }
                );


            routes.MapRoute("sub_forum_post_create", "forum/{key}/{subKey}/create",
                new
                {
                    controller = "Forum",
                    action = "CreateForumPost"
                }
                );


            routes.MapRoute("sub_forum", "forum/{key}",
                new
                {
                    controller = "Forum",
                    action = "SubCategory"
                }
                );


            routes.MapRoute("sub_forum_post_page", "forum/{key}/{subKey}/{pageNumber}",
                new
                {
                    controller = "Forum",
                    action = "ForumPost"
                }
                );

            #endregion

            // dispays the user profile
            routes.MapRoute(
                "user_detail",
                "{userName}",
                new {controller = "Profile", action = "ProfileDetail"}
                );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }
    }
}