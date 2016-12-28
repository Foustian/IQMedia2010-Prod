using System;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaGroupEntities _context;

        protected IQMediaGroupEntities Context 
        {
            get { return _context ?? (_context = new IQMediaGroupEntities()); }
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
