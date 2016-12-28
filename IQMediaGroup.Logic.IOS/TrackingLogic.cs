using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Logic.IOS
{
    public class TrackingLogic : BaseLogic, ILogic
    {
        public void UpdateLog(long id, short percentPlayed)
        {
            var playLog = Context.RawMediaPlayLogs.Single(p => p.ID == id);
            playLog.PercentPlayed = percentPlayed;
            Context.SaveChanges();
        }
    }
}
