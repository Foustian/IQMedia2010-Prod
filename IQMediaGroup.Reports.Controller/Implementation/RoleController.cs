using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using System.Data;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class RoleController : IRoleController
    {
         ModelFactory _ModelFactory = new ModelFactory();
        IRoleModel _IRoleModel;

        public RoleController()
        {
            _IRoleModel = _ModelFactory.CreateObject<IRoleModel>();
        }

        public bool GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName)
        {
            bool _HasAccess;
            
            try
            {
                _HasAccess = _IRoleModel.GetClientRoleByClientGUIDRoleName(ClientGUID, RoleName);              
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _HasAccess;

        }
    }
}
