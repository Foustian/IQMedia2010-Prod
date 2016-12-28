using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain.IOS
{
    public class MobileLogInOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public bool MobileLogin { get; set; }

        public string UID { get; set; }

        public string FTPDetails { get; set; }

        public bool HasOldVersion { get; set; }

        public string Path { get; set; }

        public Guid? DefaultCategory { get; set; }

        public List<Category> CategoryList { get; set; }

        public bool IsCategoryEnabled { get; set; }

        public int MaxVideoDuration { get; set; }

        public bool ForceLandscape { get; set; }
    }
}
