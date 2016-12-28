using System.Xml.Serialization;
using System;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentResultsUpdateInput
    {
        public string SessionID { get; set; }       

        public int? Rows { get; set; }

        public Int64? USeqID { get; set; }

        [XmlIgnore]
        public bool? IsBoolFetchFullCCData { get; set; }


        public string IsFetchFullCCData
        {
            get { return IsBoolFetchFullCCData.HasValue ? IsBoolFetchFullCCData.ToString() : null; }
            set
            {
                bool temp;
                if (Boolean.TryParse(value, out temp))
                {
                    IsBoolFetchFullCCData = temp;
                }
                else
                {
                    throw new Exception();
                }

            }
        }
    }
}