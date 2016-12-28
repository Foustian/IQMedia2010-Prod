using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;


namespace IQMediaGroup.Controller.Implementation
{
    internal class ClientRoleController : IClientRoleController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IClientRoleModel _IClientRoleModel;

        public ClientRoleController()
        {
            _IClientRoleModel = _ModelFactory.CreateObject<IClientRoleModel>();
        }

       
        /// <summary>
        /// Description: This Methods Fills Client Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Client Role Infromarmation</param>
        /// <returns>List of Object of Client Role</returns>
        private List<ClientRoles> FillClientRoleInformation(DataSet _DataSet)
        {
            List<ClientRoles> _ListOfClientRoles = new List<ClientRoles>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ClientRoles _ClientRoles = new ClientRoles();
                        _ClientRoles.ClientRoleKey = Convert.ToInt32(_DataRow["ClientRoleKey"]);
                        _ClientRoles.RoleName = Convert.ToString(_DataRow["RoleName"]);
                        _ClientRoles.IsAccess = Convert.ToBoolean(_DataRow["IsAccess"]);

                        _ListOfClientRoles.Add(_ClientRoles);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfClientRoles;
        }


        /// <summary>
        /// Description: This Methods gets Client Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Client Role</returns>
        public List<ClientRoles> GetClientAccess(ClientRoles p_ClientRole)
        {
            DataSet _DataSet = null;
            List<ClientRoles> _ListOfClientRoles = null;
            try
            {
                _DataSet = _IClientRoleModel.GetClientAccess(p_ClientRole);
                _ListOfClientRoles = FillClientAccessInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfClientRoles;
        }

        /// <summary>
        /// Description: This Methods Fills Client Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Client Role Infromarmation</param>
        /// <returns>List of Object of Client Role</returns>
        private List<ClientRoles> FillClientAccessInformation(DataSet _DataSet)
        {
            List<ClientRoles> _ListOfClientRoles = new List<ClientRoles>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ClientRoles _ClientRoles = new ClientRoles();
                        _ClientRoles.IsAccess = Convert.ToBoolean(_DataRow["IsAccess"]);
                        _ListOfClientRoles.Add(_ClientRoles);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfClientRoles;
        }

        
        /// <summary>
        /// This method inserts Client Role Information
        /// </summary>
        /// <param name="p_ClientRoles">Object Of ClientRoles Class</param>
        /// <returns></returns>
        public string InsertClientRole(ClientRoles p_ClientRoles)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClientRoleModel.InsertClientRole(p_ClientRoles);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets Client Role Details
        /// </summary>
        /// <returns>List of object of client role details</returns>
        public List<ClientRoles> GetClientRoleAdmin()
        {
            DataSet _DataSet = null;
            List<ClientRoles> _ListOfClientRoles = null;
            try
            {
                _DataSet = _IClientRoleModel.GetClientRoleAdmin();
                _ListOfClientRoles = FillClientRoleInfo(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfClientRoles;
        }

        /// <summary>
        /// Description: This Methods Fills Client Role Information from DataSet.
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="_DataSet">Dataset for Client Role Infromarmation</param>
        /// <returns>List of Object of Client Role</returns>
        private List<ClientRoles> FillClientRoleInfo(DataSet _DataSet)
        {
            List<ClientRoles> _ListOfClientRoles = new List<ClientRoles>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ClientRoles _ClientRoles = new ClientRoles();
                        _ClientRoles.ClientRoleKey = Convert.ToInt32(_DataRow["ClientRoleKey"]);
                        _ClientRoles.RoleName = Convert.ToString(_DataRow["RoleName"]);
                        _ClientRoles.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _ClientRoles.RoleID = Convert.ToInt32(_DataRow["RoleID"]);
                        _ClientRoles.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        _ClientRoles.IsAccess = Convert.ToBoolean(_DataRow["IsAccess"]);

                        _ListOfClientRoles.Add(_ClientRoles);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfClientRoles;
        }

        /// <summary>
        /// Description:This method update Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQAgentSearchRequest">object of SearchRequest</param>
        /// <returns>string</returns>
        public string UpdateClientRole(ClientRoles p_ClientRole)
        {
            try
            {
               string _Result =  _IClientRoleModel.UpdateClientRole(p_ClientRole);
               return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public int GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName)
        {
            DataSet _DataSet = null;
            int IsAccess = 0;
            try
            {
                _DataSet = _IClientRoleModel.GetClientRoleByClientGUIDRoleName(ClientGUID,RoleName);
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    IsAccess = Convert.ToInt32(_DataSet.Tables[0].Rows[0]["IsAccess"]);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return IsAccess;

        }
    }
}
