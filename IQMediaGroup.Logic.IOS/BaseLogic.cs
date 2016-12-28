using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain.IOS;


namespace IQMediaGroup.Logic.IOS
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaGroupIOSEntities _context;

        protected IQMediaGroupIOSEntities Context
        {
            get { return _context ?? (_context = new IQMediaGroupIOSEntities()); }
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
