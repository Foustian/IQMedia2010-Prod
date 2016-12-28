using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using System.Configuration;
using System.Xml;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Services.IOS.Web.Serializers;
using IQMediaGroup.Logic.IOS;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class UploadUGC : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string output = string.Empty;
            var _UploadUGCOutput = new UploadUGCOutput();
            Log4NetLogger.Debug("UploadUGC Request Started");
            try
            {
                if (HttpRequest.Files.Count > 0 && HttpRequest.Files[0] != null)
                {
                    Log4NetLogger.Info("UGC File : " + HttpRequest.Files[0].FileName);
                }
                else
                {
                    Log4NetLogger.Info("UGC File : " + string.Empty);
                }
                
                Log4NetLogger.Info("Title : " + HttpRequest["Title"]);
                Log4NetLogger.Info("Description : " + HttpRequest["Description"]);
                Log4NetLogger.Info("Keywords : " + HttpRequest["Keywords"]);
                Log4NetLogger.Info("StartTime : " + HttpRequest["StartTime"]);
                Log4NetLogger.Info("EndTime : " + HttpRequest["EndTime"]);

                if (HttpRequest.Files.Count > 0)
                {
                    
                    //string _FileName = Path.GetFileName(HttpRequest.Files[0].FileName);
                    //_FileName = Regex.Replace(_FileName, @"[\s\\/]", "_");
                    //_FileName = Path.GetFileNameWithoutExtension(_FileName) + DateTime.Now.ToString("MMddyyyy_hhmmss") + Path.GetExtension(_FileName);

                    string _FileName = HttpRequest["UDID"] + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss") + Path.GetExtension(HttpRequest.Files[0].FileName);
                    string _FileLocation = ConfigurationManager.AppSettings["UGCFileLocation"] + _FileName;
                    HttpRequest.Files[0].SaveAs(_FileLocation);

                    UGCUploadInput _UGCUploadInput = new UGCUploadInput();
                    _UGCUploadInput.FileName = _FileName;
                    _UGCUploadInput.Title =HttpRequest["Title"];
                    _UGCUploadInput.Keywords =HttpRequest["Keywords"];
                    _UGCUploadInput.Description =HttpRequest["Description"];
                    _UGCUploadInput.StartTime =HttpRequest["StartTime"];
                    _UGCUploadInput.EndTime =HttpRequest["EndTime"];
                    _UGCUploadInput.UUID =HttpRequest["UDID"];

                    XDocument xdoc = new XDocument(
                                            new XElement("Root",
                                                new XElement("Title", _UGCUploadInput.Title),
                                                new XElement("Description", _UGCUploadInput.Description),
                                                new XElement("Keywords", _UGCUploadInput.Keywords),
                                                new XElement("StartTime", _UGCUploadInput.StartTime),
                                                new XElement("EndTime", _UGCUploadInput.EndTime),
                                                new XElement("UGCLocation", _FileLocation),
                                                new XElement("UDID", _UGCUploadInput.UUID)
                                            )
                                        );

                    XmlWriterSettings xws = new XmlWriterSettings { OmitXmlDeclaration = true };
                    using (XmlWriter xw = XmlWriter.Create(_FileLocation + ".xml", xws))
                        xdoc.Save(xw);

                    var ugcUploadLogic = (UGCUploadLogic)LogicFactory.GetLogic(LogicType.UGCUpload);
                    long _output = ugcUploadLogic.InsertUGCUpload(_UGCUploadInput);
                    if (_output > 0)
                    {
                        _UploadUGCOutput.Message = "Success";
                        _UploadUGCOutput.Status = 0;
                    }
                    else
                    {
                        _UploadUGCOutput.Message = "Failed to insert in DB";
                        _UploadUGCOutput.Status = 0;
                    }

                }
                else
                {
                    throw new CustomException("Video file not exist");
                }
            }
            catch (CustomException ex)
            {
                _UploadUGCOutput.Message = ex.Message;
                _UploadUGCOutput.Status = 1;
                Log4NetLogger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _UploadUGCOutput.Message = "An error occured, please try again!!";
                _UploadUGCOutput.Status = 1;
                Log4NetLogger.Error("Error : " + ex.ToString() + " stack : " + ex.StackTrace);
            }

            HttpResponse.ContentType = "application/json";
            output = Serializer.Searialize(_UploadUGCOutput);

            Log4NetLogger.Debug("UploadUGC Request Ended");
            HttpResponse.Output.Write(output);

        }
    }
}