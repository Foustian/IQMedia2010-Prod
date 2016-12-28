using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentResultsInput
    {
        public string SessionID { get; set; }

        public Int64? SRID { get; set; }

        public Int64? SeqID { get; set; }

        public int? Rows { get; set; }

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
