using System;
using IQMediaGroup.ExportClip.Domain;


namespace IQMediaGroup.ExportClip.Logic
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaGroupRemoteEntities _context;

        protected IQMediaGroupRemoteEntities Context
        {
            get { return _context ?? (_context = new IQMediaGroupRemoteEntities()); }
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
