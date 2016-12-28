using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class BookMarkInput
    {
        public string From { get; set; }
        public string FileID { get; set; }
        public string PageName { get; set; }
        public string ClipURL { get; set; }
        public string EncryptionKey { get; set; }
    }
}
