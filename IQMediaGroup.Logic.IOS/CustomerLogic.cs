using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Common.IOS.Util;
using IQMediaGroup.Domain.IOS;

namespace IQMediaGroup.Logic.IOS
{
    public class CustomerLogic : BaseLogic, ILogic
    {
        public readonly string FTPEncKey = "764164673344344D55624A6C486E4168";
        public readonly string FTPEncIV = "4957414F6B524147";
        public readonly string delimeter = "¶";

        public IsRegisteredOutput IsCustomerRegistered(MobileLogInInput p_MobileLogInInput)
        {
            try
            {


                IsRegisteredOutput serviceResponse = new IsRegisteredOutput();

                System.Data.Objects.ObjectParameter _objparameter = new System.Data.Objects.ObjectParameter("Status", typeof(int));
                System.Data.Objects.ObjectParameter _objClientParameter = new System.Data.Objects.ObjectParameter("ClientGuid", typeof(Guid));
                System.Data.Objects.ObjectParameter _objPathParameter = new System.Data.Objects.ObjectParameter("AppPath", typeof(string));

                var iOSApplicationFTPDetails = Context.CheckIsCustomerRegistered(p_MobileLogInInput.UID, p_MobileLogInInput.Application, _objClientParameter, p_MobileLogInInput.Version, _objparameter, _objPathParameter).FirstOrDefault();

                int status = Convert.ToInt32(_objparameter.Value);
                if (status == 0)
                {
                    if (iOSApplicationFTPDetails != null && iOSApplicationFTPDetails.FTPLoginID != null)
                    {
                        serviceResponse.Status = 0;
                        serviceResponse.IsRegistered = true;
                        serviceResponse.FTPDetails = CommonFunctions.EncryptStringAES(iOSApplicationFTPDetails.FTPLoginID + delimeter + iOSApplicationFTPDetails.FTPPwd + delimeter + iOSApplicationFTPDetails.FTPHost + delimeter + iOSApplicationFTPDetails.FTPPath, FTPEncKey, FTPEncIV);
                        serviceResponse.DefaultCategory = iOSApplicationFTPDetails.DefaultCategory;
                        serviceResponse.IsCategoryEnabled = iOSApplicationFTPDetails.IsCategoryEnabled.HasValue ? iOSApplicationFTPDetails.IsCategoryEnabled.Value : false;
                        serviceResponse.MaxVideoDuration = iOSApplicationFTPDetails.MaxVideoDuration.HasValue ? iOSApplicationFTPDetails.MaxVideoDuration.Value : 0;
                        serviceResponse.ForceLandscape = iOSApplicationFTPDetails.ForceLandscape.HasValue ? iOSApplicationFTPDetails.ForceLandscape.Value : false;
                        if (_objClientParameter != null && _objClientParameter.Value != null)
                        {
                            serviceResponse.CategoryList = Context.GetCustomCategoryByClient(new Guid(_objClientParameter.Value.ToString())).
                                                                   Select(a => new Category()
                                                                   {
                                                                       ID = a.CategoryGUID,
                                                                       Name = a.CategoryName
                                                                   }).Where(a => serviceResponse.IsCategoryEnabled == true || a.ID == iOSApplicationFTPDetails.DefaultCategory).ToList();
                        }

                        serviceResponse.Message = "Success";
                    }
                    else
                    {
                        serviceResponse.Status = 1;
                        serviceResponse.IsRegistered = false;
                        serviceResponse.Message = "Application authentication failed";
                    }
                }
                else
                {
                    serviceResponse.Status = 2;
                    serviceResponse.HasOldVersion = true;
                    serviceResponse.Path = _objPathParameter.Value.ToString();
                    serviceResponse.Message = "Old application version";
                }

                return serviceResponse;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw ex;
            }
        }

        public MobileLogInOutput MobileLogin(MobileLogInInput p_MobileLogInInput, int p_MaxPasswordAttempts)
        {
            try
            {
                MobileLogInOutput mobileLogInOutput = new MobileLogInOutput();

                var customerDetails = Context.GetCustomerDetailsForAuthentication(p_MobileLogInInput.UserID, p_MobileLogInInput.Application);

                if (customerDetails != null)
                {
                    if (string.IsNullOrEmpty(customerDetails["PasswordAttempts"]))
                    {
                        customerDetails["PasswordAttempts"] = "0";
                    }

                    var maxPasswordAttempts = p_MaxPasswordAttempts <= 0 ? 5 : p_MaxPasswordAttempts;

                    if (Convert.ToInt32(customerDetails["PasswordAttempts"]) >= maxPasswordAttempts)
                    {
                        mobileLogInOutput.Status = 3;
                        mobileLogInOutput.MobileLogin = false;
                        mobileLogInOutput.Message ="You have exceeded the maximum number of attempts to authenticate with your credentials. contact: support@iqmediacorp.com";                        

                        return mobileLogInOutput;
                    }

                    var isAuthenticated = IQMedia.Security.Authentication.VerifyPassword(p_MobileLogInInput.Password, customerDetails["Password"]);

                    if (isAuthenticated)
                    {
                        Version appVersion = new Version(customerDetails["Version"]);

                        if (appVersion != (new Version(p_MobileLogInInput.Version)))
                        {
                            mobileLogInOutput.Status = 2;
                            mobileLogInOutput.MobileLogin = false;
                            mobileLogInOutput.HasOldVersion = true;
                            mobileLogInOutput.Path = customerDetails["Path"];
                            mobileLogInOutput.Message = "Old application version";

                            return mobileLogInOutput;
                        }
                        else
                        {
                            var customerGUID = new Guid(customerDetails["CustomerGUID"]);

                            var appFTPDetails = Context.GetClientApplicationDetailsByCustomerGUIDNApplication(customerGUID, p_MobileLogInInput.Application);                            

                            UTF8Encoding encoding = new UTF8Encoding();
                            byte[] key = encoding.GetBytes(FTPEncKey);
                            byte[] iv = encoding.GetBytes(FTPEncIV);

                            mobileLogInOutput.Status = 0;
                            mobileLogInOutput.MobileLogin = true;
                            mobileLogInOutput.FTPDetails = CommonFunctions.EncryptStringAES(appFTPDetails.FTPLoginID + delimeter + appFTPDetails.FTPPwd + delimeter + appFTPDetails.FTPHost + delimeter + appFTPDetails.FTPPath, FTPEncKey, FTPEncIV);
                            mobileLogInOutput.DefaultCategory = appFTPDetails.DefaultCategory;
                            mobileLogInOutput.IsCategoryEnabled = appFTPDetails.IsCategoryEnabled.HasValue ? appFTPDetails.IsCategoryEnabled.Value : false;
                            mobileLogInOutput.MaxVideoDuration = appFTPDetails.MaxVideoDuration.HasValue ? appFTPDetails.MaxVideoDuration.Value : 0;
                            mobileLogInOutput.ForceLandscape = appFTPDetails.ForceLandscape.HasValue ? appFTPDetails.ForceLandscape.Value : false;

                            mobileLogInOutput.CategoryList = Context.GetCustomCategoryListByCustomerGUID(customerGUID).Where(cat => mobileLogInOutput.IsCategoryEnabled == true || cat.ID == mobileLogInOutput.DefaultCategory).ToList();

                            mobileLogInOutput.Message = "Success";

                            Context.UpdateUIDNPasswordAttempts(customerGUID, p_MobileLogInInput.Application, new Guid(p_MobileLogInInput.UID));

                            return mobileLogInOutput;
                        }
                    }
                    else
                    {
                        mobileLogInOutput.Status = 3;
                        mobileLogInOutput.MobileLogin = false;
                        mobileLogInOutput.Message = "UserID and/or Password is not correct.";

                        Context.UpdatePasswordAttempts(p_MobileLogInInput.UserID, false);

                        return mobileLogInOutput;
                    }
                }
                else
                {
                    mobileLogInOutput.Status = 3;
                    mobileLogInOutput.MobileLogin = false;
                    mobileLogInOutput.Message = "UserID and/or Password is not correct.";

                    return mobileLogInOutput;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw ex;
            }
        }

        public IOSExceptionLogOutput LogException(IOSExceptionLogInput p_IOSExceptionLogInput)
        {
            try
            {
                IOSExceptionLogOutput iOSExceptionLogOutput = new IOSExceptionLogOutput();
                var recordsAffected = Context.InsertIOSException(p_IOSExceptionLogInput.UID, p_IOSExceptionLogInput.Application, p_IOSExceptionLogInput.Exception).FirstOrDefault();
                if (recordsAffected > 0)
                {
                    iOSExceptionLogOutput.Status = 0;
                    iOSExceptionLogOutput.Message = "Success";
                }
                else
                {
                    iOSExceptionLogOutput.Status = 1;
                    iOSExceptionLogOutput.Message = "IOS Exception Logging Failed";
                }
                return iOSExceptionLogOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw ex;
            }
        }
    }
}
