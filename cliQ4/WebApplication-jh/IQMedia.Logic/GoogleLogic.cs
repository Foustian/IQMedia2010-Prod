using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Logic.Base;
using IQMedia.Data;

namespace IQMedia.Web.Logic
{
    public class GoogleLogic : ILogic
    {
        public void UpdateAuthCode(Guid clientGuid, string authCode)
        {
            GoogleDA googleDA = (GoogleDA)DataAccessFactory.GetDataAccess(DataAccessType.Google);
            googleDA.UpdateAuthCode(clientGuid, authCode);
        }

        public bool CheckClientAccess(Guid clientGuid)
        {
            GoogleDA googleDA = (GoogleDA)DataAccessFactory.GetDataAccess(DataAccessType.Google);
            return googleDA.CheckClientAccess(clientGuid);
        }

        public string GetClientID()
        {
            GoogleDA googleDA = (GoogleDA)DataAccessFactory.GetDataAccess(DataAccessType.Google);
            return googleDA.GetClientID();
        }
    }
}
