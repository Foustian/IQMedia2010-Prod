using System;
using System.Web;
using IQMediaGroup.Domain;
using System.IO;
using IQMediaGroup.Logic;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetVideoMetaData : ICommand
    {
        public void Execute(HttpRequest request, HttpResponse response)
        {
            CLogger.Info("GetVideoMetaData input");

            var videoMetaDataOutput = new VideoMetaDataOutput();
            var output = "";
            Guid mediaID;
            string callback = "";
            string type = "";

            try
            {
                try
                {
                    mediaID = Guid.Parse(request["ID"]);
                    type = request["Type"];
                    callback = request["callback"];

                    CLogger.Debug("MediaID: " + mediaID + " Type:" + type);
                }
                catch (CustomException ex)
                {
                    ex.Message = "Error during parsing input.";
                    throw;
                }

                var playerDataLogic = (PlayerDataLogic)LogicFactory.GetLogic(LogicType.PlayerData);
                dynamic result = playerDataLogic.GetPlayerData(mediaID, type, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.TV.ToString(), null, null), BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.QR.ToString(),null,null));

                if (result != null)
                {
                    videoMetaDataOutput.Status = 0;
                    videoMetaDataOutput.Message = "Success";
                    videoMetaDataOutput.VideoMetaData = result;
                }
                else
                {
                    videoMetaDataOutput.Status = -1;
                    videoMetaDataOutput.Message = "An error occurred";
                }

            }
            catch (CustomException ex)
            {
                CLogger.Error(ex);

                videoMetaDataOutput.Status = -2;
                videoMetaDataOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                CLogger.Error(ex);

                videoMetaDataOutput.Status = -1;
                videoMetaDataOutput.Message = "Error";
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            output = callback + "([" + Serializer.JsonSearialize(videoMetaDataOutput) + "])";

            CLogger.Debug("Final result: " + output);

            CLogger.Debug("Get CompeteData SM Request Ended");
            response.Output.Write(output);
        }
    }
}