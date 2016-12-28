using System.Xml.Serialization;
namespace IQMediaGroup.ExposeApi.Domain
{
    public class UpdatedTVResult:TVResult
    {
        public long USeqID
        {
            get;
            set;
        }
    }
}