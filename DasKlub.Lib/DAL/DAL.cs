using System;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.DAL
{
    public static class DbAct
    {
        #region Public Static methods

        #region core DB actions

        /// <summary>
        ///     Returns a DataSet (best used for multiple tables in the result set)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <see cref=">http://msdn.microsoft.com/en-us/library/fks3666w%28VS.80%29.aspx" />
        public static DataSet ExecuteMultipleTableSelectCommand(DbCommand command)
        {
            using (command)
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(DataBaseConfigs.DbProviderName);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    if (adapter == null) return null;

                    adapter.SelectCommand = command;

                    using (var ds = new DataSet())
                    {
                        try
                        {
                            adapter.Fill(ds);
                        }
                        catch (Exception ex)
                        {
                            Utilities.LogError(ex);

                            return null;
                        }

                        command.Connection.Close();
                        return ds;
                    }
                }
            }
        }


        /// <summary>
        ///     Execute a command and return the results as a DataTable object
        /// </summary>
        /// <param name="command">database command</param>
        public static DataTable ExecuteSelectCommand(DbCommand command)
        {
            DataTable table = null;

            using (command)
            {
                try
                {
                    command.Connection.Open();

                    table = new DataTable();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Utilities.LogError(ex);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Connection.Close();
                        command.Parameters.Clear();
                    }
                }
                return table;
            }
        }

        /// <summary>
        ///     Execute an update, delete, or insert command
        ///     and return the number of affected rows
        /// </summary>
        /// <param name="command">database command</param>
        public static int ExecuteNonQuery(DbCommand command)
        {
            int affectedRows = -1;

            using (command)
            {
                try
                {
                    command.Connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Utilities.LogError(ex);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Parameters.Clear();
                        command.Connection.Close();
                    }
                }
                return affectedRows;
            }
        }

        /// <summary>
        ///     Execute a select command and return a single result as a string
        /// </summary>
        /// <param name="command">database command</param>
        public static string ExecuteScalar(DbCommand command)
        {
            string value = string.Empty;

            using (command)
            {
                try
                {
                    command.Connection.Open();
                    object result = command.ExecuteScalar();
                    value = result != null ? Convert.ToString(result) : string.Empty;
                }
                catch (Exception ex)
                {
                    Utilities.LogError(ex);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Parameters.Clear();
                        command.Connection.Close();
                    }
                }
                return value;
            }
        }

        #endregion

        #region DBCommands

        /// <summary>
        ///     Creates and prepares a new DbCommand object
        ///     on a new connection using a StoredProcedure
        /// </summary>
        public static DbCommand CreateCommand()
        {
            string dataProviderName = DataBaseConfigs.DbProviderName;
            string connectionString = DataBaseConfigs.DbConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            DbConnection conn = factory.CreateConnection();

            if (conn != null)
            {
                DbCommand comm = conn.CreateCommand();
                conn.ConnectionString = connectionString;
                comm.CommandType = CommandType.StoredProcedure;
                return comm;
            }

            return null;
        }

        /// <summary>
        ///     Creates and prepares a new DbCommand object
        ///     on a new connection using choice
        /// </summary>
        /// <param name="isText"></param>
        /// <returns></returns>
        /// <remarks>allows the use of text or stored procedure for command type</remarks>
        public static DbCommand CreateCommand(bool isText)
        {
            string dataProviderName = DataBaseConfigs.DbProviderName;
            string connectionString = DataBaseConfigs.DbConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            DbConnection conn = factory.CreateConnection();

            if (conn != null)
            {
                DbCommand comm = conn.CreateCommand();
                conn.ConnectionString = connectionString;
                comm.CommandType = isText ? CommandType.Text : CommandType.StoredProcedure;
                return comm;
            }
            return null;
        }

        #endregion

        #endregion
    }
}