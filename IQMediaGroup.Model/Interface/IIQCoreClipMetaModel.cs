using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface Model for IQCore_ClipMeta
    /// </summary>
    public interface IIQCoreClipMetaModel
    {
        /// <summary>
        /// This method get FilPath of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        DataSet GetClipPathByClipGUID(Guid ClipGUID);

        /// <summary>
        /// Update No. Of Times File Downloaded by ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns>Result of Update </returns>
        string UpdateDownloadCountByClipGUID(Guid ClipGUID);
    }
}
