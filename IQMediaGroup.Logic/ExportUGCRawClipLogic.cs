using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using System.Data.Objects;
using System.Runtime.Remoting.Contexts;
using System.IO;

namespace IQMediaGroup.Logic
{
    public class ExportUGCRawClipLogic : BaseLogic, ILogic
    {

        public string InsertIQService_UGCRawClipExport(System.Guid? ClipID)
        {
            string _result = string.Empty;

            try
            {

                IQService_UGCRawClipExport _IQService_UGCRawClipExport = Context.IQService_UGCRawClipExport.Where(UGCRawClipExport => UGCRawClipExport.ClipGUID.Equals(ClipID.Value)).SingleOrDefault();

                if (_IQService_UGCRawClipExport != null)
                {
                    Log4NetLogger.Debug("UGCRawClipExp is not null");
                }
                else
                {
                    Log4NetLogger.Debug("UGCRawClipExp is null");
                }

                if (_IQService_UGCRawClipExport == null)
                {
                    String _objfilapath = string.Empty;

                    _objfilapath = Convert.ToString(Context.GetIQCoreRecordfileMetaPathByClipGUID(ClipID.Value).SingleOrDefault());

                    if (!string.IsNullOrEmpty(_objfilapath))
                    {
                        if (File.Exists(_objfilapath))
                        {
                            _IQService_UGCRawClipExport = new IQService_UGCRawClipExport();
                            _IQService_UGCRawClipExport.ClipGUID = ClipID.Value;
                            _IQService_UGCRawClipExport.Status = "QUEUED";
                            _IQService_UGCRawClipExport.DateQueued = DateTime.UtcNow;
                            _IQService_UGCRawClipExport.LastModified = DateTime.UtcNow;

                            Context.IQService_UGCRawClipExport.AddObject(_IQService_UGCRawClipExport);
                            Context.SaveChanges();

                            try
                            {
                                var ugcClipExportSvc = new UGCClipExportWebServiceClient();
                                ugcClipExportSvc.WakeupService();
                            }
                            catch (Exception _Exception)
                            {
                                Log4NetLogger.Error("Record inserted successfully; exception occurred when wakeup service. - Exception: " + _Exception.Message + " Stacktrace: " + _Exception.StackTrace + " InnerEXception: " + _Exception.InnerException);
                            }

                            _result = "{\"Status\":0,\"Message\":\"" + "Data inserted successfully" + "\"}";
                        }
                        else
                        {
                            _result = "{\"Status\":1,\"Message\":\"" + "RawMedia File doesn’t exist" + "\"}";
                        }

                    }
                    else
                    {
                        _result = "{\"Status\":1,\"Message\":\"" + "RawMedia File details doesn’t exist" + "\"}";
                    }
                }
                else
                {
                    _result = "{\"Status\":1,\"Message\":\"" + "Clip is already exported" + "\"}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _result;

        }


    }
}
