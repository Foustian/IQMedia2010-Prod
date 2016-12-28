using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;
using System.Data.SqlTypes;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class ArchiveClipModel : IQMediaGroupDataLayer, IArchiveClipModel
    {
        /// <summary>
        /// Description:This Method will Insert Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ArchiveClip">Object of ArchiveClip</param>
        /// <returns>Primary Key of Archive Clip.</returns>
        public string InsertClip(ArchiveClip _ArchiveClip)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClipID", DbType.Guid, _ArchiveClip.ClipID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipLogo", DbType.String, _ArchiveClip.ClipLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipTitle", DbType.String, _ArchiveClip.ClipTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDate", DbType.DateTime, _ArchiveClip.ClipDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, _ArchiveClip.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int64, _ArchiveClip.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Category", DbType.String, _ArchiveClip.Category, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipCreationDate", DbType.String, _ArchiveClip.ClipCreationDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, _ArchiveClip.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClosedCaption", DbType.String, _ArchiveClip.ClosedCaption, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArchiveClipKey", DbType.Int32, _ArchiveClip.ArchiveClipKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_ArchiveClip_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception; 
            }
        }

        /// <summary>
        /// Description:This Method will get Archive Clip.
        /// </summary>
        /// <param name="_ArchiveClip">Object of ArchiveClip</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        public DataSet GetArchiveClip(ArchiveClip _ArchiveClip)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, _ArchiveClip.CustomerID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectByCustomerID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get Archive clip by ClipID
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ArchiveClip">object of ArchiveClip</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        public DataSet GetArchiveClipByClipID(ArchiveClip _ArchiveClip)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClipID", DbType.Guid , _ArchiveClip.ClipID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectByClipID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        
        /// <summary>
        /// Description:This Method will get Archive clip by Search Text.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ArchiveClip">object of ArchiveClip</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        public DataSet GetArchiveClipBySearchText(ArchiveClip _ArchiveClip)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@SearchText", DbType.String, _ArchiveClip.SearchText, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Category", DbType.String, _ArchiveClip.Category, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, _ArchiveClip.ClientID, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectBySearchText", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get Archive clip by Date.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_StartDate">Start Date of Clip.</param>
        /// <param name="p_EndDate">End Date of Clip.</param>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        public DataSet GetArchiveClipByDate(DateTime p_StartDate, DateTime p_EndDate, int p_CustomerID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@StartDate", DbType.DateTime, p_StartDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@EndDate", DbType.DateTime, p_EndDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_CustomerID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectByDate", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get Archive clip by CustomerID.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        public DataSet GetArchiveClipByCustomer(Int64 p_CustomerID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_CustomerID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectByCustomerID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will Insert Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary Key of Archive Clip</returns>
        public string InsertArchiveClip(SqlXml p_SqlXml)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_SqlXml, ParameterDirection.Input));

                string _ReturnValue= this.ExecuteNonQuery("usp_ArchiveClip_InsertList", _ListOfDataType);

                return _ReturnValue;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }


        /// <summary>
        ///  Description:This Method will Delete Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_DeleteArchiveClip">Primary Key of Archive Clip</param>
        /// <returns>Count</returns>
        public string DeleteArchiveClip(string p_DeleteArchiveClip)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArchiveClipKeys", DbType.String, p_DeleteArchiveClip, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_ArchiveClip_Delete", _ListOfDataType);

                return _Result;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

    }
}
