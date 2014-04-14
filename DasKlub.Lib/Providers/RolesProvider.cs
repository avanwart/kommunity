using System;
using System.Collections;
using System.Linq;
using System.Web.Security;
using DasKlub.Lib.BOL;

namespace DasKlub.Lib.Providers
{
    public class DKRoleProvider : RoleProvider
    {
        #region Public non static methods

        public override string ApplicationName
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        /// <summary>
        ///     Given the username(s) and role(s), add the user to the role,
        ///     if the role the user being added to doesn't exist, the role is created,
        ///     if the username doesn't exist, they are not created
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var t in usernames)
            {
                var eu = new UserAccount(t);
                if (eu.UserAccountID <= 0) continue;
                foreach (var t1 in roleNames)
                {
                    UserAccount.AddUserToRole(eu.UserAccountID, t1);
                }
            }
        }

        /// <summary>
        ///     Make a new role by name, it will become lowercase
        /// </summary>
        /// <param name="roleName"></param>
        public override void CreateRole(string roleName)
        {
            Role.Create(roleName.ToLower());
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Get all the roles in the site in the database
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            return Role.GetAllRoles();
        }

        /// <summary>
        ///     Get all the roles for the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>an array of strings containing the roles</returns>
        public override string[] GetRolesForUser(string username)
        {
            return UserAccount.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var rle = new Role(roleName);

            var allRoles = new ArrayList();

            var users = UserAccountRole.GetUsersInRole(rle.RoleID);

            foreach (var ua1 in users) allRoles.Add(ua1.UserName);

            var stringArray = (string[]) allRoles.ToArray(typeof (string));

            return stringArray;
        }

        /// <summary>
        ///     Is this user in the specified role?
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns>true or false</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            var isUserInRole = false;

            var userRoles = GetRolesForUser(username);

            foreach (var s in userRoles.Where(s => roleName == s))
            {
                isUserInRole = true;
            }
            return isUserInRole;
        }

        /// <summary>
        ///     Remove the user(s) from the role(s)
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var eu in usernames.Select(t => new UserAccount(t)).Where(eu => eu.UserAccountID > 0))
            {
                foreach (var t in roleNames)
                {
                    UserAccount.DeleteUserFromRole(eu.UserAccountID, t);
                }
            }
        }

        /// <summary>
        ///     Is this a role in the site?
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            return Role.IsRole(roleName);
        }

        #endregion
    }
}