using System;
using System.Web.Security;
using IQMedia.Domain;
using IQMedia.Logic;

namespace IQMedia.Web.Common.Providers
{
    public class MembershipProvider : System.Web.Security.MembershipProvider
    {
        UserLogic userLgc = (UserLogic)LogicFactory.GetLogic(LogicType.User);

        /// <summary>
        /// Creates the membership user from a given IQMedia User.
        /// </summary>
        /// <param name="user">The IQMedia user.</param>
        /// <returns>System.Web.Security.MembershipUser</returns>
        private MembershipUser CreateMembershipUser(User user)
        {
            return new MembershipUser(Name, user.UserName, user.Guid, user.Email, String.Empty, user.Comment, true,
                                                  !user.IsActive, user.CreatedDate, DateTime.MinValue, DateTime.MinValue,
                                                  DateTime.MinValue, DateTime.MinValue);
        }

        #region Implemented Methods and Properties

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = userLgc.GetUserByUsername(username);
            
            return (user == null) ? null : CreateMembershipUser(user);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            User user;
            try
            {
                user = userLgc.GetUser((Guid) providerUserKey);
            }
            catch(Exception ex)
            {
                //The provider key must have failed casting
                user = null;
            }
            return (user == null) ? null : CreateMembershipUser(user);
        }

        public override string GetUserNameByEmail(string email)
        {
            var user = GetUser(email, true);
            return (user == null) ? null : user.UserName;
        }


        public override bool ValidateUser(string username, string password)
        {
            var user = userLgc.GetUserByUsername(username);
            if (user == null) return false;

            return (user.Password == password);
        }

        #endregion

        //For our implementation, we don't really need the methods below to be implemented
        //We don't leverage any of them and we're using this membership provider strictly
        //for cross-server authentication.
        #region Unimplemented Methods and Properties
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
