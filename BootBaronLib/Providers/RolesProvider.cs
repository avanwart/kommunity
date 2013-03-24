//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using System.Collections;


namespace DK.ASPNET.DKRoles
{
    public class DKRoleProvider : RoleProvider
    {
        #region Public non static methods

        /// <summary>
        /// Given the username(s) and role(s), add the user to the role,
        /// if the role the user being added to doesn't exist, the role is created,
        /// if the username doesn't exist, they are not created
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            UserAccount eu;
            for (int i = 0; i < usernames.Length; i++)
            {
                eu = new UserAccount(usernames[i]);
                if (eu.UserAccountID > 0)
                {
                    for (int j = 0; j < roleNames.Length; j++)
                    {
                        UserAccount.AddUserToRole(eu.UserAccountID, roleNames[j]);
                    }
                }
            }
        }

        /// <summary>
        /// TODO: FIND OUT WHY AND HOW THIS MUST BE IMPLEMENTED
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return string.Empty;
                //throw new NotImplementedException();
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Make a new role by name, it will become lowercase
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
        /// Get all the roles in the site in the database
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            return Role.GetAllRoles();
        }

        /// <summary>
        /// Get all the roles for the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>an array of strings containing the roles</returns>
        public override string[] GetRolesForUser(string username)
        {
            return UserAccount.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            Role rle = new Role(roleName);

            ArrayList allRoles = new ArrayList();

            UserAccounts uas = UserAccountRole.GetUsersInRole(rle.RoleID);

            foreach (UserAccount ua1 in uas) allRoles.Add(ua1.UserName);

            string[] stringArray = (string[])allRoles.ToArray(typeof(string));

            return stringArray;
        }

        /// <summary>
        /// Is this user in the specified role?
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns>true or false</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            bool isUserInRole = false;

            string[] userRoles = GetRolesForUser(username);
            foreach (string s in userRoles)
            {
                if (roleName == s) { isUserInRole = true; }
            }
            return isUserInRole;
        }

        /// <summary>
        /// Remove the user(s) from the role(s)
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            UserAccount eu;
            for (int i = 0; i < usernames.Length; i++)
            {
                eu = new UserAccount(usernames[i]);
                if (eu.UserAccountID > 0)
                {
                    for (int j = 0; j < roleNames.Length; j++)
                    {
                        UserAccount.DeleteUserFromRole(eu.UserAccountID, roleNames[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Is this a role in the site?
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
