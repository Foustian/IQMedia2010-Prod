using System;
using System.Configuration;
using System.Linq;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class AssetLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the asset location.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public AssetLocation GetAssetLocation(Guid guid)
        {
            return Context.AssetLocations.SingleOrDefault(al => al.AssetGuid.Equals(guid));
        }

        /// <summary>
        /// Gets the thumbnail UR path based on the clip guid and thumbnail type.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="thumbType">Type of the thumbnail.</param>
        /// <returns>The Url of the clip's thumbnail.</returns>
        public string GetThumbnailUrl(Clip clip, ThumbnailType thumbType)
        {
            //If the clip is null, return the unavailable image.
            return (clip == null) 
                ? ConfigurationManager.AppSettings["ThumbnailUnavailable"] 
                : GetThumbnailUrl(clip.Guid, clip.Recordfile, thumbType);
        }

        /// <summary>
        /// Gets the thumbnail UR path based on the clip guid and thumbnail type.
        /// </summary>
        /// <param name="rFile">The clip.</param>
        /// <param name="thumbType">Type of the thumbnail.</param>
        /// <returns>The Url of the recordfile's thumbnail.</returns>

        public string GetThumbnailUrl(Recordfile rFile, ThumbnailType thumbType)
        {
            //If the clip is null, return the unavailable image.
            return (rFile == null)
                ? ConfigurationManager.AppSettings["ThumbnailUnavailable"]
                : GetThumbnailUrl(rFile.Guid, rFile, thumbType);
        }

        /// <summary>
        /// Private helper method used by the overloads above...
        /// </summary>
        /// <param name="mediaGuid"></param>
        /// <param name="rFile"></param>
        /// <param name="thumbType"></param>
        /// <returns></returns>
        private string GetThumbnailUrl(Guid mediaGuid, Recordfile rFile, ThumbnailType thumbType)
        {
            //In the case that the clip is NOT video...
            if (!rFile.RecordfileType.IsVideo())
            {
                string pathPrefix;
                switch (thumbType)
                {
                    case ThumbnailType.Player:
                        pathPrefix = ConfigurationManager.AppSettings["SourcePlayerLogoUrlPrefix"];
                        break;
                    case ThumbnailType.Small:
                        pathPrefix = ConfigurationManager.AppSettings["SourceSmallLogoUrlPrefix"];
                        break;
                    //Default to LARGE
                    default:
                        pathPrefix = ConfigurationManager.AppSettings["SourceLargeLogoUrlPrefix"];
                        break;
                }
                return pathPrefix + rFile.Recording.Source.Logo;
            }

            var assetLoc = GetAssetLocation(mediaGuid);

            return (assetLoc != null) ? assetLoc.VirtualPath.ToString()
                : ConfigurationManager.AppSettings["ThumbnailUnavailable"];
        }
    }
}

/// <summary>
/// Enumerator for strict thumbnail types...
/// Small (Icon[32x32] For viewing in player drop-down)
/// Large (For Viewing as thumbnail image [Usually 400x300])
/// Player(For viewing as still image in player.)
/// </summary>
public enum ThumbnailType
{
    Small,
    Large,
    Player
}