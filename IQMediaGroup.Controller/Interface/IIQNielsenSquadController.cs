using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Xml.Linq;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQNielsenSquadController
    {
        List<RawMedia> GetNielsenData(string iqCCKey, List<RawMedia> _listRawMedia);
        List<RawMedia> GetNielsenDataByXML(XDocument xmldata, List<RawMedia> _listRawMedia, Guid clientGuid);
    }
}
