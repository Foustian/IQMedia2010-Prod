using System;

namespace IQMedia.Domain
{
    public partial class AssetLocation
    {
        public Uri VirtualPath 
        { 
            get { return new Uri(RootPath.StreamSuffixPath + Location); }
        }
    }
}
