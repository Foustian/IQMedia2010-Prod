using System;
using IQMediaGroup.CoreServices.Domain;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Logic
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaGroupEntities _context;

        protected IQMediaGroupEntities Context
        {
            get 
            { 
                if(_context!=null)
                {
                return _context;
                }
                else
                {
                    _context = new IQMediaGroupEntities();
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
