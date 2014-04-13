using System;
using System.Web;

namespace DasKlub.Lib.HttpModules
{
    /// <summary>
    ///     For this to work,
    ///     make sure the application pool is configured to Integreated mode (which is the default)
    /// </summary>
    public class HttpHeaderCleanup : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private static void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Headers.Remove("Server");
            }
            catch //(PlatformNotSupportedException ex)
            {
                // cannot do anything
            }
        }

        #endregion
    }
}