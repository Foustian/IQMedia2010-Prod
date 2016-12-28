using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IQMediaGroup.Logic.CaptureLog
{
    public class ValidationLogic : BaseLogic, ILogic
    {
        public bool ValidateCapturesvcInput(string Source)
        {
            String AllowedIPAddress = ConfigurationManager.AppSettings["IPCaptureServers"];
            string[] ListOFIPAddress = AllowedIPAddress.Split(',');
            bool IsValid = false;
            foreach (string IPAdd in ListOFIPAddress)
            {
                if (IPAdd == Source)
                {
                    IsValid = true;
                    break;
                }
            }

            return IsValid;
        }
    }
}
