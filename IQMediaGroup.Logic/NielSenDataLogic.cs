using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Xml.Linq;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public class NielSenDataLogic : BaseLogic, ILogic
    {
        public NielSenDataOutput GetNielSenData(NielSenDataInput _NielSenInput)
        {
            NielSenDataOutput _NielSenDataOutput = new NielSenDataOutput();
            try
            {
                List<NielSenDataDB> _ListOfNielSenDataDB = Context.GetNielSenDataByGuid(_NielSenInput.Guid, _NielSenInput.IsRawMedia, _NielSenInput.IQ_Start_Point, _NielSenInput.IQ_Dma_Num, _NielSenInput.ClientGuid.Value).ToList();
                _NielSenDataOutput.NielSenData = _ListOfNielSenDataDB;
                _NielSenDataOutput.Status = 0;
                return _NielSenDataOutput;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public bool? CheckClientNielSenDataAccess(Guid _ClientGuid)
        {
            try
            {
                var HasAccess = Context.CheckForNielSenDataAccessByClientGUID(_ClientGuid);

                return HasAccess.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NielsenOutput GetNielsenByList(NielsenInput _NielsenInput)
        {
            NielsenOutput _NielsenOutput = new NielsenOutput();
            try
            {
                var NielsenXml = new XElement("list",
                                                from RawMedia _NielsenInputList in _NielsenInput.RawMediaSet
                                                select new XElement("item",
                                                    new XAttribute("guid", _NielsenInputList.Guid),
                                                    new XAttribute("iq_dma", _NielsenInputList.IQ_Dma_Num)
                                                )
                                             );

                List<NielsenData> _ListOfNielsenOutputDB = null;

                try
                {
                    _ListOfNielsenOutputDB = Context.GetNielSenDataByRecordFileGuidList(NielsenXml.ToString(), _NielsenInput.ClientGuid.Value).ToList();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Debug("NielsenLogicError: " + ex.ToString());
                    Context.Connection.Open();

                    _ListOfNielsenOutputDB = Context.GetNielSenDataByRecordFileGuidList(NielsenXml.ToString(), _NielsenInput.ClientGuid.Value).ToList();
                }

                _NielsenOutput.NielsenDataSet = _ListOfNielsenOutputDB;
                _NielsenOutput.Status = 0;
                _NielsenOutput.Message = "Success";
                return _NielsenOutput;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VideoNielSenDataOutput GetNielSenData(VideoNielSenDataInput _VideoNielSenDataInput)
        {
            VideoNielSenDataOutput _VideoNielSenDataOutput = new VideoNielSenDataOutput();
            try
            {
                List<NielSenDataDB> _ListOfNielSenDataDB = Context.GetNielSenDataByGuid(_VideoNielSenDataInput.Guid, _VideoNielSenDataInput.IsRawMedia, _VideoNielSenDataInput.IQ_Start_Point, _VideoNielSenDataInput.IQ_Dma_Num, _VideoNielSenDataInput.ClientGuid.Value).ToList();
                _VideoNielSenDataOutput.NielSenData = _ListOfNielSenDataDB;
                _VideoNielSenDataOutput.Status = 0;
                return _VideoNielSenDataOutput;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public List<NielsenDemographicData> GetNielsenDemographicByList(NielsenDemographicInput p_NlsnDgfInput)
        {
            List<NielsenDemographicData> nlsnDgfDataList = null;
            XElement NielsenXml = null;

            if (!p_NlsnDgfInput.IsGUID)
            {

                var iqcckeyCom = new NielsenDemographicIQCCKeyComparer();
                var iqcckeyList = p_NlsnDgfInput.IQCCKeyList.Distinct(iqcckeyCom);

                NielsenXml = new XElement("list",
                                                        from NielsenDemographicIQCCKey nlsenDgfIQCCKey in iqcckeyList
                                                        select new XElement("item",
                                                            new XAttribute("iq_cc_key", nlsenDgfIQCCKey.IQCCKey),
                                                            new XAttribute("iq_dma", nlsenDgfIQCCKey.IQ_Dma_Num),
                                                            new XAttribute("Start", nlsenDgfIQCCKey.IQStartPoint)
                                                        )
                                                     );
            }
            else
            {
                var guidComp = new NielsenDemographicGUIDComparer();
                var guidList = p_NlsnDgfInput.GUIDList.Distinct(guidComp);

                NielsenXml = new XElement("list",
                                                        from NielsenDemographicGUID nlsenDgfGUID in guidList
                                                        select new XElement("item",
                                                            new XAttribute("guid", nlsenDgfGUID.GUID),
                                                            new XAttribute("iq_dma", nlsenDgfGUID.IQ_Dma_Num),
                                                            new XAttribute("Start", nlsenDgfGUID.IQStartPoint)
                                                        )
                                                     );
            }

            

            if (p_NlsnDgfInput.IsGUID)
            {
                nlsnDgfDataList = Context.GetNielsenDemographicDataByGUID(p_NlsnDgfInput.ClientGuid, NielsenXml.ToString());
            }
            else
            {
                nlsnDgfDataList = Context.GetNielsenDemographicDataByIQCCKey(p_NlsnDgfInput.ClientGuid, NielsenXml.ToString());
            }           

            return nlsnDgfDataList;
        }
    }
}
