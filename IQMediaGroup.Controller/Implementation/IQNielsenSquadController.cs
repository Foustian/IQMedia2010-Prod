using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Xml.Linq;

namespace IQMediaGroup.Controller.Implementation
{
    public class IQNielsenSquadController : IIQNielsenSquadController
    {

        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IIQNielsenSquadModel _IIQNielsenSquadModel;

        public IQNielsenSquadController()
        {
            _IIQNielsenSquadModel = _ModelFactory.CreateObject<IIQNielsenSquadModel>();
        }

        public List<RawMedia> GetNielsenData(string iqCCKey, List<RawMedia> _listRawMedia)
        {
            try
            {
                DataSet _Result;

                _Result = _IIQNielsenSquadModel.GetNielsenData(iqCCKey);
                if (_Result != null && _Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        RawMedia rawMedia = _listRawMedia.Find(p => p.IQ_CC_Key.Equals(dr["IQ_CC_KEY"]));
                        if (rawMedia != null)
                        {
                            rawMedia.IQNielenseAudience = Convert.ToString(dr["AUDIENCE"]);
                            rawMedia.IQAddShareValue = Convert.ToString(dr["SQAD_SHAREVALUE"]);
                        }
                    }
                }
                return _listRawMedia;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<RawMedia> GetNielsenDataByXML(XDocument xmldata, List<RawMedia> _listRawMedia, Guid clientGuid)
        {
            try
            {
                DataSet _Result;

                _Result = _IIQNielsenSquadModel.GetNielsenDataByXML(xmldata, clientGuid);
                if (_Result != null && _Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        RawMedia rawMedia = _listRawMedia.Find(p => p.IQ_CC_Key.Equals(dr["IQ_CC_KEY"]));
                        if (rawMedia != null)
                        {
                            rawMedia.IQNielenseAudience = Convert.ToString(dr["AUDIENCE"]);
                            rawMedia.IQAddShareValue = Convert.ToString(dr["SQAD_SHAREVALUE"]);
                        }
                    }
                }
                return _listRawMedia;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
