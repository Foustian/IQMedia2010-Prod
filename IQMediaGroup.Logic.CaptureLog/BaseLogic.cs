using System;
using IQMediaGroup.Domain.CaptureLog;

namespace IQMediaGroup.Logic.CaptureLog
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaCaptureLogEntities _context;

        protected IQMediaCaptureLogEntities Context
        {
            get { return _context ?? (_context = new IQMediaCaptureLogEntities()); }
        }

        /// <summary>
        /// Persists all context changes to the data store.
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
