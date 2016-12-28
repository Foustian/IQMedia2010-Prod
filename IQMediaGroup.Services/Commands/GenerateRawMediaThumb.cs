using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Logic;
using IQMediaGroup.Domain;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using System.IO;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GenerateRawMediaThumb : ICommand
    {
        public Int64? _ID { get; private set; }
        public Guid? _RawMediaGuid { get; private set; }
        public Int64? _Offset { get; private set; }
        public string _Format { get; private set; }


        public GenerateRawMediaThumb(object ID, object RawMediaGuid, object Offset, object Format)
        {
            _ID = (ID is NullParameter) ? null : (Int64?)ID;
            _Offset = (Offset is NullParameter) ? null : (Int64?)Offset;
            _Format = (Format is NullParameter) ? null : (String)Format;
            _RawMediaGuid = (RawMediaGuid is NullParameter) ? null : (Guid?)RawMediaGuid;

        }


        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {

            CLogger.Debug("GenrateRawMediaThumb Service Started");
            var generateRawMediaThumbOutput = new GenerateRawMediaThumbOutput();
            String OutputResult = string.Empty;

            try
            {

                if (_ID == null && _RawMediaGuid == null)
                {
                    throw new ArgumentException("Invalid or missing ID/RawMediaGuid");
                }

                if (_RawMediaGuid != null && _Offset == null)
                {
                    throw new ArgumentException("Invalid or missing Offset");
                }

                CLogger.Debug("{\"IQAgent_TVResultsID\":\"" + _ID + ",\"RawMediaGuid\":\"" + _RawMediaGuid + ",\"Offset\":" + _Offset + "}");

                Guid rlVideoGuid = new Guid();
                DateTime dtRLVideoDateTime = new DateTime();
                int rootPathID = 0;
                string fileLocation = string.Empty;
                Int32 rawMediaoffset = 0;
                var rawMediaLogic = (RawMediaLogic)LogicFactory.GetLogic(LogicType.RawMedia);
                bool isValidID = true;
                if (_ID == null) // This means we have to check for RawMediaGuid
                {
                    rlVideoGuid = (Guid)_RawMediaGuid;
                    var iQCoreRecordFile = (IQCoreRecordFile)rawMediaLogic.GetIQCoreRecordFileLocationByGUID(rlVideoGuid);
                    if (iQCoreRecordFile == null)
                    {
                        isValidID = false;
                    }
                    else
                    {
                        dtRLVideoDateTime = iQCoreRecordFile.RecordingDate;
                        rootPathID = iQCoreRecordFile.RootPathID;
                        fileLocation = iQCoreRecordFile.FileLocation;
                    }
                }
                else // This means we have to check for ID
                {
                    var iQAgentTVResults = rawMediaLogic.GetIQagentTVResultsByID(_ID);
                    if (iQAgentTVResults == null)
                    {
                        isValidID = false;
                    }
                    else
                    {
                        rlVideoGuid = (Guid)iQAgentTVResults.RL_VideoGUID;
                        dtRLVideoDateTime = iQAgentTVResults.RL_Date;
                        string responseHighlightedCC = iQAgentTVResults.CC_Highlight;

                        if (!string.IsNullOrEmpty(responseHighlightedCC))
                        {

                            HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                            highlightedCCOutput.CC = new List<ClosedCaption>();

                            highlightedCCOutput = (HighlightedCCOutput)Serializer.DeserialiazeXml(responseHighlightedCC, highlightedCCOutput);
                            if (highlightedCCOutput != null && highlightedCCOutput.CC != null && highlightedCCOutput.CC.Count > 0)
                            {
                                rawMediaoffset = highlightedCCOutput.CC.FirstOrDefault().Offset;
                                CLogger.Debug("Offset from HighlightedCC : " + rawMediaoffset);
                            }
                            else
                            {
                                rawMediaoffset = Convert.ToInt32(ConfigurationManager.AppSettings["Offset"]);
                                CLogger.Debug("Offset from HighlightedCC is null , default offset : " + rawMediaoffset);
                            }
                        }
                        else
                        {
                            rawMediaoffset = Convert.ToInt32(ConfigurationManager.AppSettings["Offset"]);
                            CLogger.Debug("Offset from HighlightedCC is null , default offset : " + rawMediaoffset);
                        }


                        var iQCoreRecordFile = (IQCoreRecordFile)rawMediaLogic.GetIQCoreRecordFileLocationByGUID(iQAgentTVResults.RL_VideoGUID);
                        if (iQCoreRecordFile == null)
                        {
                            isValidID = false;
                        }
                        else
                        {
                            rootPathID = iQCoreRecordFile.RootPathID;
                            fileLocation = iQCoreRecordFile.FileLocation;
                        }
                    }
                }


                if (!isValidID)
                {
                    generateRawMediaThumbOutput.Status = 1;
                    generateRawMediaThumbOutput.Message = "Fail - No Result Found for specified ID";
                }
                else
                {
                    /*var iQCoreRecordFile = (IQCoreRecordFile)rawMediaLogic.GetIQCoreRecordFileLocationByGUID(iQAgentTVResults.RL_VideoGUID);
                    if (iQCoreRecordFile != null)
                    {*/
                    if (_ID == null)
                    {
                        if (_Offset == null || _Offset == 0)
                        {
                            rawMediaoffset = Convert.ToInt32(ConfigurationManager.AppSettings["Offset"]);
                            CLogger.Debug("Offset is 0, default offset : " + rawMediaoffset);
                        }
                        else
                        {
                            if (_Offset - Convert.ToInt32(ConfigurationManager.AppSettings["Offset"]) >= 0)
                            {
                                rawMediaoffset = (int)(_Offset - Convert.ToInt32(ConfigurationManager.AppSettings["Offset"]));
                                CLogger.Debug("Offset : " + rawMediaoffset);
                            }
                            else
                            {
                                rawMediaoffset = (int)_Offset;
                                CLogger.Debug("Offset - default offset is less than 0, offset : " + rawMediaoffset);
                            }
                        }
                    }

                    //get StoragePath and StreamsuffixPath by Config define RootPath ID
                    var iQCoreRootPath = (IQCoreRootPath)rawMediaLogic.SelectIQCoreRootPathByID(Convert.ToInt64(ConfigurationManager.AppSettings["IQRootPathID"]));

                    String outputFile = ConfigurationManager.AppSettings["OutputDirectory"] + rlVideoGuid + ".jpg";
                    string outPutPath = String.Empty;
                    string outPutPathForRemoteService = string.Empty;

                    if (iQCoreRootPath != null && !string.IsNullOrWhiteSpace(iQCoreRootPath.StoragePath))
                    {
                        outPutPath = iQCoreRootPath.StoragePath + @"\" + dtRLVideoDateTime.Year + @"\" +
                                                                                    dtRLVideoDateTime.Month.ToString().PadLeft(2, '0') + @"\" + dtRLVideoDateTime.Day.ToString().PadLeft(2, '0') + @"\" +
                                                                                    rlVideoGuid + ".jpg";
                        outPutPathForRemoteService = dtRLVideoDateTime.Year + @"\" +
                                                                                    dtRLVideoDateTime.Month.ToString().PadLeft(2, '0') + @"\" + dtRLVideoDateTime.Day.ToString().PadLeft(2, '0') + @"\" +
                                                                                    rlVideoGuid + ".jpg";
                    }
                    //if (CommonFunctions.GenerateThumbnail(fileLocation, rawMediaoffset, iQCoreRootPath.StoragePath + @"\" + iQAgentTVResults.RL_Date.Year + @"\" +
                    //                                                            iQAgentTVResults.RL_Date.Month.ToString().PadLeft(2, '0') + @"\" + iQAgentTVResults.RL_Date.Day.ToString().PadLeft(2, '0') + @"\" +
                    //                                                            iQAgentTVResults.RL_VideoGUID + ".jpg"))

                    // if(iQCoreRecordFile != null)

                    String remoteSvcURL = rawMediaLogic.GetServiceURLByRootPathID(rootPathID);

                    CLogger.Debug("RemoteSVCURL : " + remoteSvcURL);
                    if (!string.IsNullOrWhiteSpace(remoteSvcURL))
                    {
                        CLogger.Debug("Into Remote Service If Condition");
                        string remoteServiceResponse = rawMediaLogic.RemoteThumbGenService(remoteSvcURL, rawMediaoffset, fileLocation, rlVideoGuid, outPutPathForRemoteService);
                        var remoteGenerateRawMediaThumbOutput = new RemoteGenerateRawMediaThumbOutput();
                        CLogger.Debug("RemoteURL Response : " + remoteServiceResponse);
                        remoteGenerateRawMediaThumbOutput = (RemoteGenerateRawMediaThumbOutput)Serializer.DeserialiazeXml(remoteServiceResponse, remoteGenerateRawMediaThumbOutput);

                        if (remoteGenerateRawMediaThumbOutput != null)
                        {
                            if (remoteGenerateRawMediaThumbOutput.Status == 1)
                            {
                                generateRawMediaThumbOutput.Status = 1;
                                generateRawMediaThumbOutput.Message = remoteGenerateRawMediaThumbOutput.Message;
                            }
                            else
                            {
                                var iQCoreRootPathFromRemote = (IQCoreRootPath)rawMediaLogic.SelectIQCoreRootPathByID(remoteGenerateRawMediaThumbOutput.ID);

                                if (iQCoreRootPathFromRemote != null && !string.IsNullOrWhiteSpace(iQCoreRootPathFromRemote.StreamSuffixPath))
                                {
                                    CLogger.Debug("Get StreamSuffixPath from IQCoreRootPath  By Remote IQRootPathID : " + iQCoreRootPathFromRemote.StreamSuffixPath);
                                    generateRawMediaThumbOutput.Status = 0;
                                    generateRawMediaThumbOutput.Message = "Success";
                                    generateRawMediaThumbOutput.Location = (iQCoreRootPathFromRemote.StreamSuffixPath + @"\" + outPutPathForRemoteService).Replace('\\', '/');
                                }
                                else
                                {
                                    CLogger.Debug("IQCOreRootPath is null or Stream suffix path is null");

                                    generateRawMediaThumbOutput.Status = 1;
                                    generateRawMediaThumbOutput.Message = "Fail - Remote Rooth Path ID doesn't exists";
                                }
                            }
                        }
                        else
                        {
                            generateRawMediaThumbOutput.Status = 1;
                            generateRawMediaThumbOutput.Message = "Fail - Null Response from Remote Service";
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(fileLocation) && !String.IsNullOrWhiteSpace(outPutPath))
                    {
                        if (IQMediaGroup.Common.Util.CommonFunctions.GenerateThumbnail(fileLocation, rawMediaoffset, outputFile, logUID: IQMediaGroup.Services.Util.CommonFunctions.GetUniqueSeqIDForLog()))
                        {
                            if (IQMediaGroup.Common.Util.CommonFunctions.CopyFile(outputFile, outPutPath,logUID:IQMediaGroup.Services.Util.CommonFunctions.GetUniqueSeqIDForLog()))
                            {
                                generateRawMediaThumbOutput.Status = 0;
                                generateRawMediaThumbOutput.Message = "Success";
                                generateRawMediaThumbOutput.Location = (iQCoreRootPath.StreamSuffixPath + @"\" + dtRLVideoDateTime.Year + @"\" +
                                                                                        dtRLVideoDateTime.Month.ToString().PadLeft(2, '0') + @"\" + dtRLVideoDateTime.Day.ToString().PadLeft(2, '0') + @"\" +
                                                                                        rlVideoGuid + ".jpg").Replace('\\', '/');
                                IQMediaGroup.Common.Util.CommonFunctions.DeleteFile(outputFile,logUID:IQMediaGroup.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
                            }
                            else
                            {
                                generateRawMediaThumbOutput.Status = 1;
                                generateRawMediaThumbOutput.Message = "Fail to copy thumbnail to destination Path";
                            }
                        }
                        else
                        {
                            generateRawMediaThumbOutput.Status = 1;
                            generateRawMediaThumbOutput.Message = "Fail to create Thumbnail";
                        }
                    }
                    else
                    {
                        generateRawMediaThumbOutput.Status = 1;
                        generateRawMediaThumbOutput.Message = "Source or Destination path is null";
                    }
                    /*}
                    else
                    {
                        generateRawMediaThumbOutput.Status = 1;
                        generateRawMediaThumbOutput.Message = "Fail - Record file doesn't exists";
                    }*/
                }

            }
            catch (Exception exception)
            {
                generateRawMediaThumbOutput.Status = 1;
                generateRawMediaThumbOutput.Message = "Fail";
                generateRawMediaThumbOutput.Message = exception.Message;
                CLogger.Error("Error : " + exception.Message + "\n Inner Exception" + exception.InnerException + "\n stack : " + exception.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                OutputResult = Serializer.SerializeToXml(generateRawMediaThumbOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                OutputResult = Serializer.Searialize(generateRawMediaThumbOutput);
            }

            CLogger.Debug("Final Result: " + OutputResult);

            CLogger.Debug("GenrateRawMediaThumb Service Ended");
            HttpResponse.Output.Write(OutputResult);
        }
    }
}