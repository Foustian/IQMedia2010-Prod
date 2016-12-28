using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.IO;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Serializers;
using IQMediaGroup.CoreServices.Logic;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class UpdateRecordFile : ICommand
    {

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            
            string _RecordFileOutput = string.Empty;

            Logger.LogInfo("Update Record File Request Started");
            RecordFileUpdateOutput _RecordFileUpdateOutput = new RecordFileUpdateOutput();
            try
            {

                StreamReader _StreamReader = new StreamReader(HttpRequest.InputStream);
                RecordFileUpdate _RecordFileUpdate = new RecordFileUpdate();
                
                try
                {
                    string _RecordFileInput = _StreamReader.ReadToEnd();
                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Logger.LogInfo("RecordFileUpdate Input : "+_RecordFileInput);
                    }
                    _RecordFileUpdate = (RecordFileUpdate)Serializer.DeserialiazeXml(_RecordFileInput, _RecordFileUpdate);
                }
                catch (Exception ex)
                {
                    throw new CustomException("Error during parsing input");
                }

                ValidationLogic _ValidationLogic = new ValidationLogic();
                bool IsValidate = _ValidationLogic.ValidateUpdateFileInput(_RecordFileUpdate);

                if (!IsValidate)
                {
                    
                    _RecordFileUpdateOutput.Message = "Invalid Input";
                    _RecordFileUpdateOutput.Status = 1;
                    _RecordFileOutput = Serializer.SerializeToXml(_RecordFileUpdateOutput);
                }
                else
                {

                    RecordFileLogic _RecordFileLogic = new RecordFileLogic();
                    Int32 Result = _RecordFileLogic.UpdateRecordFile(_RecordFileUpdate);

                    if (Result > 0)
                    {
                        _RecordFileUpdateOutput.Message = "Record File Updated Successfully.";
                        _RecordFileUpdateOutput.Status = 0;
                        _RecordFileOutput = Serializer.SerializeToXml(_RecordFileUpdateOutput);
                    }
                    else
                    {

                        _RecordFileUpdateOutput.Message = "No Data Updated";
                        _RecordFileUpdateOutput.Status = 1;
                        _RecordFileOutput = Serializer.SerializeToXml(_RecordFileUpdateOutput);
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                _RecordFileUpdateOutput.Message = _CustomException.Message;
                _RecordFileUpdateOutput.Status = 1;
                _RecordFileOutput = Serializer.SerializeToXml(_RecordFileUpdateOutput);
            }
            catch (Exception _ex)
            {
                Logger.LogInfo("Error : " + _ex.Message + " stack : " + _ex.StackTrace);
                _RecordFileUpdateOutput.Message = "An error occurred, please try again."+_ex.Message+" inner Exception="+_ex.InnerException;
                _RecordFileUpdateOutput.Status = 1;
                _RecordFileOutput = Serializer.SerializeToXml(_RecordFileUpdateOutput);

            }
           
            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("RecordFileUpdate Output : " + _RecordFileOutput);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Logger.LogInfo("Update Record File Request Ended");
            HttpResponse.Output.Write(_RecordFileOutput);
        }
    }
}