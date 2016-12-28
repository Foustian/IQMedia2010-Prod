using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;


namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IIQCoreClipMetaModel
    /// </summary>
    internal class IQCoreClipMetaModel : IQMediaGroupDataLayer, IIQCoreClipMetaModel
    {
        /// <summary>
        /// This method get the FilePAth of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        public DataSet GetClipPathByClipGUID(Guid ClipGUID)
        {

            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClipGUID", DbType.Guid, ClipGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQCore_ClipMeta_SelectFilePathByClipGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateDownloadCountByClipGUID(Guid ClipGUID)
        {
            string _Result = string.Empty;
            List<DataType> _ListOfDataType = new List<DataType>();

            _ListOfDataType.Add(new DataType("@ClipGUID", DbType.Guid, ClipGUID, ParameterDirection.Input));

            _Result = ExecuteNonQuery("usp_IQCore_ClipMeta_UpdateNoOfTimesDownloadByClipGUID", _ListOfDataType);

            return _Result;
        }
    }
}
