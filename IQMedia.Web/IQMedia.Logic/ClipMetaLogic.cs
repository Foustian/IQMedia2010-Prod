using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class ClipMetaLogic : BaseLogic, ILogic
    {
        //NOTE: This code has NOT been tested and probably does't work...
        public void Update(IEnumerable<ClipMeta> clipMeta)
        {
            foreach(var meta in clipMeta)
            {
                var stub = new ClipMeta {ClipGuid = meta.ClipGuid, Field = meta.Field};
                Context.ClipMeta.DeleteObject(stub);
                
                Context.ClipMeta.AddObject(meta);
            }
            Context.SaveChanges();
        }
    }
}
