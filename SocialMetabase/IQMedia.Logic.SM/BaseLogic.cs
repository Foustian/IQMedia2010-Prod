using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Domain.SM;


namespace IQMedia.Logic.SM
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
        /// Refreshes the context by disposing of it and setting it to null for the garbage
        /// collector.
        /// </summary>
        public static void RefreshContext()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
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
