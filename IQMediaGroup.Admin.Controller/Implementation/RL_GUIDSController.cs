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

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRL_GUIDS
    /// </summary>
    internal class RL_GUIDSController : IRL_GUIDSController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IRL_GUIDSModel _IRL_GUIDSModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public RL_GUIDSController()
        {
            _IRL_GUIDSModel = _ModelFactory.CreateObject<IRL_GUIDSModel>();
        }

        /// <summary> 
        /// Add RL_GUIDS 
        /// </summary> 
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param> 
        /// <returns>RL_GUIDSKey of added record</returns> 

        public string AddRL_GUIDS(RL_GUIDS p_RL_GUIDS,DateTime p_RequestDateTime,int p_RequestTime)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_GUIDSModel.AddRL_GUIDS(p_RL_GUIDS,p_RequestDateTime,p_RequestTime);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        /// <summary> 
        /// This method gets all RL_GUIDS details 
        /// </summary> 
        /// <returns> List Of RL_GUIDS </returns> 

        public List<RL_GUIDS> GetAllRL_GUIDS()
        {
            try
            {
                DataSet _DataSet = null;

                List<RL_GUIDS> _ListOfRL_GUIDS = new List<RL_GUIDS>();

                _DataSet = _IRL_GUIDSModel.GetAllRL_GUIDS();
                _ListOfRL_GUIDS = FillRL_GUIDS(_DataSet);

                return _ListOfRL_GUIDS;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary> 
        /// This method will feill RL_GUIDS summary 
        /// </summary> 
        /// <returns> List Of RL_GUIDS </returns> 

        public List<RL_GUIDS> FillRL_GUIDS(DataSet p_DataSet)
        {
            List<RL_GUIDS> _ListofRL_GUIDS = new List<RL_GUIDS>();

            if (p_DataSet != null && p_DataSet.Tables.Count > 0)
            {
                DataTable _DataTable = p_DataSet.Tables[0];

                foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                {
                    RL_GUIDS _RL_GUIDS = new RL_GUIDS();

                    if (_DataTable.Columns.Contains("RL_GUIDSKey"))
                    {
                        _RL_GUIDS.RL_GUIDSKey = Convert.ToInt64(_DataRow["RL_GUIDSKey"]);    
                    }

                    if (_DataTable.Columns.Contains("RL_Station_ID"))
                    {
                        _RL_GUIDS.RL_Station_ID = Convert.ToString(_DataRow["RL_Station_ID"]); 
                    }

                    if (_DataTable.Columns.Contains("RL_Station_Date"))
                    {
                        _RL_GUIDS.RL_Station_Date = Convert.ToDateTime(_DataRow["RL_Station_Date"]); 
                    }

                    if (_DataTable.Columns.Contains("RL_Station_Time"))
                    {
                        _RL_GUIDS.RL_Station_Time = Convert.ToInt32(_DataRow["RL_Station_Time"]); 
                    }

                    if (_DataTable.Columns.Contains("RL_Time_zone"))
                    {
                        _RL_GUIDS.RL_Time_zone = Convert.ToString(_DataRow["RL_Time_zone"]); 
                    }

                    if (_DataTable.Columns.Contains("RL_GUID"))
                    {
                        _RL_GUIDS.RL_GUID = Convert.ToString(_DataRow["RL_GUID"]); 
                    }

                    if (_DataTable.Columns.Contains("GMT_Date"))
                    {
                        _RL_GUIDS.GMT_Date = Convert.ToDateTime(_DataRow["GMT_Date"]); 
                    }

                    if (_DataTable.Columns.Contains("GMT_Time"))
                    {
                        _RL_GUIDS.GMT_Time = Convert.ToInt32(_DataRow["GMT_Time"]); 
                    }

                    if (_DataTable.Columns.Contains("IQ_CC_Key"))
                    {
                        _RL_GUIDS.IQ_CC_Key = Convert.ToString(_DataRow["IQ_CC_Key"]); 
                    }

                    if (_DataTable.Columns.Contains("GUID_Status"))
                    {
                        _RL_GUIDS.GUID_Status = Convert.ToBoolean(_DataRow["GUID_Status"]); 
                    }

                    _ListofRL_GUIDS.Add(_RL_GUIDS);
                }
            }

            return _ListofRL_GUIDS;
        }


        /// <summary> 
        /// This method updates RL_GUIDS details 
        /// </summary> 
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param> 
        /// <returns>1/0-Updated or not</returns> 

        public string UpdateRL_GUIDS(RL_GUIDS p_RL_GUIDS)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_GUIDSModel.UpdateRL_GUIDS(p_RL_GUIDS);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        /// <summary> 
        /// This method deletes RL_GUIDS 
        /// </summary> 
        /// <param name="p_RL_GUIDSKey"> RL_GUIDSKey</param> 
        /// <returns>1/0-Deleted or not</returns> 

        public string DeleteRL_GUIDS(string p_RL_GUIDSKey)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_GUIDSModel.DeleteRL_GUIDS(p_RL_GUIDSKey);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets RL_GUIDS Records by IQ_CC_Key
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        public List<RL_GUIDS> GetRL_GUIDSByIQCCKey(string p_IQ_CC_Key)
        {
            try
            {
                List<RL_GUIDS> _ListOfRL_GUIDS = null;
                DataSet _DataSet = null;

                _DataSet = _IRL_GUIDSModel.GetRL_GUIDSByIQCCKey(p_IQ_CC_Key);

                _ListOfRL_GUIDS = FillRL_GUIDS(_DataSet);

                return _ListOfRL_GUIDS;
               
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}