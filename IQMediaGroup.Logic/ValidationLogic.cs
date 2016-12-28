using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Configuration;

namespace IQMediaGroup.Logic
{
    public class ValidationLogic : BaseLogic, ILogic
    {
        private static string[] RawMediaSortField = { "datetime", "datetime-", "guid", "guid-", "station", "station-", "market", "market-", "title120", "title120-" };
        private static string[] RadioRawMediaSortField = { "datetime", "datetime-", "guid", "guid-", "station", "station-", "market", "market-" };
        private static string[] RawMediaTimeZone = { "all", "cst", "mst", "pst", "est" };



        public bool ValidateBookmarkInput(BookMarkInput p_BookMarkInput)
        {
            try
            {
                if (string.IsNullOrEmpty(p_BookMarkInput.From))
                {
                    return false;
                }

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

        public bool ValidateEmailInput(EmaiInput p_EmaiInput)
        {
            try
            {
                /* Start SessionID Validation */

                if (string.IsNullOrEmpty(p_EmaiInput.From))
                {
                    return false;
                }


                if (string.IsNullOrEmpty(p_EmaiInput.To))
                {
                    return false;
                }


                //if (string.IsNullOrEmpty(p_EmaiInput.Body))
                //{
                //    return false;
                //}


                //if (string.IsNullOrEmpty(p_EmaiInput.FileName))
                //{
                //    return false;
                //}

                //if (string.IsNullOrEmpty(p_EmaiInput._imagePath))
                //{
                //    return false;
                //}

                if (string.IsNullOrEmpty(p_EmaiInput.FileID))
                {
                    return false;
                }


              /*if (string.IsNullOrEmpty(p_EmaiInput.PageName))
                {
                    return false;
                }
                */
                
                /* Stop SessionID Validation */




                return true;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool ValidateNielSenInput(NielSenDataInput p_NielSenInput)
        {
            if (p_NielSenInput.Guid == null)
            {
                return false;
            }
            /*
            if (p_NielSenInput.ClientGuid == null)
            {
                return false;
            }
            */
            if (p_NielSenInput.IsRawMedia == null)
            {
                return false;
            }

            if (p_NielSenInput.IsRawMedia == false)
            {
                if(p_NielSenInput.IQ_Start_Point == null)
                    return false;
            }
            return true;
        }

        public bool ValidateNielSenInput(VideoNielSenDataInput p_VideoNielSenDataInput)
        {
            if (p_VideoNielSenDataInput.Guid == null)
            {
                return false;
            }

            if (p_VideoNielSenDataInput.ClientGuid == null)
            {
                return false;
            }

            if (p_VideoNielSenDataInput.IsRawMedia == null)
            {
                return false;
            }

            if (p_VideoNielSenDataInput.IsRawMedia == false)
            {
                if (p_VideoNielSenDataInput.IQ_Start_Point == null)
                    return false;
            }
            return true;
        }
    }
}
