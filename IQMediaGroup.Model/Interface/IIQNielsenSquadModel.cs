using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;

namespace IQMediaGroup.Model.Interface
{
    public interface IIQNielsenSquadModel
    {
        DataSet GetNielsenData(string iqCCKey);
        DataSet GetNielsenDataByXML(XDocument xmldata, Guid clientGuid);
    }
}
