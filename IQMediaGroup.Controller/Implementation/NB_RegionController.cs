using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Factory;

namespace IQMediaGroup.Controller.Implementation
{
    internal class NB_RegionController : INB_RegionController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly INB_RegionModel _INB_RegionModel;

        public NB_RegionController()
        {
            _INB_RegionModel = _ModelFactory.CreateObject<INB_RegionModel>();
        }

        public List<NB_Region> GetAllRegion()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _INB_RegionModel.GetAllRegion();

                List<NB_Region> _ListOfNB_Region = FillNB_RegionInfo(_DataSet);

                return _ListOfNB_Region;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<NB_Region> FillNB_RegionInfo(DataSet _DataSet)
        {
            List<NB_Region> _ListOfNB_Region = new List<NB_Region>();
            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {

                        NB_Region _NB_Region = new NB_Region();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && _DataRow["ID"] != null)
                            _NB_Region.ID = Convert.ToInt32(_DataRow["ID"]);

                        if (_DataSet.Tables[0].Columns.Contains("Name") && _DataRow["Name"] != null)
                            _NB_Region.Name = Convert.ToString(_DataRow["Name"]);

                        if (_DataSet.Tables[0].Columns.Contains("Label") && _DataRow["Label"] != null)
                            _NB_Region.Label = Convert.ToString(_DataRow["Label"]);

                        if (_DataSet.Tables[0].Columns.Contains("IsActive") && _DataRow["IsActive"] != null)
                            _NB_Region.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfNB_Region.Add(_NB_Region);
                    }
                }
                return _ListOfNB_Region;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
