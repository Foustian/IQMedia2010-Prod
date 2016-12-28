using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;
using System.Data;
using System.IO;
using System.Web;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Model.Interface;


namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class StatSkedProgController : IStatSkedProgController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IStatSkedProgModel _IStatSkedProgModel;

        public StatSkedProgController()
        {
            _IStatSkedProgModel = _ModelFactory.CreateObject<IStatSkedProgModel>();            
        }       
       
        /// <summary>
        /// Description: This Methods gets Program Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_Program">Dataset for Program</param>
        /// <returns>List of object of Program</returns>
        public MasterStatSkedProg GetAllDetail()
        {
            DataSet _DataSet = null;
          
           
            MasterStatSkedProg _MasterStatSkedProg = new MasterStatSkedProg();
            try
            {
                _DataSet = _IStatSkedProgModel.GetAllDetail(); 
                _MasterStatSkedProg._ListofMarket = FillMarketInformation(_DataSet);
                _MasterStatSkedProg._ListofType = FillProgramTypeInformation(_DataSet);
                _MasterStatSkedProg._ListofAffil = FillAffilInformation(_DataSet);
                
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _MasterStatSkedProg;
        }

        /// <summary>
        /// Description: This Methods Fills Program Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="_DataSet">Dataset for Program Infromarmation</param>
        /// <returns>List of object of Program Information</returns>
        private List<SSP_IQ_Dma_Name> FillMarketInformation(DataSet _DataSet)
        {
            List<SSP_IQ_Dma_Name> _ListOfMarketInformation = new List<SSP_IQ_Dma_Name>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        SSP_IQ_Dma_Name _SSP_IQ_Dma_Name = new SSP_IQ_Dma_Name();
                        _SSP_IQ_Dma_Name.IQ_Dma_Name = Convert.ToString(_DataRow["Dma_Name"]);
                        _SSP_IQ_Dma_Name.IQ_Dma_Num = Convert.ToString(_DataRow["Dma_Num"]);
                        _ListOfMarketInformation.Add(_SSP_IQ_Dma_Name);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfMarketInformation;
        }


        /// <summary>
        /// Description:This Methods Fills Program Type Information from DataSet.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Program Type Infromarmation</param>
        /// <returns>List of object of Program Type Information</returns>
        private List<SSP_IQ_Class> FillProgramTypeInformation(DataSet _DataSet)
        {
            List<SSP_IQ_Class> _ListOfProgramTypeInformation = new List<SSP_IQ_Class>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[1].Rows)
                    {
                        SSP_IQ_Class _SSP_IQ_Class = new SSP_IQ_Class();

                        _SSP_IQ_Class.IQ_Class = Convert.ToString(_DataRow["IQ_Class"]);
                        _SSP_IQ_Class.IQ_Class_Num = Convert.ToString(_DataRow["IQ_Class_Num"]);

                        _ListOfProgramTypeInformation.Add(_SSP_IQ_Class);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfProgramTypeInformation;
        }

        /// <summary>
        /// Description:This Methods Fills Affil Information from DataSet.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Affil Infromarmation</param>
        /// <returns>List of object of Affil Information</returns>
        private List<SSP_Station_Affil> FillAffilInformation(DataSet _DataSet)
        {
            List<SSP_Station_Affil> _ListOfAffilTypeInformation = new List<SSP_Station_Affil>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[2].Rows)
                    {
                        SSP_Station_Affil _SSP_Station_Affil = new SSP_Station_Affil();

                        _SSP_Station_Affil.Station_Affil = Convert.ToString(_DataRow["Station_Affil"]);
                        _SSP_Station_Affil.Station_Affil_Num = Convert.ToString(_DataRow["Station_Affil_Num"]);

                        _ListOfAffilTypeInformation.Add(_SSP_Station_Affil);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfAffilTypeInformation;
        }

       

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <param name="p_TotalRowCount">TotalRowCount</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailsByString(StatSkedProg _StatSkedProg,out int p_TotalRowCount)
        {
            DataSet _DataSet = null;           

            List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();
            try
            {
                _DataSet = _IStatSkedProgModel.GetDetailsByString(_StatSkedProg);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                p_TotalRowCount = 0;

                if (_DataSet!=null && _DataSet.Tables.Count>1 && _DataSet.Tables[1]!=null && _DataSet.Tables[1].Rows.Count>0)
                {
                    if (_DataSet.Tables[1].Columns.Contains("TotalRowCount"))
                    {
                        if (_DataSet.Tables[1].Rows[0]["TotalRowCount"] != null)
                        {
                            p_TotalRowCount = Convert.ToInt32(_DataSet.Tables[1].Rows[0]["TotalRowCount"].ToString());
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStatSkedProg;
        }

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String With Time.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailsByStringWithTime(StatSkedProg _StatSkedProg)
        {
            DataSet _DataSet = null;

            List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();
            try
            {
                _DataSet = _IStatSkedProgModel.GetDetailsByString(_StatSkedProg);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStatSkedProg;
        }

        /// <summary>
        /// Description:This Methods Fill StatSkedProg details.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for StatSkedProg Infromarmation</param>
        /// <returns>List of object of StatSkedProg</returns>
        private List<StatSkedProg> FillAllInformation(DataSet _DataSet)
        {
            List<StatSkedProg> _ListOfInformation = new List<StatSkedProg>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        StatSkedProg _StatSkedProg = new StatSkedProg();

                        if (_DataSet.Tables[0].Columns.Contains("Station_ID"))
                        {
                            _StatSkedProg.Station_ID = Convert.ToString(_DataRow["Station_ID"]); 
                        }                        

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Local_Air_Date"))
                        {
                            _StatSkedProg.IQ_Local_Air_Date = Convert.ToDateTime(_DataRow["IQ_Local_Air_Date"]); 
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Local_Air_Time"))
                        {
                            _StatSkedProg.IQ_Local_Air_Time = _DataRow["IQ_Local_Air_Time"].ToString(); 
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Dma_Num"))
                        {
                            _StatSkedProg.IQ_Dma_Num = Convert.ToString(_DataRow["IQ_Dma_Num"]); 
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Dma_Name"))
                        {
                            _StatSkedProg.IQ_Dma_Name = Convert.ToString(_DataRow["IQ_Dma_Name"]); 
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Title120"))
                        {
                            _StatSkedProg.Title120 = Convert.ToString(_DataRow["Title120"]); 
                        }

                        if (_DataSet.Tables[0].Columns.Contains("RL_GUID"))
                        {
                            _StatSkedProg.RL_GUID = _DataRow["RL_GUID"].ToString();
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_CC_Key"))
                        {
                            _StatSkedProg.IQ_CC_Key = _DataRow["IQ_CC_Key"].ToString();
                        }

                        _ListOfInformation.Add(_StatSkedProg);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfInformation;


        }

        /// <summary>
        /// Description:This Methods get StatSkedProg details by GUIDs.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_GUIDs">p_GUID</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailByGUIDs(string p_GUIDs)
        {
            try
            {
                DataSet _DataSet = null;

                List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();

                _DataSet = _IStatSkedProgModel.GetDetailByGUIDs(p_GUIDs);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                return _ListOfStatSkedProg;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }


        public MasterStatSkedProg GetAllDetailByClientSettings(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed)
        {
            try
            {
                DataSet _DataSet = null;
                MasterStatSkedProg _MasterStatSkedProg = new MasterStatSkedProg();

                _DataSet = _IStatSkedProgModel.GetAllDetailByClientSettings(p_ClientGUID, out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);
                _MasterStatSkedProg._ListofMarket = FillMarketInformation(_DataSet);
                _MasterStatSkedProg._ListofType = FillProgramTypeInformation(_DataSet);
                _MasterStatSkedProg._ListofAffil = FillAffilInformation(_DataSet);

                return _MasterStatSkedProg;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

    
    }
}
