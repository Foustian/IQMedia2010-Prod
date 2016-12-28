using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class RecordingLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the recording by source and date.
        /// </summary>
        /// <param name="sourceGuid">The source GUID.</param>
        /// <param name="startDate">The start date.</param>
        /// <returns>The relevant recording.</returns>
        public Recording GetRecordingBySourceAndDate(Guid sourceGuid, DateTime startDate)
        {
            return
                Context.Recordings.FirstOrDefault(r => r.SourceGuid.Equals(sourceGuid) && r.StartDate.Equals(startDate));
        }

        /// <summary>
        /// Inserts the recording into the persistence.
        /// </summary>
        /// <param name="recording">The recording.</param>
        public void Insert(Recording recording)
        {
            Context.Recordings.AddObject(recording);
            Context.SaveChanges();
        }
    }
}
