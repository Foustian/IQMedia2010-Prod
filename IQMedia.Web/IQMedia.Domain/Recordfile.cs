using System;

namespace IQMedia.Domain
{
    public partial class Recordfile
    {
        private Uri _physicalPath;
        private Uri _virtualPath;

        public Uri PhysicalPath
        {
            get { return _physicalPath ?? (_physicalPath = new Uri(this.RootPath.StoragePath + this.Location)); }
        }

        public Uri VirtualPath
        {
            get { return _virtualPath ?? (_virtualPath = new Uri(this.RootPath.StreamSuffixPath + this.Location)); }
        }
    }
}
