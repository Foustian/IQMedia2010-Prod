using System;
using IQMedia.Services.SMS.Domain;

namespace IQMedia.Services.SMS.Logic
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaEntities _context;

        protected IQMediaEntities Context
        {
            get { return _context ?? (_context = new IQMediaEntities()); }
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