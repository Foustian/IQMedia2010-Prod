using System;
using IQMedia.Service.Ingestion.Config.Sections;

namespace IQMedia.Service.Ingestion
{
    public class IngestionTask : IEquatable<IngestionTask>
    {
        public Directory Directory { get; set; }

        public IngestionTask(Directory directory)
        {
            Directory = directory;
        }

        #region IEquatable<IngestionTask> Members

        public bool Equals(IngestionTask other)
        {
            //Two ingestion tasks are equal if the have matching source paths and type filters
            return Directory.SourcePath.Equals(other.Directory.SourcePath)
                   && Directory.TypeFilters.Equals(other.Directory.TypeFilters);
        }

        #endregion
    }
}
