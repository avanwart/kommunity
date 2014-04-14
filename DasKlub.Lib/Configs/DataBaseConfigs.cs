using System.Configuration;

namespace DasKlub.Lib.Configs
{
    public static class DataBaseConfigs
    {
        #region constructors

        static DataBaseConfigs()
        {
            const string connectionStringName = "DasKlubDB";
            // database
            _dbConnectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _dbProviderName =
                ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
        }

        #endregion

        #region variables

        // database
        private static string _dbConnectionString = string.Empty;
        private static readonly string _dbProviderName = string.Empty;

        #endregion

        #region database

        /// <summary>
        ///     The connectin string to the database
        /// </summary>
        public static string DbConnectionString
        {
            set { _dbConnectionString = value; }

            get { return _dbConnectionString; }
        }

        public static string DbProviderName
        {
            get { return _dbProviderName; }
        }

        #endregion
    }
}