using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRL_Station_exception
    /// </summary>
    internal class RL_Station_exceptionController : IRL_Station_exceptionController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IRL_Station_exceptionModel _IRL_Station_exceptionModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public RL_Station_exceptionController()
        {
            _IRL_Station_exceptionModel = _ModelFactory.CreateObject<IRL_Station_exceptionModel>();
        }

        /// <summary> 
        /// Add RL_Station_exception 
        /// </summary> 
        /// <param name="p_RL_Station_exception">Object of RL_Station_exception</param> 
        /// <returns>RL_Station_exceptionKey of added record</returns> 

        public string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception, DateTime p_RequestedDate, int p_RequestedTime)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_Station_exceptionModel.AddRL_Station_exception(p_RL_Station_exception, p_RequestedDate, p_RequestedTime);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception,bool p_IsMisMatch)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_Station_exceptionModel.AddRL_Station_exception(p_RL_Station_exception,p_IsMisMatch);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public List<RL_Station_exception> GetNoPassCountRL_Station_exception(DateTime p_StartDate, DateTime p_EndDate, IQ_Process p_IQ_Process)
        {
            try
            {
                DataSet _DataSet = null;

                List<RL_Station_exception> _ListOfRL_Station_exception = new List<RL_Station_exception>();

                _DataSet = _IRL_Station_exceptionModel.GetNoPassCountRL_Station_exception(p_StartDate, p_EndDate, p_IQ_Process);
                _ListOfRL_Station_exception = FillNoPassCountRL_Station_exception(_DataSet);

                return _ListOfRL_Station_exception;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<RL_Station_exception> FillNoPassCountRL_Station_exception(DataSet p_DataSet)
        {
            try
            {
                List<RL_Station_exception> _ListofRL_Station_exception = new List<RL_Station_exception>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        RL_Station_exception _RL_Station_exception = new RL_Station_exception();

                        if (!_DataRow["RL_Station_ID"].Equals(DBNull.Value))
                            _RL_Station_exception.RL_Station_ID = Convert.ToString(_DataRow["RL_Station_ID"]);

                        if (!_DataRow["RL_Station_Date"].Equals(DBNull.Value))
                            _RL_Station_exception.RL_Station_Date = Convert.ToDateTime(_DataRow["RL_Station_Date"]);

                        if (!_DataRow["RL_Station_Time"].Equals(DBNull.Value))
                            _RL_Station_exception.RL_Station_Time = Convert.ToInt32(_DataRow["RL_Station_Time"]);

                        if (!_DataRow["Time_zone"].Equals(DBNull.Value))
                            _RL_Station_exception.Time_zone = Convert.ToString(_DataRow["Time_zone"]);

                        if (!_DataRow["GMT_Adj"].Equals(DBNull.Value))
                            _RL_Station_exception.GMT_Adj = Convert.ToString(_DataRow["GMT_Adj"]);

                        if (!_DataRow["DST_Adj"].Equals(DBNull.Value))
                            _RL_Station_exception.DST_Adj = Convert.ToString(_DataRow["DST_Adj"]);

                        if (!_DataRow["RQ_Converted_Date"].Equals(DBNull.Value))
                            _RL_Station_exception.RQ_Converted_Date = Convert.ToDateTime(_DataRow["RQ_Converted_Date"].ToString());

                        if (!_DataRow["RQ_Converted_Time"].Equals(DBNull.Value))
                            _RL_Station_exception.RQ_Converted_Time = Convert.ToInt32(_DataRow["RQ_Converted_Time"].ToString());

                        _ListofRL_Station_exception.Add(_RL_Station_exception);
                    }
                }

                return _ListofRL_Station_exception;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary> 
        /// This method will feill RL_Station_exception summary 
        /// </summary> 
        /// <returns> List Of RL_Station_exception </returns> 

        public List<RL_Station_exception> FillRL_Station_exception(DataSet p_DataSet)
        {
            List<RL_Station_exception> _ListofRL_Station_exception = new List<RL_Station_exception>();

            if (p_DataSet != null && p_DataSet.Tables.Count > 0)
            {
                foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                {


                    RL_Station_exception _RL_Station_exception = new RL_Station_exception();



                    _RL_Station_exception.RL_Station_exceptionKey = Convert.ToInt64(_DataRow["RL_Station_exceptionKey"]);
                    _RL_Station_exception.RL_Station_ID = Convert.ToString(_DataRow["RL_Station_ID"]);
                    _RL_Station_exception.RL_Station_Date = Convert.ToDateTime(_DataRow["RL_Station_Date"]);
                    _RL_Station_exception.RL_Station_Time = Convert.ToInt32(_DataRow["RL_Station_Time"]);
                    _RL_Station_exception.Time_zone = Convert.ToString(_DataRow["Time_zone"]);
                    _RL_Station_exception.GMT_Adj = Convert.ToString(_DataRow["GMT_Adj"]);
                    _RL_Station_exception.DST_Adj = Convert.ToString(_DataRow["DST_Adj"]);
                    _RL_Station_exception.IQ_Process = Convert.ToString(_DataRow["IQ_Process"]);
                    _RL_Station_exception.Pass_count = Convert.ToString(_DataRow["Pass_count"]);


                    _ListofRL_Station_exception.Add(_RL_Station_exception);
                }
            }

            return _ListofRL_Station_exception;
        }

        /// <summary> 
        /// This method deletes RL_Station_exception 
        /// </summary> 
        /// <param name="p_RL_Station_exceptionKey"> RL_Station_exceptionKey</param> 
        /// <returns>1/0-Deleted or not</returns> 
        public string DeleteRL_Station_exception(string p_RL_Station_exceptionKey)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_Station_exceptionModel.DeleteRL_Station_exception(p_RL_Station_exceptionKey);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}