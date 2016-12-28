using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Logic
{
    public class ClientLogic : BaseLogic, ILogic
    {
        public bool HasMicrositeAccess(Guid ClientGUID)
        {
            try
            {
                var HasAccess = Context.CheckForMicrositeAccessByClientGUID(ClientGUID);

                return HasAccess.Single().Value;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        public Guid? GetClientGUIDByClipGUID(Guid ClipGUID)
        {
            var clip = Context.ArchiveClips.Where(c => c.ClipID == ClipGUID).SingleOrDefault();

            if (clip!=null)
            {
                return clip.ClientGUID;
            }

            return null;
        }
    }    
}
