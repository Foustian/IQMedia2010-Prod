using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQCoreClipMetaController
    {
        /// <summary>
        /// This method get the FilePath of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns>FilePath of Clip</returns>
        Dictionary<string, string> GetClipPathByClipGUID(Guid ClipGUID);

        /// <summary>
        /// Update No. Of Times File Downloaded by ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns>Result of Update </returns>
        string UpdateDownloadCountByClipGUID(Guid ClipGUID);
    }
}
