using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using System.Configuration;
using IQMediaGroup.Common.Util;
using System.IO;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class BookmarkService : ICommand
    {
        #region ICommand Members

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {

            string _JSONResult = string.Empty;
            string _JSONRequest = string.Empty;
            try
            {
                CLogger.Debug("Bookmark Request Started");
                BookMarkInput _BookMarkInput = new BookMarkInput();

                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);

                try
                {
                    _JSONRequest = StreamReader.ReadToEnd();
                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_JSONRequest);
                    }
                    _BookMarkInput = (BookMarkInput)Serializer.Deserialize(_JSONRequest, _BookMarkInput.GetType());
                    _BookMarkInput.ClipURL = Convert.ToString(ConfigurationSettings.AppSettings["ClipURL"]);
                    
                    _BookMarkInput.EncryptionKey = Convert.ToString(ConfigurationSettings.AppSettings["EncryptionKey"]);
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);

                /*bool IsValidate = ValidationLogic.ValidateBookmarkInput(_BookMarkInput);*/
                bool IsValidate = ValidateBookmarkInput(_BookMarkInput);

                if (IsValidate)
                {
                    BookmarkServiceLogic _objBookmarkServiceLogic = (BookmarkServiceLogic)LogicFactory.GetLogic(LogicType.BookMarkService);

                    _JSONResult = BookmarkLink(_BookMarkInput);

                    _JSONResult = Serializer.Searialize(_JSONResult);
                }
                else
                {
                    _JSONResult = Serializer.Searialize("Invalid Input");
                }


            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
                _JSONResult = Serializer.Searialize("An error occurred, please try again" + " Error : " + ex.Message + " stack : " + ex.StackTrace);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_JSONResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("Bookmark Request Ended");

            HttpResponse.Output.Write(_JSONResult);

        }

        private string BookmarkLink(BookMarkInput _BookMarkInput)
        {

            try
            {
                 string _BookmarkURL=string.Empty;

                if (!string.IsNullOrEmpty(_BookMarkInput.From))
                {
                    _BookmarkURL = _BookMarkInput.ClipURL + _BookMarkInput.FileID;
                }
                else 
                {
                    _BookmarkURL = _BookMarkInput.ClipURL + _BookMarkInput.FileID;
                }


                return _BookmarkURL;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateBookmarkInput(BookMarkInput p_BookMarkInput)
        {
            try
            {
                /*if (string.IsNullOrEmpty(p_BookMarkInput.From))
                {
                    return false;
                }*/

                if (string.IsNullOrEmpty(p_BookMarkInput.FileID))
                {
                    return false;
                }


                if (string.IsNullOrEmpty(p_BookMarkInput.PageName))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(p_BookMarkInput.ClipURL))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(p_BookMarkInput.EncryptionKey))
                {
                    return false;
                }

                return true;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion
    }
}