using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;
using System.Data.SqlTypes;


namespace IQMediaGroup.Model.Implementation
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
                //_ListOfDataType.Add(new DataType("@ClipThumbNailImage", DbType.Binary, _ArchiveClip.ThumbnailImagePath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, _ArchiveClip.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, _ArchiveClip.CategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, _ArchiveClip.ClientGUID, ParameterDirection.Input));


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

        public DataSet GetArchiveClipFromXML(SqlXml ClipXML)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                DataSet _DataSet = new DataSet();

                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, ClipXML, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectByClipList", _ListOfDataType);

                return _DataSet;

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
        public DataSet GetAllArchiveClip(ArchiveClip _ArchiveClip)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectAll", _ListOfDataType);

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

        public DataSet GetArchiveClipByCustomerGUID(Guid p_CustomerGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Int32, p_CustomerGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectByCustomerGUID", _ListOfDataType);

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

        /// <summary>
        /// Update existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string UpdateArchiveClip(ArchiveClip p_ArchiveClip)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClipID", DbType.Guid, p_ArchiveClip.ClipID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipTitle", DbType.String, p_ArchiveClip.ClipTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_ArchiveClip.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_ArchiveClip.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_ArchiveClip.CategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1GUID", DbType.Guid, p_ArchiveClip.SubCategory1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2GUID", DbType.Guid, p_ArchiveClip.SubCategory2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3GUID", DbType.Guid, p_ArchiveClip.SubCategory3GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, p_ArchiveClip.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_ArchiveClip.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, p_ArchiveClip.Rating, ParameterDirection.Input));

                
                _Result = ExecuteNonQuery("usp_ArchiveClip_Update", _ListOfDataType);

                return _Result;
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
        /// <param name="_ArchiveClip">Object of ArchiveClip</param>
        /// <returns>Primary Key of Archive Clip.</returns>
        public string InsertArchiveClipFromConsole(ArchiveClip _ArchiveClip)
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
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, _ArchiveClip.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, _ArchiveClip.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClosedCaption", DbType.String, _ArchiveClip.ClosedCaption, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipCreationDate", DbType.String, _ArchiveClip.ClipCreationDate, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("@ClipThumbNailImage", DbType.Binary, _ArchiveClip.ThumbnailImagePath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, _ArchiveClip.CategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, _ArchiveClip.ClientGUID, ParameterDirection.Input));    
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, _ArchiveClip.CustomerGUID, ParameterDirection.Input));
                            

                _Result = ExecuteNonQuery("usp_ArchiveClip_InsertFromConsole", _ListOfDataType);
                

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get Data.
        /// Added By:Maulik Gandhi
        /// </summary>        
        /// <returns>DataSet of ArchiveClip.</returns>
        public DataSet GetData()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();


                _DataSet = this.GetDataSet("usp_ArchiveClip_SelectData", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveClipBySearchTerm(Guid p_ClientGUID, string p_SearchTerm,int p_PageNumber,int p_PageSize,string p_SortField,bool p_IsAscending,out int p_TotalRecordsCount)
        {
            try
            {
                p_TotalRecordsCount = 0;
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTerm", DbType.String, p_SearchTerm, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@TotalRecordsCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));
               

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_SearchBySearchTerm", _ListOfDataType,out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    p_TotalRecordsCount = Convert.ToInt32(_output);
                }

                return _DataSet;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        public DataSet GetArchiveClipBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, string p_ListCategory1GUID, string p_ListCategory2GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount)
        {
            try
            {
                p_TotalRecordsCount = 0;
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermTitle", DbType.String, p_SearchTermTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermDesc", DbType.String, p_SearchTermDesc, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermKeyword", DbType.String, p_SearchTermKeyword, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermCC", DbType.String, p_SearchTermCC, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDateFrom", DbType.Date, p_ClipFromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDateTo", DbType.Date, p_ClipToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID1", DbType.String, p_ListCategory1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID2", DbType.String, p_ListCategory2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_ListCustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsNielSen", DbType.Boolean, p_IsNielSenRights, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecordsClipCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));


                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_Search", _ListOfDataType, out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    p_TotalRecordsCount = Convert.ToInt32(_output);
                }

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetArchiveClipBySearchNew(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount)
        {
            try
            {
                p_TotalRecordsCount = 0;
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermTitle", DbType.String, p_SearchTermTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermDesc", DbType.String, p_SearchTermDesc, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermKeyword", DbType.String, p_SearchTermKeyword, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermCC", DbType.String, p_SearchTermCC, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDateFrom", DbType.Date, p_ClipFromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDateTo", DbType.Date, p_ClipToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID1", DbType.Guid, p_Category1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID2", DbType.Guid, p_Category2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID3", DbType.Guid, p_Category3GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID4", DbType.Guid, p_Category4GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator1", DbType.String, p_CategoryOperator1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator2", DbType.String, p_CategoryOperator2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator3", DbType.String, p_CategoryOperator3, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_ListCustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsNielSen", DbType.Boolean, p_IsNielSenRights, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecordsClipCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));


                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_SearchNew", _ListOfDataType, out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    p_TotalRecordsCount = Convert.ToInt32(_output);
                }

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetArchiveClipByParams(Guid p_ClientGUID, string p_ListCategoryGUID, string p_ListSubCategory1GUID, string p_ListSubCategory2GUID, string p_ListSubCategory3GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, string p_SearchText, string p_ClipTitle, out Guid? p_ClipID, out int p_TotalRecordsCount)
        {
            try
            {
                p_TotalRecordsCount = 0;
                p_ClipID = null;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Category", DbType.String, p_ListCategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1", DbType.String, p_ListSubCategory1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2", DbType.String, p_ListSubCategory2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3", DbType.String, p_ListSubCategory3GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_ListCustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTerm", DbType.String, p_SearchText, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipTitle", DbType.String, p_ClipTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipID", DbType.Guid, p_ClipID, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@TotalRecordsClipCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_SelectByParams", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    if (string.IsNullOrEmpty(_OutputParams["@ClipID"]))
                    {
                        p_ClipID = null;
                    }
                    else
                    {
                        p_ClipID = new Guid(_OutputParams["@ClipID"]);
                    }
                    p_TotalRecordsCount = Convert.ToInt32(_OutputParams["@TotalRecordsClipCount"]);
                }

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDate", DbType.Date, p_Date, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_Report", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID,string p_SortField, bool p_IsAscending,DateTime p_Date,Guid p_CategoryGuid)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDate", DbType.Date, p_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_CategoryGuid, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_SelectByCategory", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }



    }
}
