using System;

namespace IQMedia.Model
{
    public class SonyTableModel
    {
        public int RowID { get; set; }
        public long ArtistID { get; set; }
        public string Artist { get; set; }
        public long AlbumID { get; set; }
        public string Album { get; set; }
        public long TrackID { get; set; }
        public string Track { get; set; }
        public string SourceType { get; set; }
        public long TotalCount { get; set; }
        public long SpotifyCount { get; set; }
        public long ITunesDownloadCount { get; set; }
        public long ITunesStreamingCount { get; set; }
    }
}
