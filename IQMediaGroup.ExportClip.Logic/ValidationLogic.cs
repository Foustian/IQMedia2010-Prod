using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IQMediaGroup.ExportClip.Domain;
using System.Configuration;

namespace IQMediaGroup.ExportClip.Logic
{
    public class ValidationLogic : BaseLogic, ILogic
    {

        public bool ValidateInput(RemoteExportClipInput _RemoteExportClipInput)
        {
            return true;
        }

      
    }
}
