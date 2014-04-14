using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class Role : BaseIUserLogCRUD, ICacheName
    {
        #region properties 

        private string _description = string.Empty;
        private string _roleName = string.Empty;
        public int RoleID { get; set; }

        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        public Role(string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRoleByName";

            comm.AddParameter("roleName", roleName);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);
            RoleID = FromObj.IntFromObj(dr["roleID"]);
            RoleName = FromObj.StringFromObj(dr["roleName"]);
            Description = FromObj.StringFromObj(dr["description"]);
        }


        /// <summary>
        ///     Make a new role if it doesn't exist
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool Create(string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            comm.AddParameter("roleName", roleName);

            // execute the stored procedure
            int result = Convert.ToInt32(DbAct.ExecuteScalar(comm));

            // was something returned?
            if (result != 0)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        ///     Get all the roles in the site in the database
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllRoles()
        {
            var allRoles = new ArrayList();
            DataTable dt;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllRoles";
            // exec
            dt = DbAct.ExecuteSelectCommand(comm);
            foreach (DataRow r in dt.Rows)
            {
                allRoles.Add(FromObj.StringFromObj(r["RoleName"]));
            }
            var stringArray = (string[]) allRoles.ToArray(typeof (string));
            return stringArray;
        }

        /// <summary>
        ///     Does this role name exist?
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool IsRole(string roleName)
        {
            return true;
            // if (roleName == string.Empty) return false; // name not given

            // // get a configured DbCommand object
            // DbCommand comm = DbAct.CreateCommand();
            // // set the stored procedure name
            ////  comm.CommandText = StoredProcedures.Name.up_IsRole.ToString();
            // // create a new parameter
            // DbParameter param = comm.CreateParameter();
            // // 
            // param.ParameterName = "@roleName";
            // param.Value = roleName;
            // param.DbType = DbType.String;
            // comm.Parameters.Add(param);

            // /// Exec
            // bool result = bool.TryParse(DbAct.ExecuteScalar(comm), out result);
            // return result;
        }

        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}