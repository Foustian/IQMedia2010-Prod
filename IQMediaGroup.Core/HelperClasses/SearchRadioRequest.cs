using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class SearchRadioRequest : IEquatable<SearchRadioRequest>
    {
        public SqlXml IQStationIDXML { get; set; }

        public int PageNumber { get; set; }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        } int _pageSize = 25;

        public string SortExpression { get; set; }
        public bool IsSortDirectionAsc { get; set; }
        public Int64 _TotalRecordsCount { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public bool Equals(SearchRadioRequest other)
        {

            XDocument xDocThis = XDocument.Parse(this.IQStationIDXML.Value);
            XDocument xDocOther = XDocument.Parse(other.IQStationIDXML.Value);

            /*XDocument xDocThis = new XDocument(new XElement("IQ_Station_ID_Set", new XElement("IQ_Station_ID", "sagar"), new XElement("IQ_Station_ID", "joshi"), new XElement("IQ_Station_ID", "joshi1")));
            XDocument xDocOther = new XDocument(new XElement("IQ_Station_ID_Set", new XElement("IQ_Station_ID", "joshi"), new XElement("IQ_Station_ID", "sagar")));*/

            List<String> lstStringThis = (from c in xDocThis.Descendants("IQ_Station_ID_Set").Descendants("IQ_Station_ID") select c.Value).ToList();
            List<String> lstStringOther = (from c in xDocOther.Descendants("IQ_Station_ID_Set").Descendants("IQ_Station_ID") select c.Value).ToList();


            IEnumerable<string> differenceThis = lstStringThis.Except(lstStringOther);
            IEnumerable<string> differenceOther = lstStringOther.Except(lstStringThis);


            if (differenceThis.Any() || differenceOther.Any())
                return false;

            /*if (this.IQStationIDXML != other.IQStationIDXML)
                return false;*/

            if (this.FromDate != other.FromDate)
                return false;

            if (this.ToDate != other.ToDate)
                return false;

            return true;
        }



    }
}
