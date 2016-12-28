using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public class SessionDA : IDataAccess
    {
        public string InsertSession(SessionModel p_SessionModel)
        {

            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@SessionID", DbType.String, p_SessionModel.SessionID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_SessionModel.LoginID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SessionTimeout", DbType.DateTime, p_SessionModel.SessionTimeOut, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@LastAccessTime", DbType.DateTime, p_SessionModel.LastAccessTime, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Server", DbType.String, p_SessionModel.Server, ParameterDirection.Input));


            string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQSession_Insert", dataTypeList);
            return _Result;

        }

        public List<SessionModel> GetAll(string p_SearchTerm,string p_SortColumn,bool p_IsAsc)
        {
            List<DataType> dataTypeList = new List<DataType>();

            DataSet ds = DataAccess.GetDataSet("usp_v4_IQSession_SelectAll", dataTypeList);

            return FillSessionList(ds);            
        }

        private List<SessionModel> FillSessionList(DataSet ds)
        {
            List<SessionModel> sessionList = new List<SessionModel>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SessionModel session = new SessionModel();

                session.SessionID = Convert.ToString(row["SessionID"]);
                session.LoginID = Convert.ToString(row["LoginID"]);
                session.LastAccessTime = Convert.ToDateTime(row["LastAccessTime"]);
                session.SessionTimeOut = Convert.ToDateTime(row["SessionTimeOut"]);
                session.Server = Convert.ToString(row["Server"]);

                sessionList.Add(session);
            }

            return sessionList;
        }

        public string DeleteBySessionID(string p_SessionID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@SessionID",DbType.String,p_SessionID,ParameterDirection.Input));

            string result = DataAccess.ExecuteNonQuery("usp_v4_IQSession_DeleteBySessionID",dataTypeList);

            return result;
        }

        public string DeleteByLoginID(string p_LoginID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            string result = DataAccess.ExecuteNonQuery("usp_v4_IQSession_DeleteByLoginID", dataTypeList);

            return result;
        }        
    }
}
