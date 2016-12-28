using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    public partial class IQ_Dma
    {
        #region Primitive Properties

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("num")]
        public string Num
        {
            get;
            set;
        }

        #endregion
    }
    public partial class Station_Affil
    {
        #region Primitive Properties

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        #endregion
    }
    public partial class IQ_Class
    {
        #region Primitive Properties

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("num")]
        public string Num
        {
            get;
            set;
        }

        #endregion
    }

    public partial class IQ_Station
    {
        public string IQ_Station_ID { get; set; }
        public string Station_Call_Sign { get; set; }
    }

    public partial class IQ_Region
    {
        #region Primitive Properties

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("num")]
        public int Num
        {
            get;
            set;
        }

        #endregion
    }

    public partial class IQ_Country
    {
        #region Primitive Properties

        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("num")]
        public int Num
        {
            get;
            set;
        }

        #endregion
    }

    public partial class IQ_Zip_Code
    {
        public int ZipCode { get; set; }
        public string IQ_DMA_Name { get; set; }
    }
}
