using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
namespace IQMediaGroup.Controller.Implementation
{
    internal class RoleController:IRoleController
    {
         private readonly ModelFactory _ModelFactory = new ModelFactory();
            private readonly IRoleModel _IRoleModel;

            public RoleController()
            {
                _IRoleModel = _ModelFactory.CreateObject<IRoleModel>();
            }

        /// <summary>
        /// This method gets Role Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the role</param>
        /// <returns>List of object of Role Class</returns>
        public List<Role> GetRoleInformation(bool? p_IsActive)
        {
            try
            {
                List<Role> _ListOfRole = null;

                DataSet _DataSet = _IRoleModel.GetRoleInfo(p_IsActive);

                _ListOfRole = FillListOfRole(_DataSet);

                return _ListOfRole;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object Role from DataSet
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_DataSet">DataSet contains RoleInformation</param>
        /// <returns>List of Object of class Roles</returns>
        private List<Role> FillListOfRole(DataSet p_DataSet)
        {
            try
            {
                List<Role> _ListOfRole = new List<Role>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Role _Role = new Role();

                        _Role.RoleID = Convert.ToInt32(_DataRow["RoleKey"]);
                        _Role.RoleName = _DataRow["RoleName"].ToString();
                        _Role.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfRole.Add(_Role);
                    }
                }

                return _ListOfRole;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts Role Information
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_Role">Object of Role details</param>
        /// <returns>Role Key</returns>
        public string InsertRole(Role p_Role)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IRoleModel.InsertRole(p_Role);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates Role Information
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_Role">Object of Role details class</param>
        /// <returns>Role Key</returns>
        public string UpdateRole(Role p_Role)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IRoleModel.UpdateRole(p_Role);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public List<Role> GetRoleName(Int32 p_CustomerID)
        {
            try
            {
                List<Role> _ListOfRole = null;

                DataSet _DataSet = _IRoleModel.GetRoleName(p_CustomerID);

                _ListOfRole = FillRoleName(_DataSet);

                return _ListOfRole;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Role> FillRoleName(DataSet p_DataSet)
        {
            try
            {
                List<Role> _ListOfRole = new List<Role>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Role _Role = new Role();
                       
                        _Role.RoleName = _DataRow["RoleName"].ToString();                        

                        _ListOfRole.Add(_Role);
                    }
                }

                return _ListOfRole;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
