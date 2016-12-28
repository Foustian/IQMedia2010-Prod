using System;
using System.Collections.Generic;

namespace IQMedia.Domain
{
    public partial class RecordfileType
    {
        public static readonly RecordfileType INDEX = new RecordfileType(1, "INDEX", "gdx");
        public static readonly RecordfileType FLASH = new RecordfileType(2, "FLASH", "flv");
        public static readonly RecordfileType WINDOWS_MEDIA = new RecordfileType(3, "WMEDIA", "wmv");
        public static readonly RecordfileType TEXT = new RecordfileType(4, "TEXT", "txt");
        public static readonly RecordfileType WINDOWS_MEDIA_AUDIO = new RecordfileType(6, "WMAUDIO", "wma");
        public static readonly RecordfileType WINDOWS_MEDIA_STREAM = new RecordfileType(7, "WMSTREAM", "asf");
        public static readonly RecordfileType WAVE = new RecordfileType(8, "WAVE", "wav");
        public static readonly RecordfileType MP3 = new RecordfileType(9, "MP3", "mp3");
        public static readonly RecordfileType MP4 = new RecordfileType(10, "MP4", "mp4");

        public static readonly List<int> AUDIO_TYPES = new List<int> { WINDOWS_MEDIA_AUDIO.ID, WINDOWS_MEDIA_STREAM.ID, WAVE.ID, MP3.ID };
        public static readonly List<int> VIDEO_TYPES = new List<int> { WINDOWS_MEDIA.ID, FLASH.ID, MP4.ID };
        public static readonly List<int> META_TYPES = new List<int> { TEXT.ID, INDEX.ID };

        private static readonly Dictionary<string, RecordfileType> _typesByExtension = new Dictionary<string, RecordfileType>
            {
                { INDEX.Extension, INDEX},
                { FLASH.Extension, FLASH},
                { WINDOWS_MEDIA.Extension, WINDOWS_MEDIA },
                { TEXT.Extension, TEXT },
                { WINDOWS_MEDIA_AUDIO.Extension, WINDOWS_MEDIA_AUDIO },
                { WINDOWS_MEDIA_STREAM.Extension, WINDOWS_MEDIA_STREAM },
                { WAVE.Extension, WAVE },
                { MP3.Extension, MP3 },
                { MP4.Extension, MP4 }
            };


        private RecordfileType(int typeId, string name, string ext)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            ID = typeId;
            Name = name;
            Extension = ext;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public RecordfileType() { }

        /// <summary>
        /// Determines whether this instance is metadata.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is meta; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMeta()
        {
            return META_TYPES.Contains(this.ID);
        }

        /// <summary>
        /// Determines whether this instance is media.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is media; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMedia()
        {
            //If its not a meta-file (Index or CC Text) then its media
            return !(META_TYPES.Contains(this.ID));
        }

        /// <summary>
        /// Determines whether this instance is video.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is video; otherwise, <c>false</c>.
        /// </returns>
        public bool IsVideo()
        {
            return VIDEO_TYPES.Contains(this.ID);
        }

        /// <summary>
        /// Determines whether this instance is audio.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is audio; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAudio()
        {
            return AUDIO_TYPES.Contains(this.ID);
        }

        /// <summary>
        /// Attempts to get the RecordfileType by its extension.
        /// </summary>
        /// <param name="fileExt">The file extension.</param>
        /// <returns>The relevant RecordfileType</returns>
        public static RecordfileType GetByExtension(string fileExt)
        {
            RecordfileType result;
            if(!_typesByExtension.TryGetValue(fileExt, out result))
                throw new ArgumentException(String.Format("The file extension supplied '{0}' is not supported.", fileExt));

            return result;
        }
    }
}