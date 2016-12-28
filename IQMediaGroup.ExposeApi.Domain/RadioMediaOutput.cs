using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class RadioMediaOutput
    {
        public string Message { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// Count for All RadioRawMedia found
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Flag for next page is available or not
        /// </summary>
        public bool HasNextPage { get; set; }
        
        /// <summary>
        /// List of RadioRawMedia
        /// </summary>
        public List<RadioMedia> RadioMediaList { get; set; }
                
    }
}
