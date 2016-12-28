using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.IO;
using IQMediaGroup.CoreServices.Serializers;
using IQMediaGroup.CoreServices.Domain;
using System.Xml.Linq;
using IQMediaGroup.CoreServices.Logic;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class InsertRecordFile : ICommand
    {

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _RecordFileInputStr = string.Empty;
            string _RecordFileOutputStr = string.Empty;
            RecordFileOutput _RecordFileOutput = new RecordFileOutput();

            Log4NetLogger.Info("Insert RecordFile Request Started.");
            
            try
            {
                StreamReader _StreamReader = new StreamReader(HttpRequest.InputStream);
                var _RecordFileInput = new RecordFileInput();

                try
                {
                    _RecordFileInputStr = _StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        
                        Log4NetLogger.Info("RecordFile Input : " + _RecordFileInputStr);
                    }

                    _RecordFileInput = (RecordFileInput)Serializer.DeserialiazeXml(_RecordFileInputStr, _RecordFileInput);
                }
                catch (Exception)
                {
                    throw new CustomException("An error occurred in parsing input.");
                }


                var _ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                bool isValidate = _ValidationLogic.ValidateCreateFileInput(_RecordFileInput);

                if (!isValidate)
                {
                    _RecordFileOutput.Message = "Invalid Input";
                    _RecordFileOutput.Status = 1;
                    _RecordFileOutputStr = Serializer.SerializeToXml(_RecordFileOutput);
                }
                else
                {
                    var RecordFileLogic = (RecordFileLogic)LogicFactory.GetLogic(LogicType.RecordFile);
                    string UGCXML = (_RecordFileInput.UGCMetaData == null || _RecordFileInput.UGCMetaData.IngestionData1==null) ? string.Empty : Serializer.SerializeToXml(_RecordFileInput.UGCMetaData.IngestionData1);
                    _RecordFileOutput = RecordFileLogic.InsertRecordFile(_RecordFileInput, UGCXML);
                    _RecordFileOutputStr = Serializer.SerializeToXml(_RecordFileOutput);
                }   
            }
            catch (CustomException _CustomException)
            {
                _RecordFileOutput.Message = _CustomException.Message;
                _RecordFileOutput.Status = 1;
                _RecordFileOutputStr = Serializer.SerializeToXml(_RecordFileOutput);
            }
            catch (Exception ex)
            {

                _RecordFileOutput.Message = "An error occurred, please try again.";//ex.Message;
                _RecordFileOutput.Status = 1;
                _RecordFileOutputStr = Serializer.SerializeToXml(_RecordFileOutput);

                Log4NetLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("RecordFile Output : " + _RecordFileOutputStr);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";

            Log4NetLogger.Info("Insert RecordFile Request Completed.");

            HttpResponse.Output.Write(_RecordFileOutputStr);
        }
    }
}