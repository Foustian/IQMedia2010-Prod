using System;
using System.Linq;
using IQMediaGroup.ExposeApi.Logic;
using System.Collections.Generic;
using System.Security.Authentication;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class AuthenticationLogic : BaseLogic, ILogic
    {
        public bool AuthenticateCustomer(string p_Email, string p_Password)
        {
            try
            {
                int Result = Convert.ToInt32(Context.AuthenticateCustomer(p_Email, p_Password).Single());

                if (Result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool AuthenticateCustomer(string p_LoginID, string p_Password, int p_MaxPasswordAttempts)
        {

            bool isAuthenticated = false;

            Dictionary<string, string> customerDetails = Context.GetCustomerDetailsForAuthentication(p_LoginID);

            if (customerDetails != null)
            {
                if (string.IsNullOrEmpty(customerDetails["PasswordAttempts"]))
                {
                    customerDetails["PasswordAttempts"] = "0";
                }

                var maxPasswordAttempts = p_MaxPasswordAttempts <= 0 ? 5 : p_MaxPasswordAttempts;

                if (Convert.ToInt32(customerDetails["PasswordAttempts"]) >= p_MaxPasswordAttempts)
                {
                    throw new AuthenticationException("You have exceeded the maximum number of attempts to authenticate with your credentials. contact: support@iqmediacorp.com");
                }

                isAuthenticated = IQMedia.Security.Authentication.VerifyPassword(p_Password, customerDetails["Password"]);

                if (!isAuthenticated || Convert.ToInt32(customerDetails["PasswordAttempts"]) > 0)
                {
                    Context.UpdatePasswordAttempts(p_LoginID, isAuthenticated);
                }
            }

            return isAuthenticated;
        }

        public Int64 GetCustomerIDByEmail(string p_Email)
        {
            try
            {
                Int64 CustomerID = Context.Customers.FirstOrDefault(_Customer => _Customer.Email.Equals(p_Email)).CustomerKey;

                return CustomerID;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool AuthenticateRoleAccess(IQMediaGroup.Common.Util.CommonConstants.Roles p_Role, Guid p_CustomerGuid)
        {
            try
            {
                var roleAccess = Context.CheckRoleAccessByCustomerGuidAndRoleName(p_CustomerGuid, p_Role.ToString()).FirstOrDefault();
                return roleAccess.HasValue ? roleAccess.Value : false;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool CheckFeedsAccess(Guid p_CustomerGUID, out Dictionary<string, bool> p_Roles)
        {
            var hasAccess = false;

            var roles = Context.GetCustomerFeedsRoles(p_CustomerGUID);
            /*var roles = new Dictionary<string, bool> { { "v4Feeds", true },
                                                        { "v4TV",true},
                                                        { "v4NM",false},
                                                        { "v4SM",true},
                                                        { "v4TW",true},
                                                        { "v4TM",true},
                                                        { "v4BLPM",true},
                                                        { "v4PQ",true},
                                                        {"NielsenData",true},
                                                        {"CompeteData",true}
                                                       };*/

            p_Roles = roles;

            if (!roles[CommonConstants.Roles.v4Feeds.ToString()])
            {
                hasAccess = false;
            }
            else
            {
                if (roles.Count(r => r.Key != CommonConstants.Roles.v4Feeds.ToString() && r.Value == true) > 0)
                {
                    hasAccess = true;
                }
            }

            return hasAccess;
        }
    }
}