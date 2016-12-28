using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    public interface IUGCRawMediaController
    {
        List<UGCRawMedia> GetUGCRawMediaBySearch(Guid p_ClientGUID, int p_PageNo, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount, string p_CategoryGUID, string p_CustomerGUID, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTermTitle, string p_SearchTermKeyword, string p_SearchTermDesc, out int p_ErrorNumber);

        UGCRawMedia GetUGCRawMediabyUGCGUID(UGCRawMedia _InUGCRawMedia);

        string UpdateUGCRawMedia(Guid p_RawMediaID, Guid p_CustomerGUID, Guid p_CategoryGUID, Guid? p_SubCategory1GUID, Guid? p_SubCategory2GUID, Guid? p_SubCategory3GUID, string p_Title, string p_Keyword, string p_Description);

        string DeleteUGCRawMedia(string p_UGCRawMediaIDs);

        string FillRecordsFromCore(Guid p_ClientGUID);

        string GetUGCFilePathByUGCGUID(Guid p_UGCGUID);

        string UpdateUGCRawMedia(Guid p_RawMediaID, SqlXml p_MetaData);
    }
}
