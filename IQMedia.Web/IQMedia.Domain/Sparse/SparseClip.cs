using System;
using System.Collections.Generic;

namespace IQMedia.Domain.Sparse
{
    public class SparseClip
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string CategoryKey { get; set; }
        public string Description { get; set; }
        public IEnumerable<SparseClipMeta> ClipMeta { get; set; }
    }
}
