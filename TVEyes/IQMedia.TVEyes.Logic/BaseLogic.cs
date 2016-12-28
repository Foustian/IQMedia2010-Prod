using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Domain;

namespace IQMedia.TVEyes.Logic
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
