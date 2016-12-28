using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;

namespace IQMediaGroup.Logic
{
    public class TrackingLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Inserts an impression into the tracking persistence.
        /// </summary>
        /// <param name="assetGuid">The Asset (usually clip) GUID.</param>
        /// <param name="userGuid">The User GUID.</param>
        /// <param name="ipAddress">The IP Address of the user.</param>
        public void InsertImpression(Guid assetGuid, Guid userGuid, string ipAddress)
        {
            var impSum = Context.RawMediaImpressionSummaries.SingleOrDefault(i => i.C_AssetGuid.Equals(assetGuid));

            //If the ImpressionSummary doesn't exist, create it...
            if (impSum == null)
            {
                impSum = new RawMediaImpressionSummary()
                {
                    C_AssetGuid = assetGuid,
                    Count = 0,
                    LastUpdated = DateTime.Now
                };
                Context.RawMediaImpressionSummaries.AddObject(impSum);
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
        public long InsertPlay(Guid assetGuid, Guid userGuid, string ipAddress, string referrer,string device,string os)
        {
            //First, Update (or add) the PlaySummary
            var playSum = Context.RawMediaPlaySummaries.SingleOrDefault(p => p.C_AssetGuid.Equals(assetGuid));
            //If the PlaySummary doesn't exist, create it...
            if (playSum == null)
            {
                playSum = new RawMediaPlaySummary()
                {
                    C_AssetGuid = assetGuid,
                    Count = 0,
                    LastUpdated = DateTime.Now
                };
                Context.RawMediaPlaySummaries.AddObject(playSum);
            }
            playSum.Count++;
            playSum.LastUpdated = DateTime.Now;

            byte[] bipadd = System.Net.IPAddress.Parse(ipAddress).GetAddressBytes();
            uint ipv4Int = (uint)bipadd[3] + ((uint)bipadd[2] * 256) + ((uint)bipadd[1] * 65536) + ((uint)bipadd[0] * 16777216);

            //Next, Insert a record into the PlayLog
            var playLog = new RawMediaPlayLog()
            {
                C_AssetGuid = assetGuid,
                C_UserGuid = (userGuid.Equals(Guid.Empty)) ? (Guid?)null : userGuid,
                IPAddress = ipAddress,
                IPAddDecimal=ipv4Int,
                PlayDate = DateTime.Now,
                Referrer = referrer,
                Device = device,
                DeviceOS = os
            };
            Context.RawMediaPlayLogs.AddObject(playLog);

            //Finally, Save all changes to the persistence
            Context.SaveChanges();

            return playLog.ID;
        }

        /// <summary>
        /// Updates the percentage played in an existing play log entry.
        /// </summary>
        /// <param name="id">Row ID of the log entry</param>
        /// <param name="secondsPlayed">Number of seconds of the video that have been played</param>
        public int? UpdateLog(long id, short secondsPlayed)
        {
            return Context.UpdateRawMediaSecondsPlayed(id, secondsPlayed).First();
        }
    }
}
