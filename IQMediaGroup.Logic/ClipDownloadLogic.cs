using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Net;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Text.RegularExpressions;

namespace IQMediaGroup.Logic
{
    public class ClipDownloadLogic : BaseLogic, ILogic
    {
        public enum ClipFormat
        {
            Audio,
            Video
        }

        private static string[] VideoFormat = { "flv", "wmv", "asf", "mp4" };
        private static string[] AudioFormat = { "wma", "mp3", "wav" };

        public string DownloadClip(Guid p_ClipID, Guid p_ClientGUID, Guid p_CustomerGUID, Guid p_CategoryGUID)
        {
            try
            {

                ClipDownloadSetting ClipDownloadSetting = Context.ClipDownloadSettings.SingleOrDefault(_ClipDownloadSettings => _ClipDownloadSettings.ClientGUID.Equals(p_ClientGUID) && ((bool)(_ClipDownloadSettings.IsActive)).Equals(true));
                Customer Customer = Context.Customers.SingleOrDefault(_Customer => _Customer.CustomerGUID.Equals(p_CustomerGUID) && ((bool)_Customer.IsActive == true));

                if (ClipDownloadSetting != null && Customer != null)
                {
                    ArchiveClipDB ArchiveClipDB = null;

                    int MaxNoOfReqToGetClipData = 3;
                    int DelayToGetClipData = 2500;

                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DelayToGetClipData"]))
                    {
                        DelayToGetClipData = Convert.ToInt32(Convert.ToDouble(ConfigurationManager.AppSettings["DelayToGetClipData"]) * 1000);
                    }

                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxNoOfReqToGetClipData"]))
                    {
                        MaxNoOfReqToGetClipData = Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfReqToGetClipData"]);
                    }

                    for (int Index = 0; Index < MaxNoOfReqToGetClipData; Index++)
                    {
                        Thread.Sleep(DelayToGetClipData);

                        

                        ArchiveClipDB = Context.GetArchiveClipDataByClipID(p_ClipID).SingleOrDefault();

                        if (ArchiveClipDB != null)
                        {
                            break;
                        }
                    }

                    if (ArchiveClipDB != null)
                    {
                        ArchiveClip ArchiveClip = Context.ArchiveClips.SingleOrDefault(_ArchiveClip => _ArchiveClip.ClipID.Equals(p_ClipID));

                       // ArchiveClip ArchiveClip = new ArchiveClip();
                        bool IsExistedArchiveClip = false;

                        if (ArchiveClip!=null)
                        {
                            IsExistedArchiveClip = true;                            
                        }
                        else
                        {
                            ArchiveClip = new ArchiveClip();
                        }

                        ArchiveClip.CustomerID = Customer.CustomerKey;
                        ArchiveClip.FirstName = Customer.FirstName;
                        ArchiveClip.CategoryGUID = p_CategoryGUID;
                        ArchiveClip.Category = ArchiveClipDB.Category;
                        ArchiveClip.ClientGUID = p_ClientGUID;
                        ArchiveClip.ClipCreationDate = ArchiveClipDB.ClipCreationDate;
                        ArchiveClip.ClipDate = ArchiveClipDB.ClipDate;
                        ArchiveClip.ClipID = p_ClipID;

                        string clipThumbnailImagePath = string.Empty;
                        clipThumbnailImagePath = ArchiveClipDB.ClipThumbnailImagePath;

                        if (string.IsNullOrWhiteSpace(clipThumbnailImagePath))
                        {
                            clipThumbnailImagePath = ConfigurationManager.AppSettings["DefaultImagePath"];
                        }

                        if (!string.IsNullOrWhiteSpace(clipThumbnailImagePath))
                        {
                            //ArchiveClip.ClipImageContent = GetThumbnailIMage(clipThumbnailImagePath);
                            ArchiveClip.ThumbnailImagePath = clipThumbnailImagePath;
                        }

                        ArchiveClip.ClipLogo = ArchiveClipDB.ClipLogo;
                        ArchiveClip.ClipTitle = ArchiveClipDB.ClipTitle;
                        ArchiveClip.ClosedCaption = GetClosedCaption(p_ClipID);
                        ArchiveClip.CreatedBy = "Service";
                        ArchiveClip.CreatedDate = DateTime.Now;
                        ArchiveClip.CustomerGUID = p_CustomerGUID;
                        ArchiveClip.Description = ArchiveClipDB.Description;
                        ArchiveClip.IsActive = true;
                        ArchiveClip.ModifiedBy = "Service";
                        ArchiveClip.ModifiedDate = DateTime.Now;
                        ArchiveClip.Keywords = ArchiveClipDB.Keywords;
                        

                        if (!IsExistedArchiveClip)
                        {
                            Context.ArchiveClips.AddObject(ArchiveClip); 
                        }

                        Context.SaveChanges();


                        string Format = Context.GetClipFormat(p_ClipID).Single();
                        string ClipDownloadFormat = string.Empty;

                        switch (CheckClipFormat(Format))
                        {
                            case ClipFormat.Audio:
                                ClipDownloadFormat = ClipDownloadSetting.AudioFormat;
                                break;
                            case ClipFormat.Video:
                                ClipDownloadFormat = ClipDownloadSetting.VideoFormat;
                                break;
                        }

                        string Response = MakeClipExportRequest(p_ClipID, ClipDownloadFormat);

                        if (Response.Trim().ToLower() == ConfigurationManager.AppSettings["ExportClipMsg"].Trim().ToLower())
                        {
                            ClipDownload ClipDownload = new Domain.ClipDownload();

                            ClipDownload.ClipDLRequestDateTime = DateTime.Now;
                            ClipDownload.ClipDownloadStatus = 2;
                            ClipDownload.ClipFileLocation = ClipDownloadSetting.ClipDownloadFileLocation;
                            ClipDownload.ClipID = p_ClipID;
                            ClipDownload.CreatedBy = "Service";
                            ClipDownload.CreatedDate = DateTime.Now;
                            ClipDownload.CustomerGUID = p_CustomerGUID;
                            ClipDownload.IsActive = true;
                            ClipDownload.ModifiedBy = "Service";
                            ClipDownload.ModifiedDate = DateTime.Now;
                            ClipDownload.ClipDLFormat = ClipDownloadFormat;

                            Context.ClipDownloads.AddObject(ClipDownload);
                            Context.SaveChanges();

                            GenerateXmlFile(ClipDownloadSetting.ClipDownloadFileLocation, ArchiveClipDB, p_ClientGUID, p_CustomerGUID);

                            return "Success: Clip download started";
                        }
                        else
                        {
                            return "Failed:" + Response.Trim();
                        }
                    }
                    else
                    {
                        return "Clip Data can not be found";
                    }
                }
                else
                {
                    return "Failure: Clip download not authorized";
                }
            }
            catch (Exception _Exception)
            {
                // return "Error :" + _Exception.Message + "__" + _Exception.StackTrace;
                throw _Exception;
            }
        }

        private void GenerateXmlFile(string p_FileLocation, ArchiveClipDB p_ArchiveClipDB, Guid p_ClientGUID, Guid p_CustomerGUID)
        {
            try
            {
                XElement MetaXElement = XElement.Parse(p_ArchiveClipDB.ClipMeta);

                XDocument XDocument = new XDocument(new XElement("ClipInfo",
                             new XElement("ClipID", p_ArchiveClipDB.ClipID),
                            new XElement("Title", p_ArchiveClipDB.ClipTitle),
                            new XElement("Category", p_ArchiveClipDB.Category),
                            new XElement("KeyWords", p_ArchiveClipDB.Keywords),
                            new XElement("Description", p_ArchiveClipDB.Description),
                            new XElement("Client", p_ClientGUID),
                            MetaXElement
                            ));

                string FileName = string.Empty;
                string ClipTitle = p_ArchiveClipDB.ClipTitle;

                ClipTitle = Regex.Replace(ClipTitle, "[/\\\\:<>|*?\"]", "-", RegexOptions.None);

                FileName = p_FileLocation + p_ClientGUID + "_" + ClipTitle + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss") + ".xml";

                XDocument.Save(FileName);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string MakeClipExportRequest(Guid p_ClipID, string p_Format)
        {
            try
            {
                string _URL = ConfigurationManager.AppSettings["ExportClip"] + "?fid=" + p_ClipID + "&fmt=" + p_Format;

                HttpWebRequest HttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_URL);

                HttpWebRequest.KeepAlive = false;

                StreamReader StreamReader = null;
                string Response = string.Empty;

                if ((HttpWebRequest.GetResponse().ContentLength > 0))
                {
                    StreamReader = new StreamReader(HttpWebRequest.GetResponse().GetResponseStream());
                    Response = StreamReader.ReadToEnd();
                    StreamReader.Dispose();
                }

                return Response;

                /*if (Response.Trim().ToLower() == ConfigurationManager.AppSettings["ExportClipMsg"].Trim().ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }*/
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GetClosedCaption(Guid p_ClipID)
        {
            try
            {
                string _Caption = string.Empty;

                string _ClosedCaptionURL = ConfigurationManager.AppSettings["GetClosedCaptionFromIQ"].ToString() + p_ClipID;

                WebClient client = new WebClient();
                _Caption = client.DownloadString(_ClosedCaptionURL);

                XmlDocument Xdoc = new XmlDocument();
                Xdoc.LoadXml(_Caption);

                _Caption = _Caption.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                _Caption = _Caption.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                _Caption = _Caption.Replace("<tt xmlns=\"http://www.w3.org/2006/10/ttaf1\" xml:lang=\"EN\">", "<tt>");
                _Caption = _Caption.Replace("<tt xml:lang=\"EN\" xmlns=\"http://www.w3.org/2006/10/ttaf1\">", "<tt>");

                XmlDocument _XmlDocument = new XmlDocument();

                try
                {
                    _XmlDocument.LoadXml(_Caption.ToString());
                }
                catch (XmlException)
                {
                    _Caption = "<tt><head><metadata><title></title></metadata></head><body region=\"subtitleArea\"><div><p begin=\"0s\" end=\"0s\">IQM-ClosedCaption is missing</p></div></body></tt>";
                }

                return _Caption;

            }
            catch (Exception _Exception)
            {
                return "<tt><head><metadata><title></title></metadata></head><body region=\"subtitleArea\"><div><p begin=\"0s\" end=\"0s\">IQM-ClosedCaption is missing</p></div></body></tt>";
                // throw _Exception;
            }
        }

        public static ClipFormat CheckClipFormat(string p_Format)
        {
            try
            {
                if (VideoFormat.Contains(p_Format.Trim().ToLower()))
                {
                    return ClipFormat.Video;
                }
                else if (AudioFormat.Contains(p_Format.Trim().ToLower()))
                {
                    return ClipFormat.Audio;
                }

                return ClipFormat.Audio;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }
    }
}
