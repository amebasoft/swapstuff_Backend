using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using SwapStff.Service;
using System.Web.Mvc;

namespace PhotoContest.Web.Infrastructure.Providers
{
    public class AccountRoleProvider : RoleProvider
    {
        //private IUserService userService;
        public AccountRoleProvider()
        {
            //this.userService = DependencyResolver.Current.GetService<IUserService>();
        }
        public override string[] GetRolesForUser(string username)
        {
            var userService = DependencyResolver.Current.GetService<IUserService>();
            return userService.GetRoles(username);
        }
        #region Overrides of RoleProvider that throw NotImplementedException
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
