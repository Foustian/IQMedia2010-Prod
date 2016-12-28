using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{

     [XmlType(TypeName = "ProgramCategory")]
     [DataContract(Name = "ProgramCategory")]
    public partial class Class
    {
        #region Primitive Properties

         [DataMember]
        public string Name
        {
            get;
            set;
        }

         [DataMember]
        public string Num
        {
            get;
            set;
        }

        #endregion
    }
}
