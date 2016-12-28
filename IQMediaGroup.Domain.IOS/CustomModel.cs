using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.Extensions;

namespace IQMediaGroup.Domain.IOS
{
    public partial class IQMediaGroupIOSEntities : ObjectContext
    {
        public void UpdatePasswordAttempts(string p_LoginID, bool p_ResetPasswordAttempts)
        {
            DbCommand command = this.CreateStoreCommand("usp_iossvc_fliQ_Customer_UpdatedPasswordAttempts", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("LoginID", p_LoginID));
            command.Parameters.Add(new SqlParameter("ResetPasswordAttempts", p_ResetPasswordAttempts));

            using (command.Connection.CreateConnectionScope())
            {
                command.ExecuteNonQuery();
            }
        }

        public Dictionary<string, string> GetCustomerDetailsForAuthentication(string p_LoginID, string p_Application)
        {
            Dictionary<string, string> result = null;

            DbCommand command = this.CreateStoreCommand("usp_iossvc_fliQ_Customer_SelectForAuthentication", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("LoginID", p_LoginID));
            command.Parameters.Add(new SqlParameter("Application", p_Application));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Dictionary<string, string>();

                    result.Add("Password", Convert.ToString(reader["CustomerPassword"]));
                    result.Add("PasswordAttempts", Convert.ToString(reader["PasswordAttempts"]));
                    result.Add("Version", Convert.ToString(reader["Version"]));
                    result.Add("Path", Convert.ToString(reader["Path"]));
                    result.Add("CustomerGUID", Convert.ToString(reader["CustomerGUID"]));
                }
            }

            return result;
        }

        public IOSApplicationFTPDetails GetClientApplicationDetailsByCustomerGUIDNApplication(Guid p_CustomerGUID, string p_Application)
        {
            IOSApplicationFTPDetails appFTPDetails = null;

            DbCommand command = this.CreateStoreCommand("usp_iossvc_fliQ_ClientApplication_SelectByCustomerGUIDNApplication", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("CustomerGUID", p_CustomerGUID));
            command.Parameters.Add(new SqlParameter("Application", p_Application));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    appFTPDetails = new IOSApplicationFTPDetails();

                    appFTPDetails.DefaultCategory = reader["DefaultCategory"] == DBNull.Value ? (Guid?)null : new Guid(Convert.ToString(reader["DefaultCategory"]));
                    appFTPDetails.ForceLandscape = reader["ForceLandscape"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["ForceLandscape"]);
                    appFTPDetails.FTPHost = Convert.ToString(reader["FTPHost"]);
                    appFTPDetails.FTPPath = Convert.ToString(reader["FTPPath"]);
                    appFTPDetails.FTPLoginID = Convert.ToString(reader["FTPLoginID"]);
                    appFTPDetails.FTPPwd = Convert.ToString(reader["FTPPwd"]);
                    appFTPDetails.IsCategoryEnabled = reader["IsCategoryEnable"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsCategoryEnable"]);
                    appFTPDetails.MaxVideoDuration = reader["MaxVideoDuration"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["MaxVideoDuration"]);                    
                }
            }

            return appFTPDetails;
        }

        public List<Category> GetCustomCategoryListByCustomerGUID(Guid p_CustomerGUID)
        {
            List<Category> categoryList = new List<Category>();

            DbCommand command = this.CreateStoreCommand("usp_iossvc_CustomCategory_SelectByCustomerGUID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("CustomerGUID", p_CustomerGUID));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var category = new Category();

                    category.ID = new Guid(Convert.ToString(reader["CategoryGUID"]));
                    category.Name = Convert.ToString(reader["CategoryName"]);

                    categoryList.Add(category);
                }
            }

            return categoryList;
        }

        public void UpdateUIDNPasswordAttempts(Guid p_CustomerGUID, string p_Application, Guid p_UID)
        {
            DbCommand command = this.CreateStoreCommand("usp_iossvc_fliQ_Customer_UpdateUIDNPasswordAttempts", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("CustomerGUID", p_CustomerGUID));
            command.Parameters.Add(new SqlParameter("Application", p_Application));
            command.Parameters.Add(new SqlParameter("UID", p_UID));

            using (command.Connection.CreateConnectionScope())
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
