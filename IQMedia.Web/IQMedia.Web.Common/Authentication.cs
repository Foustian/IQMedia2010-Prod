using System;
using System.Web;
using System.Web.Security;
using IQMedia.Common.Util;
using IQMedia.Domain;
using IQMedia.Logic;

namespace IQMedia.Web.Common
{
    public static class Authentication
    {
        [ThreadStatic]
        private static User _user;

        /// <summary>
        /// Gets the currently logged-in user. If not authenticated, returns the Anonymous User.
        /// </summary>
        /// <value>The current user.</value>
        public static User CurrentUser
        {
            get
            {
                if (_user != null) return _user;
                try
                {
                    var userLgc = (UserLogic)LogicFactory.GetLogic(LogicType.User);
                    _user = (IsAuthenticated)
                                ? userLgc.GetUserByUsername(HttpContext.Current.User.Identity.Name)
                                : userLgc.GetUser(Guid.Empty);
                    return _user;
                }
                catch (Exception ex)
                {
                    Logger.Error("An error occurred while attempting to retrieve the current user.", ex);
                    return null;
                }
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }


        #region Login/Out Methods

        public static LoginStatus Login(string email, string password)
        {
            try
            {
                var user = Membership.GetUser(email);

                if (user == null)
                    return LoginStatus.InvalidUser;
                if (user.IsLockedOut)
                    return LoginStatus.LockedOut;
                if (!user.IsApproved)
                    return LoginStatus.NotApproved;
                if (Membership.ValidateUser(email, password))
                {
                    //We've validated, so set the auth cookie
                    var ck = FormsAuthentication.GetAuthCookie(user.UserName, true);
                    //ck.Domain = "/";
                    HttpContext.Current.Response.Cookies.Add(ck);

                    return LoginStatus.Success; //new[] { "0", ((Guid?)user.ProviderUserKey).ToString() };
                }
                else
                    return LoginStatus.InvalidPassword;

            }
            catch(Exception ex)
            {
                Logger.Error("An error occurred while authenticating a user.", ex);
                return LoginStatus.Exception;
            }
        }

        public static void Logout()
        {
            if (null != HttpContext.Current.Session)
                HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }


        /// <summary>
        /// A nested enum of potential results from the login function.
        /// </summary>
        public enum LoginStatus
        {
            Success,
            Exception,
            InvalidUser,
            InvalidPassword,
            LockedOut,
            NotApproved,
            NotActive
        }

        #endregion
    }
}
