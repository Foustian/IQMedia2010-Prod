using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IRoleController
    {
        bool GetClientRoleByClientGUIDRoleName(Guid p_ClientGUID, string p_RoleName);
    }
}
