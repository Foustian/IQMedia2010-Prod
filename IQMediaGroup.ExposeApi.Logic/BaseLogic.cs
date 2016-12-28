using System;
using IQMediaGroup.ExposeApi.Domain;
using System.Configuration;
using System.Collections.Generic;

namespace IQMediaGroup.ExposeApi.Logic
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaGroupExposeEntities _context;

        protected IQMediaGroupExposeEntities Context
        {
            get
            {
                if (_context != null)
                {
                    return _context;
                }
                else
                {
                    _context = new IQMediaGroupExposeEntities();
                    _context.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCommandTimeout"]);
                    return _context;
                }
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
