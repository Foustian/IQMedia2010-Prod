using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.Objects;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Logic
{
    public class RecordFileLogic : BaseLogic, ILogic
    {

        public RecordFileOutput InsertRecordFile(RecordFileInput _RecordFileInput, string UGCXML)
        {

            try
            {
                RecordFileOutput _RecordFileOutput = new RecordFileOutput();


                var RecordFileGuid = new ObjectParameter("RecordFileGuid", typeof(Guid?));
                var DestinationFile = new ObjectParameter("DestinationFile", _RecordFileInput.DestinationFile);
                var Message = new ObjectParameter("Message", typeof(string));
                Context.Insert_IQCore_RecordFile(RecordFileGuid,
                                                              DestinationFile,
                                                              _RecordFileInput.FileExtension, Convert.ToDateTime(_RecordFileInput.StartDate),
                                                               Convert.ToDateTime(_RecordFileInput.EndDate), _RecordFileInput.EndOffset, _RecordFileInput.SourceGuid
                                                              , _RecordFileInput.RecordFileTypeID, _RecordFileInput.RootPathID, _RecordFileInput.Status
                                                              , _RecordFileInput.IsUGC, UGCXML, Message);



                string Messagevalue = (string)Message.Value;
                if (Messagevalue == "Record Inserted Successfully")
                {
                    _RecordFileOutput.Message = "Record File Created Successfully";
                    _RecordFileOutput.Status = 0;
                    _RecordFileOutput.RecordFileGUID = (Guid)RecordFileGuid.Value;
                    _RecordFileOutput.Location = (string)DestinationFile.Value;
                }
                else
                {
                    _RecordFileOutput.Message = "Error Occured While Creating Record File.";
                    _RecordFileOutput.Status = 1;
                }

                return _RecordFileOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public Int32 UpdateRecordFile(RecordFileUpdate _RecordFileUpdate)
        {
            try
            {
                Int32 _result = Context.Update_IQCore_RecordFile(new Guid(Convert.ToString(_RecordFileUpdate.RecordfileID))
                                                , _RecordFileUpdate.Location,
                                                _RecordFileUpdate.EndOffset,
                                                _RecordFileUpdate.RootPathID,
                                                _RecordFileUpdate.Status
                                                ).FirstOrDefault().Value;
                return _result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }


        }

        public Boolean CheckForClipByRecordFileGUID(Guid? mediaGuid)
        {
            try
            {
                Boolean isClipExists = Context.CheckClipByRecordFileGUID(mediaGuid).FirstOrDefault().Value;
                return isClipExists;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while Checking for Clip By RecordFile GUID :" + mediaGuid, ex);
                throw;
            }
        }

        public List<MediaLocation> GetMediaLocationByRecordFileGUID(Guid? mediaGuid)
        {
            try
            {
                var mediaLocationOutPut = Context.GetMediaLocationByRecordFileGUID(mediaGuid).ToList();
                return (List<MediaLocation>)mediaLocationOutPut;
            }
            catch (Exception ex)
            {

                Log4NetLogger.Fatal("An error occured while Getting Media Location By RecordFile GUID :" + mediaGuid, ex);
                throw;
            }
        }

        public bool UpdateRecordFileStatus(Guid? recordFileGuid, string status)
        {
            try
            {
                Int32? returnValue = Context.UpdateIQCoreRecordFileStatus(recordFileGuid, status).FirstOrDefault().Value;
                if (returnValue.HasValue && returnValue.Value > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while Updating Recodfile status By RecordFile GUID :" + recordFileGuid + " with status:" + status, ex);
                throw;
            }
        }        
    }
}
