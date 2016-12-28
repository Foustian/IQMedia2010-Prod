using System.Linq;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class SourceLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the source by source ID.
        /// </summary>
        /// <param name="sourceID">The source ID.</param>
        /// <returns>A source.</returns>
        public Source GetSourceBySourceID(string sourceID)
        {
            return Context.Sources.FirstOrDefault(s => s.SourceID == sourceID);
        }
    }
}
