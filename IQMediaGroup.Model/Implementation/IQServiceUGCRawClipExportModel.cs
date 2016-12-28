using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;


namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IQServiceUGCRawClipExportModel
    /// </summary>
    internal class IQServiceUGCRawClipExportModel : IQMediaGroupDataLayer, IIQServiceUGCRawClipExportModel
    {
        /// <summary>
        /// This method get the OutPutPAth of Clip By ClipGUID
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

                _DataSet = this.GetDataSet("usp_IQService_UGCRawClipExport_SelectClipPathByClipGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        
        
    }
}
