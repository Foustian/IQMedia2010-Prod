using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{


    [Serializable]
    [DataContract]
    public class Chart
    {

        [DataMember(Name = "showBorder")]
        public string showBorder { get; set; }

        [DataMember(Name = "bgColor")]
        public string bgColor { get; set; }


        [DataMember(Name = "caption")]
        public string caption { get; set; }

        [DataMember(Name = "subcaption")]
        public string subcaption { get; set; }



        [DataMember(Name = "numVisibleLabels")]
        public string numVisibleLabels { get; set; }


        [DataMember(Name = "exportEnabled")]
        public string exportEnabled { get; set; }

        [DataMember(Name = "exportAtClient")]
        public string exportAtClient { get; set; }

        [DataMember(Name = "exportHandler")]
        public string exportHandler { get; set; }

        [DataMember(Name = "drawAnchors")]
        public string drawAnchors { get; set; }

        [DataMember(Name = "allowpinmode")]
        public string allowpinmode { get; set; }

        [DataMember(Name = "yaxisname")]
        public string yaxisname { get; set; }

        [DataMember(Name = "chartRightMargin")]
        public string chartRightMargin { get; set; }

    }


    [Serializable]
    [DataContract]
    public class FCZoomChart
    {
        public FCZoomChart()
        {

        }
        [DataMember(Name = "chart")]
        public Chart Chart { get; set; }

        [DataMember(Name = "categories")]
        public List<Category> Categories { get; set; }

        [DataMember(Name = "dataset")]
        public List<Dataset> Dataset { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Dataset
    {

        [DataMember(Name = "seriesname")]
        public string Seriesname { get; set; }

        [DataMember(Name = "data")]
        public List<Datum> Data { get; set; }
    }
    [Serializable]
    [DataContract]
    public class Category
    {

        [DataMember(Name = "category")]
        public List<Category2> Category1 { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Category2 : IEqualityComparer<Category2>
    {

        [DataMember(Name = "label")]
        public string Label { get; set; }

        public bool Equals(Category2 x, Category2 y)
        {
            return x.Label.Equals(y.Label);
        }

        public int GetHashCode(Category2 obj)
        {
            return obj.Label.GetHashCode();
        }


    }

    [Serializable]
    [DataContract]
    public class Datum
    {

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

}