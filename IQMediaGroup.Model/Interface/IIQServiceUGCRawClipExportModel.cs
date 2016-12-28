using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IQMediaGroup.Model.Interface
{

    /// <summary>
    /// Interface of IIQServiceUGCRawClipExportModel
    /// </summary>
    public interface IIQServiceUGCRawClipExportModel
    {
         /// <summary>
        /// This method get the OutPutPAth of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        DataSet GetClipPathByClipGUID(Guid ClipGUID);
    }
}
