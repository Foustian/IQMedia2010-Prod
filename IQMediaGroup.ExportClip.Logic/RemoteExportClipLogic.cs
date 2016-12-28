using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.Objects;
using IQMediaGroup.ExportClip.Domain;

namespace IQMediaGroup.ExportClip.Logic
{
    public class RemoteExportClipLogic : BaseLogic, ILogic
    {
        public RemoteExportClipOutput InsertRemoteExportClip(Guid _ClipGUID, string _ClipFTPLocation, string _ClipInfo)
        {
            try
            {
                RemoteExportClipOutput _RemoteExportClipOutput = new RemoteExportClipOutput();

                IQService_Export _IQService_Export = new IQService_Export();
                _IQService_Export.ClipGUID = _ClipGUID;
                _IQService_Export.ClipRemoteLocation = _ClipFTPLocation;
                _IQService_Export.ClipInfo = _ClipInfo;
                Context.IQService_Export.AddObject(_IQService_Export);

                _RemoteExportClipOutput.Message = "Export Clip Inserted Successfully";
                _RemoteExportClipOutput.Status = 0;

                return _RemoteExportClipOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
