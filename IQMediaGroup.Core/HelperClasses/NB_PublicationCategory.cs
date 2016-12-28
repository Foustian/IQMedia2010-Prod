using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class NB_PublicationCategory
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
