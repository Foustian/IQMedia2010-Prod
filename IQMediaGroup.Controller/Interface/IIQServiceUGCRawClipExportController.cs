using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQServiceUGCRawClipExportController
    {
        /// <summary>
        /// This method get the OutPutPAth of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        string GetClipPathByClipGUID(Guid ClipGUID);

        
    }
}
