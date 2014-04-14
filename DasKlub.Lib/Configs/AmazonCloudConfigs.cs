using System.Configuration;

namespace DasKlub.Lib.Configs
{
    public class AmazonCloudConfigs
    {
        #region variables

        private static readonly string _amazonAccessKey = string.Empty;
        private static readonly string _amazonSecretKey = string.Empty;
        private static readonly string _amazonCloudDomain = string.Empty;
        private static readonly string _amazonBucketName = string.Empty;
        private static readonly string _sendFromEmail = string.Empty;

        #endregion

        #region static constructor

        static AmazonCloudConfigs()
        {
            _sendFromEmail = ConfigurationManager.AppSettings["SendFromEmail"];
            _amazonAccessKey = ConfigurationManager.AppSettings["AmazonAccessKey"];
            _amazonSecretKey = ConfigurationManager.AppSettings["AmazonSecretKey"];
            _amazonCloudDomain = ConfigurationManager.AppSettings["AmazonCloudDomain"];
            _amazonBucketName = ConfigurationManager.AppSettings["AmazonBucketName"];
        }

        #endregion

        #region properties

        public static string SendFromEmail
        {
            get { return _sendFromEmail; }
        }


        public static string AmazonBucketName
        {
            get { return _amazonBucketName; }
        }

        public static string AmazonCloudDomain
        {
            get { return _amazonCloudDomain; }
        }

        public static string AmazonSecretKey
        {
            get { return _amazonSecretKey; }
        }


        public static string AmazonAccessKey
        {
            get { return _amazonAccessKey; }
        }

        #endregion
    }
}