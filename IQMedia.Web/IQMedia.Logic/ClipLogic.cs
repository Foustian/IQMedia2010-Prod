using System;
using System.Collections;
using System.Linq;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class ClipLogic : BaseLogic, ILogic
    {
        //TODO: Move Categories to the Persistence
        private static readonly Hashtable _categories = new Hashtable
            {
                { "FI", "Entertainment" },
                { "FN", "Finance" },
                { "NE", "News" },
                { "PU", "Politics" },
                { "SR", "Sports" }
            };

        /// <summary>
        /// Gets the category description based on the pre-determined category key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetCategoryDescription(string key)
        {
            //NOTE: This is stupid... (Again categories should be in the database...)
            if (key.Equals("DEL")) return "DELETED";
            if (key.Equals("PR")) return "Private";
            return (string)_categories[key];
        }

        /// <summary>
        /// Gets the partner link URL.
        /// </summary>
        /// <param name="partnerGuid">The partner GUID.</param>
        /// <returns></returns>
        public string GetPartnerLinkUrl(Guid partnerGuid)
        {
            //TODO: STUB Function - Implement Logic
            return null;
        }

        /// <summary>
        /// Gets the clip.
        /// </summary>
        /// <param name="guid">The Clip GUID.</param>
        /// <returns></returns>
        public Clip GetClip(Guid guid)
        {
            return Context.Clips.SingleOrDefault(c => c.Guid.Equals(guid));
        }

        /// <summary>
        /// Inserts the clip into the persistence.
        /// </summary>
        /// <param name="clip">The clip to be inserted.</param>
        public void Insert(Clip clip)
        {
            Context.Clips.AddObject(clip);
            Context.SaveChanges();
        }
    }
}
