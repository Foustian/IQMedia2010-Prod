using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface IRoleModel
    {
        bool GetClientRoleByClientGUIDRoleName(Guid p_ClientGUID, string p_RoleName);
    }
}
