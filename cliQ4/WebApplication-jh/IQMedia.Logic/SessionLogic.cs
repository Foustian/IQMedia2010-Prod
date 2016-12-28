using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class SessionLogic
    {
        public string InsertSession(SessionModel p_SessionModel)
        {
           return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).InsertSession(p_SessionModel);
        }

        public List<SessionModel> GetAll(string p_SearchTerm, string p_SortColumn, bool p_IsAsc)
        {
            return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).GetAll(p_SearchTerm, p_SortColumn, p_IsAsc);
        }

        public string DeleteBySessionID(string p_SessionID)
        {
            return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).DeleteBySessionID(p_SessionID);
        }

        public string DeleteByLoginID(string p_LoginID)
        {
            return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).DeleteByLoginID(p_LoginID);
        }        
    }
}
