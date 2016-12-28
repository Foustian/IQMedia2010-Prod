using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.IO;
using System.Configuration;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class InsertClipTimeSync : ICommand
    {
        public String _Format { get; private set; }

        public InsertClipTimeSync(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _Input = string.Empty;
            string _Output = string.Empty;
            var insertClipTimeSyncOutput = new InsertClipTimeSyncOutput();
            CLogger.Debug("InsertClipTimeSync Request Started");



            try
            {
                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);
                InsertClipTimeSyncInput insertClipTimeSyncInput = new InsertClipTimeSyncInput();
                try
                {
                    _Input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_Input);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        insertClipTimeSyncInput = (InsertClipTimeSyncInput)Serializer.DeserialiazeXml(_Input, insertClipTimeSyncInput);
                    }
                    else
                    {
                        insertClipTimeSyncInput = (InsertClipTimeSyncInput)Serializer.Deserialize(_Input, insertClipTimeSyncInput.GetType());
                    }
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!insertClipTimeSyncInput.ClipGuid.HasValue)
                    throw new CustomException("Invalid or missing Clip Guid");

                if (!insertClipTimeSyncInput.StartOffset.HasValue)
                    throw new CustomException("Invalid or missing Start Offset");

                if (!insertClipTimeSyncInput.EndOffset.HasValue)
                    throw new CustomException("Invalid or missing End Offset");

                TimeSyncLogic timeSyncLogic = (TimeSyncLogic)LogicFactory.GetLogic(LogicType.TimeSync);

                insertClipTimeSyncOutput = timeSyncLogic.InsertClipTimeSync(insertClipTimeSyncInput);
               
            }
            catch (CustomException _Exception)
            {
                insertClipTimeSyncOutput.Message = _Exception.Message;
                insertClipTimeSyncOutput.Status = -2;
                CLogger.Debug(_Exception.ToString());
            }
            catch (Exception _Exception)
            {
                insertClipTimeSyncOutput.Status = -1;
                insertClipTimeSyncOutput.Message = "An error occured, please try again!!";
                CLogger.Error(_Exception.ToString());
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(insertClipTimeSyncOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.Searialize(insertClipTimeSyncOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + _Output);
            }
            CLogger.Debug("InsertClipTimeSync Request Ended");
            HttpResponse.Output.Write(_Output);
            

        }
    }
}