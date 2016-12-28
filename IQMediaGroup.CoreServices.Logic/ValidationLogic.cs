using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IQMediaGroup.CoreServices.Domain;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Logic
{
    public class ValidationLogic : BaseLogic, ILogic
    {
        public bool ValidateCreateFileInput(RecordFileInput _RecordFileInput)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.SourceGuid))
                    || string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.StartDate))
                    || string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.EndDate))
                    || string.IsNullOrEmpty(_RecordFileInput.DestinationFile)
                    || string.IsNullOrEmpty(_RecordFileInput.Status)
                    || string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.EndOffset))
                    || string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.RecordFileTypeID))
                    || string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.RootPathID))
                    || string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.IsUGC)))
                {
                    return false;
                }

                if ((_RecordFileInput.SourceGuid == Guid.Empty))
                {
                    return false;
                }

                if (_RecordFileInput.EndOffset <= 0)
                {
                    return false;
                }

                if (!Convert.ToBoolean(_RecordFileInput.IsUGC) && (string.IsNullOrEmpty(Convert.ToString(_RecordFileInput.FileExtension))))
                {

                    return false;
                }

                if (Convert.ToBoolean(_RecordFileInput.IsUGC) && _RecordFileInput.UGCMetaData.IngestionData1 == null)
                {
                    return false;
                }

                DateTime _date = new DateTime();

                if (!DateTime.TryParse(Convert.ToString(_RecordFileInput.StartDate), out _date))
                {
                    return false;
                }

                if (!DateTime.TryParse(Convert.ToString(_RecordFileInput.EndDate), out _date))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return true;

        }

        public bool ValidateUpdateFileInput(RecordFileUpdate _RecordFileUpdate)
        {

            try
            {
                if ((string.IsNullOrEmpty(Convert.ToString(_RecordFileUpdate.RecordfileID))) ||
                    (string.IsNullOrEmpty(Convert.ToString(_RecordFileUpdate.EndOffset))) ||
                    (string.IsNullOrEmpty(_RecordFileUpdate.Location)) ||
                    (string.IsNullOrEmpty(Convert.ToString(_RecordFileUpdate.RootPathID))) ||
                    (string.IsNullOrEmpty(_RecordFileUpdate.Status))
                   )
                {
                    return false;
                }

                if (_RecordFileUpdate.RecordfileID == Guid.Empty)
                {
                    return false;
                }

                if ((_RecordFileUpdate.EndOffset <= 0))
                {

                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool ValidateRequestIP(string Source)
        {
            String AllowedIPAddress = ConfigurationManager.AppSettings["ValidIPAddresses"];
            string[] ListOFIPAddress = AllowedIPAddress.Split(',');
            bool IsValid = false;

            IsValid = ListOFIPAddress.Contains(Source);

            return IsValid;
        }

        public bool ValidateIngestionLogInput(IQLog_IngestionInput iqLogIngestion)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(iqLogIngestion.StationID))
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Convert.ToString(iqLogIngestion.MediaType)))
                {
                    return false;
                }
                else
                {
                    if (!ConfigurationManager.AppSettings["AllowedMediaTypes"].Split(',').Contains(iqLogIngestion.MediaType.Trim().ToLower()))
                    {
                        return false;
                    }
                }
                if (string.IsNullOrWhiteSpace(iqLogIngestion.Level))
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(iqLogIngestion.Logger))
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(iqLogIngestion.LogMessage))
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
