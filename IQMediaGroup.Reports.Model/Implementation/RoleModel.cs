using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Reports.Model.Interface;

namespace IQMediaGroup.Reports.Model.Implementation
{
    internal class RoleModel : IQMediaGroupDataLayer,IRoleModel
    {
        public bool GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName)
        {
            try
            {
                
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleName", DbType.String, RoleName, ParameterDirection.Input));
                bool _HasAccess = Convert.ToBoolean(this.ExecuteScalar("usp_ClientRole_SelectRoleByClientGUIDRoleName", _ListOfDataType));
                return _HasAccess;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
