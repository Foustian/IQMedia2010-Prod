using System;
using System.Linq;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class TrackingLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the impression count for the Asset.
        /// </summary>
        /// <param name="assetGuid">The Asset GUID.</param>
        /// <returns></returns>
        public int GetImpressionCount(Guid assetGuid)
        {
            var impSum = Context.ImpressionSummary.SingleOrDefault(i => i.AssetGuid.Equals(assetGuid));
            return (impSum == null) ? 0 : impSum.Count;
        }

        /// <summary>
        /// Gets the play count for the Asset.
        /// </summary>
        /// <param name="assetGuid">The Asset GUID.</param>
        /// <returns></returns>
        public int GetPlayCount(Guid assetGuid)
        {
            var playSum = Context.PlaySummary.SingleOrDefault(i => i.AssetGuid.Equals(assetGuid));
            return (playSum == null) ? 0 : playSum.Count;
        }
        
        /// <summary>
        /// Inserts an impression into the tracking persistence.
        /// </summary>
        /// <param name="assetGuid">The Asset (usually clip) GUID.</param>
        /// <param name="userGuid">The User GUID.</param>
        /// <param name="ipAddress">The IP Address of the user.</param>
        public void InsertImpression(Guid assetGuid, Guid userGuid, string ipAddress)
        {
            var impSum = Context.ImpressionSummary.SingleOrDefault(i => i.AssetGuid.Equals(assetGuid));

            //If the ImpressionSummary doesn't exist, create it...
            if (impSum == null)
            {
                impSum = new ImpressionSummary()
                             {
                                 AssetGuid = assetGuid,
                                 Count = 0,
                                 LastUpdated = DateTime.Now
                             };
                Context.ImpressionSummary.AddObject(impSum);
            }

            impSum.Count++;
            impSum.LastUpdated = DateTime.Now;
            Context.SaveChanges();
        }

        /// <summary>
        /// Inserts the play into the tracking persistence.
        /// </summary>
        /// <param name="assetGuid">The Asset GUID.</param>
        /// <param name="userGuid">The User GUID.</param>
        /// <param name="ipAddress">The IP Address of the user.</param>
        /// <param name="referrer">The URL of the referring site.</param>
        public void InsertPlay(Guid assetGuid, Guid userGuid, string ipAddress, string referrer)
        {
            //First, Update (or add) the PlaySummary
            var playSum = Context.PlaySummary.SingleOrDefault(p => p.AssetGuid.Equals(assetGuid));
            //If the PlaySummary doesn't exist, create it...
            if (playSum == null)
            {
                playSum = new PlaySummary()
                {
                    AssetGuid = assetGuid,
                    Count = 0,
                    LastUpdated = DateTime.Now
                };
                Context.PlaySummary.AddObject(playSum);
            }
            playSum.Count++;
            playSum.LastUpdated = DateTime.Now;

            //Next, Insert a record into the PlayLog
            var playLog = new PlayLog()
                              {
                                  AssetGuid = assetGuid,
                                  UserGuid = (userGuid.Equals(Guid.Empty)) ? (Guid?)null : userGuid,
                                  IPAddress = ipAddress,
                                  PlayDate = DateTime.Now,
                                  Referrer = referrer
                              };
            Context.PlayLogs.AddObject(playLog);

            //Finally, Save all changes to the persistence
            Context.SaveChanges();
        }
    }
}
