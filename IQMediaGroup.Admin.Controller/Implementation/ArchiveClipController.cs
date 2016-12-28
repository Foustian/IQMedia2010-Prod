using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Collections;
using System.Xml.Linq;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class ArchiveClipController : IArchiveClipController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IArchiveClipModel _IArchiveClipModel;

        public ArchiveClipController()
        {
            _IArchiveClipModel = _ModelFactory.CreateObject<IArchiveClipModel>();
        }

        /// <summary>
        /// This method inserts Client Role Information
        /// </summary>
        /// <param name="p_ArchiveClips">Object Of ArchiveClips Class</param>
        /// <returns></returns>
        public string InsertArchiveClip(ArchiveClip p_ArchiveClips)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveClipModel.InsertClip(p_ArchiveClips);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods gets Archive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">List of Object of Archive Clip</param>
        /// <returns>List of Object of Archive Clip</returns>
        public List<ArchiveClip> GetArchiveClip(ArchiveClip p_ArchiveClip)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClip(p_ArchiveClip);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description: This Methods FillsArchive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Archive Clip Infromarmation</param>
        /// <returns>List of Object of Archive Clip</returns>
        private List<ArchiveClip> FillArchiveClipInformation(DataSet _DataSet)
        {
            List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ArchiveClip _ArchiveClip = new ArchiveClip();
                        _ArchiveClip.ClipDate = Convert.ToDateTime(_DataRow["ClipDate"]);
                        _ArchiveClip.ClipID = new Guid(_DataRow["ClipID"].ToString());
                        _ArchiveClip.ClipLogo = Convert.ToString(_DataRow["ClipLogo"]);
                        _ArchiveClip.ClipTitle = Convert.ToString(_DataRow["ClipTitle"]);
                        _ArchiveClip.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _ArchiveClip.ClipCreationDate = Convert.ToDateTime(_DataRow["ClipCreationDate"]);
                        _ArchiveClip.ClosedCaption = Convert.ToString(_DataRow["ClosedCaption"]);
                        _ArchiveClip.Description = Convert.ToString(_DataRow["Description"]);
                        _ArchiveClip.CustomerID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _ArchiveClip.Category = Convert.ToString(_DataRow["Category"]);
                        _ArchiveClip.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        _ArchiveClip.ArchiveClipKey = Convert.ToInt32(_DataRow["ArchiveClipKey"]);
                        //_ArchiveClip.ClipThumbNailImage = (byte[])_DataRow["ClipImageContent"];
                        _ListOfArchiveClip.Add(_ArchiveClip);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip </returns>
        public List<ArchiveClip> GetArchiveClipByClipID(ArchiveClip p_ArchiveClip)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipByClipID(p_ArchiveClip);
                _ListOfArchiveClip = FillArchiveClipInformationByClipID(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description: This Methods Fills Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Archive Clip by ClipID Infromarmation</param>
        /// <returns>List of Object of Archive Clip</returns>
        private List<ArchiveClip> FillArchiveClipInformationByClipID(DataSet _DataSet)
        {
            List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ArchiveClip _ArchiveClip = new ArchiveClip();
                        _ArchiveClip.ClipDate = Convert.ToDateTime(_DataRow["ClipDate"]);
                        _ArchiveClip.ClipLogo = Convert.ToString(_DataRow["ClipLogo"]);
                        _ArchiveClip.ClipTitle = Convert.ToString(_DataRow["ClipTitle"]);
                        _ArchiveClip.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _ArchiveClip.ClipCreationDate = Convert.ToDateTime(_DataRow["ClipCreationDate"]);
                        _ArchiveClip.ClosedCaption = Convert.ToString(_DataRow["ClosedCaption"]);
                        _ArchiveClip.Description = Convert.ToString(_DataRow["Description"]);
                        _ArchiveClip.CustomerID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _ListOfArchiveClip.Add(_ArchiveClip);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip by ClipID</returns>
        public List<ArchiveClip> GetArchiveClipBySearchText(ArchiveClip p_ArchiveClip)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipBySearchText(p_ArchiveClip);
                _ListOfArchiveClip = FillArchiveClipInformationBySearchText(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description: This Methods Fills Archive Clip by search text Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Client Role Infromarmation</param>
        /// <returns>List of Object of Clip by search</returns>
        private List<ArchiveClip> FillArchiveClipInformationBySearchText(DataSet _DataSet)
        {
            List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ArchiveClip _ArchiveClip = new ArchiveClip();
                        _ArchiveClip.ClipID = new Guid(_DataRow["ClipID"].ToString());
                        _ArchiveClip.ClipDate = Convert.ToDateTime(_DataRow["ClipDate"]);
                        _ArchiveClip.ClipLogo = Convert.ToString(_DataRow["ClipLogo"]);
                        _ArchiveClip.ClipTitle = Convert.ToString(_DataRow["ClipTitle"]);
                        _ArchiveClip.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _ArchiveClip.ClipCreationDate = Convert.ToDateTime(_DataRow["ClipCreationDate"]);
                        _ArchiveClip.ClosedCaption = Convert.ToString(_DataRow["ClosedCaption"]);
                        _ArchiveClip.Description = Convert.ToString(_DataRow["Description"]);
                        _ArchiveClip.CustomerID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _ArchiveClip.Category = Convert.ToString(_DataRow["Category"]);
                        _ArchiveClip.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        _ListOfArchiveClip.Add(_ArchiveClip);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// This method gets Archive clip data between Start Date And End Date
        /// </summary>
        /// <param name="p_StartDate">Start Date</param>
        /// <param name="p_EndDate">End Date</param>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>List of object of Archive Clip Details</returns>
        public List<ArchiveClipExport> GetArchiveClipByDate(DateTime p_StartDate, DateTime p_EndDate, int p_CustomerID)
        {
            DataSet _DataSet = null;
            List<ArchiveClipExport> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipByDate(p_StartDate, p_EndDate, p_CustomerID);
                _ListOfArchiveClip = FillArchiveClipInformationByDate(_DataSet);
                return _ListOfArchiveClip;
            }
            catch (Exception _Exceprion)
            {
                throw _Exceprion;
            }
        }

        /// <summary>
        /// Description: This Methods FillsArchive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Archive Clip Infromarmation</param>
        /// <returns>List of Object of Archive Clip</returns>
        private List<ArchiveClipExport> FillArchiveClipInformationByDate(DataSet _DataSet)
        {
            List<ArchiveClipExport> _ListOfArchiveClip = new List<ArchiveClipExport>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {

                        ArchiveClipExport _ArchiveClip = new ArchiveClipExport();

                        _ArchiveClip.PMGCLIPID = Convert.ToInt32(_DataRow["ArchiveClipKey"]);
                        _ArchiveClip.Clip_Air_Date = Convert.ToDateTime(_DataRow["ClipDate"]);
                        _ArchiveClip.Clip_GUID = new Guid(_DataRow["ClipID"].ToString());
                        _ArchiveClip.Clip_Title = Convert.ToString(_DataRow["ClipTitle"]);
                        _ArchiveClip.Clip_Creation_Date = Convert.ToString(_DataRow["ClipCreationDate"]);
                        _ArchiveClip.Clip_CC = Convert.ToString(_DataRow["ClosedCaption"]);
                        _ArchiveClip.Clip_Description = Convert.ToString(_DataRow["Description"]);
                        _ArchiveClip.PMGCustomer_ID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _ArchiveClip.Clip_export_file_date = DateTime.Now.ToString();

                        //if ((_DataRow["ClipImageContent"]).Equals(System.DBNull.Value))
                        //{
                        //    _ArchiveClip.Clip_ThumbNail = null;
                        //}
                        //else
                        //{
                        //    _ArchiveClip.Clip_ThumbNail = (byte[])(_DataRow["ClipImageContent"]);
                        //}
                        _ArchiveClip.Clip_Category = Convert.ToString(_DataRow["Category"]);



                        _ListOfArchiveClip.Add(_ArchiveClip);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description:This method will Generate List To XML.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ListOfArchiveClip">List of Object of Archive Clip</param>
        /// <returns>XDocument</returns>
        private XDocument GenerateListToXML(List<ArchiveClip> _ListOfArchiveClip)
        {
            XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                      new XElement("list",
                       from _ArchiveClip in _ListOfArchiveClip
                       select new XElement("Element",
                           string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipID)) ? null :
                       new XAttribute("ClipID", _ArchiveClip.ClipID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipLogo)) ? null :
                       new XAttribute("ClipLogo", _ArchiveClip.ClipLogo),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipTitle)) ? null :
                       new XAttribute("ClipTitle", _ArchiveClip.ClipTitle),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipDate)) ? null :
                       new XAttribute("ClipDate", _ArchiveClip.ClipDate),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.FirstName)) ? null :
                       new XAttribute("FirstName", _ArchiveClip.FirstName),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CustomerID)) ? null :
                       new XAttribute("CustomerID", _ArchiveClip.CustomerID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Category)) ? null :
                       new XAttribute("Category", _ArchiveClip.Category),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Description)) ? null :
                       new XAttribute("Description", _ArchiveClip.Description),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClosedCaption)) ? null :
                       new XAttribute("ClosedCaption", _ArchiveClip.ClosedCaption),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipCreationDate)) ? null :
                       new XAttribute("ClipCreationDate", _ArchiveClip.ClipCreationDate),
                       //string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipThumbNailImage)) ? null :
                       //new XAttribute("ClipImageContent", Convert.ToBase64String(_ArchiveClip.ClipThumbNailImage)),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CreatedDate)) ? null :
                       new XAttribute("CreatedDate", _ArchiveClip.CreatedDate),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ModifiedDate)) ? null :
                       new XAttribute("ModifiedDate", _ArchiveClip.ModifiedDate),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.IsActive)) ? null :
                       new XAttribute("IsActive", _ArchiveClip.IsActive)
                           )));
            return xmlDocument;
        }

        /// <summary>
        /// Description:This method will Fetch List Of Clips.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Response">Response</param>
        /// <returns>List of object of Clip</returns>
        private List<Clip> FetchListOfClips(string p_Response)
        {
            try
            {
                List<Clip> _ListOfClip = new List<Clip>();

                string[] _ClipArray = p_Response.Split(Convert.ToChar(CommonConstants.Comma));

                foreach (string _String in _ClipArray)
                {

                    Regex _Regex = new Regex(CommonConstants.RegexExClipID);

                    System.Text.RegularExpressions.MatchCollection _Match = _Regex.Matches(_String);

                    if (_Match.Count > 0)
                    {
                        Clip _Clip = new Clip();
                        _Clip.ClipID = new Guid(_Match[0].Value);

                        _ListOfClip.Add(_Clip);
                    }
                }

                return _ListOfClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will Read Params From File.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_FilePath">FilePath</param>
        /// <returns>object of SearchParametersArchiveClip</returns>
        private SearchParametersArchiveClip ReadParamsFromFile(string p_FilePath)
        {
            try
            {
                SearchParametersArchiveClip _SearchParametersArchiveClip = new SearchParametersArchiveClip();

                if (!string.IsNullOrEmpty(p_FilePath))
                {
                    string _FolderPath = p_FilePath;
                    FileInfo _FileInfo = new FileInfo(_FolderPath);
                    string _ExistingInfo = string.Empty;

                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();

                        _XmlDocument.Load(_FolderPath);
                        _ExistingInfo = _XmlDocument.InnerXml;

                        _SearchParametersArchiveClip = (SearchParametersArchiveClip)(CommonFunctions.MakeDeserialiazation(_ExistingInfo, _SearchParametersArchiveClip));
                    }
                    else
                    {
                        throw new MyException(CommonConstants.MsgFileNotExist);
                    }
                }


                if (CommonFunctions.GetBoolValue(_SearchParametersArchiveClip.DebugFileFlag)!=null && CommonFunctions.GetBoolValue(_SearchParametersArchiveClip.DebugFileFlag) == true)
                {
                    string _FolderPathTime = _SearchParametersArchiveClip.DebugFilePath;

                    DirectoryInfo _DirectoryInfo;
                    if (!Directory.Exists(_FolderPathTime))
                    {
                        throw new MyException("Debug File Path does not exist.");
                    }
                    else
                    {
                        _DirectoryInfo = new DirectoryInfo(_FolderPathTime);
                    }
                }

                if (string.IsNullOrEmpty(_SearchParametersArchiveClip.LoopCount))
                {
                    _SearchParametersArchiveClip.LoopCount = CommonConstants.DFLoopCount.ToString();
                }

                if (string.IsNullOrEmpty(_SearchParametersArchiveClip.MaxRecords))
                {
                    _SearchParametersArchiveClip.MaxRecords = CommonConstants.DFMaxRecords.ToString();
                }

                if (CommonFunctions.GetIntValue(_SearchParametersArchiveClip.MaxRecords) == null || CommonFunctions.GetIntValue(_SearchParametersArchiveClip.MaxRecords) <= 0 || CommonFunctions.GetIntValue(_SearchParametersArchiveClip.MaxRecords)>2000000000)
                {
                    throw new MyException(CommonConstants.ParamMaxRecords + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }

                if (string.IsNullOrEmpty(_SearchParametersArchiveClip.ResponseTimeOut))
                {
                    _SearchParametersArchiveClip.ResponseTimeOut = CommonConstants.DFCCSearchResponseTimeOut.ToString();
                }

                if (CommonFunctions.GetIntValue(_SearchParametersArchiveClip.LoopCount) == null || CommonFunctions.GetIntValue(_SearchParametersArchiveClip.LoopCount) <= 0)
                {
                    throw new MyException(CommonConstants.MsgLoopCountInvalid);
                }

                if (CommonFunctions.GetIntValue(_SearchParametersArchiveClip.ResponseTimeOut) == null || CommonFunctions.GetIntValue(_SearchParametersArchiveClip.ResponseTimeOut) <= 0)
                {
                    throw new MyException(CommonConstants.MsgCCSearchResponseTimeOutInvalid);
                }

                if (string.IsNullOrEmpty(_SearchParametersArchiveClip.ConnectionString))
                {
                    throw new MyException(CommonConstants.ConfigconnectionString + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }              

                return _SearchParametersArchiveClip;
            }
            catch (MyException _MyException)
            {
                throw _MyException;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        
        /// <summary>
        /// Description:This method will Find Clip Key.
        /// </summary>
        /// <param name="p_FindClipKeyValue">FindClipKeyValue</param>
        /// <param name="p_String">String</param>
        /// <returns>Is Clip key or not.</returns>
        private bool FindClipKey(KeyValue p_FindClipKeyValue, string p_String)
        {
            try
            {
                if (p_String.Contains(p_FindClipKeyValue._FindKey))
                {
                    string _SubString = p_String;
                    _SubString = p_String.Substring(p_String.IndexOf("\"v\":"));
                    _SubString = _SubString.Substring(5, (_SubString.Length - 6));

                    p_FindClipKeyValue._KeyValue = _SubString;
                    p_FindClipKeyValue._SetKey = true;

                    return true;
                }

                return false;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        ///  Description:This method Get Archive Clip By Customer.
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>List of object of ArchiveClip</returns>
        public List<ArchiveClip> GetArchiveClipByCustomer(Int64 p_CustomerID)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipByCustomer(p_CustomerID);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_ListOfArchiveClip"></param>
        /// <returns></returns>
        private string GetXmlArchiveClip(IEnumerable p_ListOfArchiveClip)
        {
            try
            {

                MemoryStream stream = new MemoryStream();

                /*using (XmlWriter writer = XmlWriter.Create(stream))
                {*/
                XmlWriter writer = XmlWriter.Create(stream);
                writer.WriteStartElement("list");

                foreach (ArchiveClip _ArchiveClip in p_ListOfArchiveClip)
                {
                    writer.WriteStartElement("Element");

                    if (_ArchiveClip.ClipID != null)
                    {
                        writer.WriteAttributeString("ClipID", _ArchiveClip.ClipID.ToString());
                    }

                    if (_ArchiveClip.ClipLogo != null)
                    {
                        writer.WriteAttributeString("ClipLogo", _ArchiveClip.ClipLogo.ToString());
                    }

                    if (_ArchiveClip.ClipTitle != null)
                    {
                        writer.WriteAttributeString("ClipTitle", _ArchiveClip.ClipTitle.ToString());
                    }

                    if (_ArchiveClip.ClipDate != null)
                    {
                        writer.WriteAttributeString("ClipDate", _ArchiveClip.ClipDate.ToString());
                    }

                    if (_ArchiveClip.FirstName != null)
                    {
                        writer.WriteAttributeString("FirstName", _ArchiveClip.FirstName.ToString());
                    }

                   /* if (_ArchiveClip.CustomerID != null)
                    {
                        writer.WriteAttributeString("CustomerID", _ArchiveClip.CustomerID.ToString());
                    }*/

                    if (_ArchiveClip.Category != null)
                    {
                        writer.WriteAttributeString("Category", _ArchiveClip.Category.ToString());
                    }

                    if (_ArchiveClip.Description != null)
                    {
                        writer.WriteAttributeString("Description", _ArchiveClip.Description.ToString());
                    }

                    if (_ArchiveClip.ClosedCaption != null)
                    {
                        // writer.WriteAttributeString("ClosedCaption", _ArchiveClip.ClosedCaption.ToString()); 
                    }

                    if (_ArchiveClip.ClipCreationDate != null)
                    {
                        writer.WriteAttributeString("ClipCreationDate", _ArchiveClip.ClipCreationDate.ToString());
                    }

                    //if (_ArchiveClip.ClipThumbNailImage != null)
                    //{
                    //    writer.WriteAttributeString("ClipImageContent", _ArchiveClip.ClipThumbNailImage.ToString());
                    //}

                    if (_ArchiveClip.CreatedDate != null)
                    {
                        writer.WriteAttributeString("CreatedDate", _ArchiveClip.CreatedDate.ToString());
                    }

                    if (_ArchiveClip.ModifiedDate != null)
                    {
                        writer.WriteAttributeString("ModifiedDate", _ArchiveClip.ModifiedDate.ToString());
                    }

                    if (_ArchiveClip.IsActive != null)
                    {
                        writer.WriteAttributeString("IsActive", _ArchiveClip.IsActive.ToString());
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                stream.Position = 0;

                StreamReader _StreamReader = new StreamReader(stream);


                //return new SqlXml(stream);
                return _StreamReader.ReadToEnd();
                //}
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string InsertArchiveClip(SqlXml p_SqlXml)
        {
            try
            {
               string _ReturnValue= _IArchiveClipModel.InsertArchiveClip(p_SqlXml);
               return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void InsertIQServiceLog(string _Event, string _FilePath)
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                string _string = string.Empty;

                IIq_Service_logController _IIq_Service_logController = _ControllerFactory.CreateObject<IIq_Service_logController>();
                Iq_Service_log _Iq_Service_log = new Iq_Service_log();
                _Iq_Service_log.ModuleName = "RL_Clip_Fetch";
                _Iq_Service_log.CreatedDatetime = DateTime.Now;
                if (_Event == "Start Event")
                {
                    _Iq_Service_log.ServiceCode = "Start Event";
                }
                else
                {
                    _Iq_Service_log.ServiceCode = "Stop Event";
                }
                _Iq_Service_log.ConfigRequest = InsertIq_Service_log(_FilePath);

                _string = _IIq_Service_logController.AddRL_GUIDS(_Iq_Service_log);

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string InsertIq_Service_log(string _FilePath)
        {
            try
            {
                //
                string _RequestPath = _FilePath;
                string _FileContent = string.Empty;
                if (!string.IsNullOrEmpty(_RequestPath))
                {
                    //string _FolderPath = _RequestPath;
                    FileInfo _FileInfo = new FileInfo(_RequestPath);

                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();
                        _XmlDocument.Load(_RequestPath);
                        //_ExistingInfo = _XmlDocument.InnerXml;
                        _FileContent = "<ServiceLog>";
                        _FileContent += "<filePath>" + _RequestPath + "</filePath>";
                        _FileContent += "<configuration>" + _XmlDocument.InnerXml + "</configuration>";
                        _FileContent += "</ServiceLog>";
                        //_SearchParameters = (SearchParameters)(CommonFunctions.MakeDeserialiazation(_ExistingInfo, _SearchParameters));

                    }

                }
                return _FileContent;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_DeleteArchiveClip">DeleteArchiveClip</param>
        /// <returns>Count</returns>
        public string DeleteArchiveClip(string p_DeleteArchiveClip)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveClipModel.DeleteArchiveClip(p_DeleteArchiveClip);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
