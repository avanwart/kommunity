//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 23-08-08
//===========================================================================================


#region Using

using System;
using System.Web;
using System.IO.Compression;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;

#endregion

namespace Miron.Web.MbCompression
{
    /// <summary>
    /// HttpModule to compress the output of System.Web.UI.Page handler using gzip/deflate algorithm,
    /// corresponding to the client browser support.
    /// <para>
    /// </para>
    /// </summary>
    public sealed class MbCompressionModule : IHttpModule
    {
        Settings settings;

        #region IHttpModule Members

        /// <summary>
        /// Release resources used by the module (Nothing realy in our case), but must implement (interface).
        /// </summary>
        void IHttpModule.Dispose()
        {
            // Nothing to dispose in our case;
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context"></param>
        void IHttpModule.Init(HttpApplication context)
        {
            settings = Settings.Instance;
            context.PostReleaseRequestState += new EventHandler(OnPostReleaseRequestState);
        }

        #endregion


        #region  // Page Compression

        /// <summary>
        /// Handles the PostReleaseRequestState event.
        /// </summary>
        /// <param name="sender">The object that raised the event (HttpApplication)</param>
        /// <param name="e">The event data</param>
        void OnPostReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            if (app.Context == null)
                return;

            // This part of the module compress only handlers from type System.Web.UI.Page
            // Other types such JavaScript or CSS files will be compressed in an httpHandelr.
            // Here we check if the current handler if a Page, if so, we compress it.
            if (app.Context.CurrentHandler is System.Web.UI.Page && app.Context.Response != null && Util.IsUIPageContentType(app.Context.Response.ContentType))
            {
                // Check if the path is not excluded.
                if (!settings.IsValidPath(app.Request.AppRelativeCurrentExecutionFilePath))
                    return;

                // Check if the mime type is not ecluded. (Use to exclude pages that generate specific mime type (such image or Excel...))
                if (!settings.IsValidType(app.Response.ContentType))
                    return;

                // Because there is a problem with async postbacks compression, we check here if the current request if an 'MS AJAX' call.
                // If so, we will not compress it.
                if (settings.CompressPage && !(Util.IsMsAjaxRequest(app.Context) && settings.MSAjaxVersion < 3.5))
                {
                    EncodingManager encodingMgr = new EncodingManager(app.Context);

                    if (encodingMgr.IsEncodingEnabled)
                    {
                        encodingMgr.CompressResponse();
                        encodingMgr.SetResponseEncodingType();
                    }
                }

                if (settings.AutoMode)
                {
                    bool processCss = settings.CompressCSS || settings.MinifyContent;
                    bool processJs = settings.CompressJavaScript || settings.MinifyContent;
                    app.Response.Filter = new AutoModeFilterStream(app.Response.Filter,app.Response.ContentEncoding, processJs,processCss);
                }
            }
        }

        #endregion

    }
}