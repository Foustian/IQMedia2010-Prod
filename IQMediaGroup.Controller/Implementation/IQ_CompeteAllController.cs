using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQ_CompeteAllController : IIQ_CompeteAllController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQ_CompeteAllModel _IIQ_CompeteAllModel;

        public IQ_CompeteAllController()
        {
            _IIQ_CompeteAllModel = _ModelFactory.CreateObject<IIQ_CompeteAllModel>();
        }

        public List<IQ_CompeteAll> GetArtileAdShareValueByClientGuidAndXml(Guid p_ClientGuid, string p_WebSiteURLXml,string p_MediaType)
        {
            try
            {
                List<IQ_CompeteAll> _ListOfIQ_CompeteAll = null;

                DataSet _DataSet = _IIQ_CompeteAllModel.GetArtileAdShareValueByClientGuidAndXml(p_ClientGuid,p_WebSiteURLXml,p_MediaType);

                _ListOfIQ_CompeteAll = FillListOfIQ_Compete(_DataSet);

                return _ListOfIQ_CompeteAll;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQ_CompeteAll> FillListOfIQ_Compete(DataSet p_DataSet)
        {
            try
            {
                List<IQ_CompeteAll> _ListOfIQ_CompeteAll = new List<IQ_CompeteAll>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        IQ_CompeteAll _IQ_CompeteAll = new IQ_CompeteAll();

                        if (p_DataSet.Tables[0].Columns.Contains("CompeteURL") && !_DataRow["CompeteURL"].Equals(DBNull.Value))
                        {
                            _IQ_CompeteAll.CompeteURL = Convert.ToString(_DataRow["CompeteURL"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IQ_AdShare_Value") && !_DataRow["IQ_AdShare_Value"].Equals(DBNull.Value))
                        {
                            _IQ_CompeteAll.IQ_AdShare_Value = Convert.ToDecimal(_DataRow["IQ_AdShare_Value"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("c_uniq_visitor") && !_DataRow["c_uniq_visitor"].Equals(DBNull.Value))
                        {
                            _IQ_CompeteAll.c_uniq_visitor = Convert.ToInt32(_DataRow["c_uniq_visitor"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IsCompeteAll") && !_DataRow["IsCompeteAll"].Equals(DBNull.Value))
                        {
                            _IQ_CompeteAll.IsCompeteAll = Convert.ToBoolean(_DataRow["IsCompeteAll"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IsUrlFound") && !_DataRow["IsUrlFound"].Equals(DBNull.Value))
                        {
                            _IQ_CompeteAll.IsUrlFound = Convert.ToBoolean(_DataRow["IsUrlFound"]);
                        }                        

                        _ListOfIQ_CompeteAll.Add(_IQ_CompeteAll);
                        
                    }
                }

                return _ListOfIQ_CompeteAll;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
