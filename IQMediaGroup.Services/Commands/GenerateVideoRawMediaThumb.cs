using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using System.Configuration;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GenerateVideoRawMediaThumb : ICommand
    {
        public Guid? _RawMediaGuid { get; private set; }
        public Int32 _Offset { get; private set; }
        public string _Callback { get; set; }

        public GenerateVideoRawMediaThumb(object RawMediaGuid, object Offset, object callback)
        {
            _Offset = (Offset is NullParameter) ? 0 : (Int32)Offset;
            _RawMediaGuid = (RawMediaGuid is NullParameter) ? null : (Guid?)RawMediaGuid;
            _Callback=(callback is NullParameter)?string.Empty:(string)callback;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {

            CLogger.Debug("GenrateVideoRawMediaThumb Service Started");
            var generateVideoRawMediaThumbOutput = new GenerateVideoRawMediaThumbOutput();
            String OutputResult = string.Empty;

            try
            {

                if (_RawMediaGuid == null)
                {
                    throw new ArgumentException("Invalid or missing RawMediaGuid");
                }

                CLogger.Debug("{\"RawMediaGuid\":\" " + _RawMediaGuid + ",\"Offset\": " + _Offset + "}");

                Guid rlVideoGuid = new Guid();
                DateTime dtRLVideoDateTime = new DateTime();
                int rootPathID = 0;
                string fileLocation = string.Empty;

                var rawMediaLogic = (RawMediaLogic)LogicFactory.GetLogic(LogicType.RawMedia);
                bool isValidID = true;

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

                if (!isValidID)
                {
                    generateVideoRawMediaThumbOutput.Status = 1;
                    generateVideoRawMediaThumbOutput.Message = "Fail - No Result Found for specified ID";
                }
                else
                {
                    /*var iQCoreRecordFile = (IQCoreRecordFile)rawMediaLogic.GetIQCoreRecordFileLocationByGUID(iQAgentTVResults.RL_VideoGUID);
                    if (iQCoreRecordFile != null)
                    {*/
                    Int32 rawMediaoffset = 0;
                    if (_Offset == 0)
                    {
                        rawMediaoffset = Convert.ToInt32(ConfigurationManager.AppSettings["Offset"]);
                        CLogger.Debug("Offset is 0, default offset : " + rawMediaoffset);
                    }
                    else
                    {
                        rawMediaoffset = _Offset;
                    }

                    //get StoragePath and StreamsuffixPath by Config define RootPath ID
                    var iQCoreRootPath = (IQCoreRootPath)rawMediaLogic.SelectIQCoreRootPathByID(Convert.ToInt64(ConfigurationManager.AppSettings["IQRootPathID"]));

                    String outputFile = ConfigurationManager.AppSettings["WorkingDirectory"] + rlVideoGuid + ".jpg";
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
                                generateVideoRawMediaThumbOutput.Status = 1;
                                generateVideoRawMediaThumbOutput.Message = remoteGenerateRawMediaThumbOutput.Message;
                            }
                            else
                            {
                                var iQCoreRootPathFromRemote = (IQCoreRootPath)rawMediaLogic.SelectIQCoreRootPathByID(remoteGenerateRawMediaThumbOutput.ID);

                                if (iQCoreRootPathFromRemote != null && !string.IsNullOrWhiteSpace(iQCoreRootPathFromRemote.StreamSuffixPath))
                                {
                                    CLogger.Debug("Get StreamSuffixPath from IQCoreRootPath  By Remote IQRootPathID : " + iQCoreRootPathFromRemote.StreamSuffixPath);
                                    generateVideoRawMediaThumbOutput.Status = 0;
                                    generateVideoRawMediaThumbOutput.Message = "Success";
                                    generateVideoRawMediaThumbOutput.Location = iQCoreRootPathFromRemote.StreamSuffixPath + @"\" + outPutPathForRemoteService;
                                }
                                else
                                {
                                    CLogger.Debug("IQCOreRootPath is null or Stream suffix path is null");

                                    generateVideoRawMediaThumbOutput.Status = 1;
                                    generateVideoRawMediaThumbOutput.Message = "Fail - Remote Rooth Path ID doesn't exists";
                                }
                            }
                        }
                        else
                        {
                            generateVideoRawMediaThumbOutput.Status = 1;
                            generateVideoRawMediaThumbOutput.Message = "Fail - Null Response from Remote Service";
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(fileLocation) && !String.IsNullOrWhiteSpace(outPutPath))
                    {
                        if (IQMediaGroup.Common.Util.CommonFunctions.GenerateThumbnail(fileLocation, rawMediaoffset, outputFile, logUID: IQMediaGroup.Services.Util.CommonFunctions.GetUniqueSeqIDForLog()))
                        {
                            CLogger.Info("local file path :" + outputFile + "");
                            CLogger.Info("destination path : "+ outPutPath);
                            CLogger.Info("going to copy local file to destination path");
                            
                            if (IQMediaGroup.Common.Util.CommonFunctions.CopyFile(outputFile, outPutPath,logUID:IQMediaGroup.Services.Util.CommonFunctions.GetUniqueSeqIDForLog()))
                            {
                                generateVideoRawMediaThumbOutput.Status = 0;
                                generateVideoRawMediaThumbOutput.Message = "Success";
                                generateVideoRawMediaThumbOutput.Location = iQCoreRootPath.StreamSuffixPath + @"\" + dtRLVideoDateTime.Year + @"\" +
                                                                                        dtRLVideoDateTime.Month.ToString().PadLeft(2, '0') + @"\" + dtRLVideoDateTime.Day.ToString().PadLeft(2, '0') + @"\" +
                                                                                        rlVideoGuid + ".jpg";
                                IQMediaGroup.Common.Util.CommonFunctions.DeleteFile(outputFile, logUID: IQMediaGroup.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
                            }
                            else
                            {
                                generateVideoRawMediaThumbOutput.Status = 1;
                                generateVideoRawMediaThumbOutput.Message = "Fail to copy thumbnail to destination Path";
                            }
                        }
                        else
                        {
                            generateVideoRawMediaThumbOutput.Status = 1;
                            generateVideoRawMediaThumbOutput.Message = "Fail to create Thumbnail";
                        }
                    }
                    else
                    {
                        generateVideoRawMediaThumbOutput.Status = 1;
                        generateVideoRawMediaThumbOutput.Message = "Source or Destination path is null";
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
                generateVideoRawMediaThumbOutput.Status = 1;
                generateVideoRawMediaThumbOutput.Message = "Fail";
                generateVideoRawMediaThumbOutput.Message = exception.Message;
                CLogger.Error("Error : " + exception.Message + "\n Inner Exception" + exception.InnerException + "\n stack : " + exception.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            HttpResponse.ContentType = "application/json";
            OutputResult = Serializer.Searialize(generateVideoRawMediaThumbOutput);

            OutputResult = _Callback + "([" + OutputResult + "])";

            CLogger.Debug("Final Result: " + OutputResult);

            CLogger.Debug("GenrateVideoRawMediaThumb Service Ended");
            HttpResponse.Output.Write(OutputResult);
        }
    }
}