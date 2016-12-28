using System;
using IQMediaGroup.Domain;
using System.Web;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public abstract class BaseLogic
    {
        [ThreadStatic]
        private static IQMediaGroupISVCEntities _context;

        protected IQMediaGroupISVCEntities Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new IQMediaGroupISVCEntities();
                }

                if (_context.Connection.State != System.Data.ConnectionState.Open)
                {
                    Log4NetLogger.Debug("BaseLogic Connection State: "+_context.Connection.State.ToString());

                    if (_context.Connection.State == System.Data.ConnectionState.Broken)
                    {
                        _context.Connection.Close();
                    }

                    _context.Connection.Open();
                }

                return _context;

                //string ocKey = "ocm_" + HttpContext.Current.GetHashCode().ToString("x");
                //if (!HttpContext.Current.Items.Contains(ocKey))
                //    HttpContext.Current.Items.Add(ocKey, new IQMediaGroupISVCEntities());
                //return HttpContext.Current.Items[ocKey] as IQMediaGroupISVCEntities;
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
