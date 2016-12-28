//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IQMedia.Domain
{
    public partial class Recordfile
    {
        #region Primitive Properties
    
        public virtual System.Guid Guid
        {
            get;
            set;
        }
    
        public virtual string Location
        {
            get;
            set;
        }
    
        public virtual string Status
        {
            get;
            set;
        }
    
        public virtual int StartOffset
        {
            get;
            set;
        }
    
        public virtual int EndOffset
        {
            get;
            set;
        }
    
        public virtual System.DateTime LastModified
        {
            get;
            set;
        }
    
        public virtual int RecordingID
        {
            get { return _recordingID; }
            set
            {
                if (_recordingID != value)
                {
                    if (Recording != null && Recording.ID != value)
                    {
                        Recording = null;
                    }
                    _recordingID = value;
                }
            }
        }
        private int _recordingID;
    
        public virtual int RecordfileTypeID
        {
            get { return _recordfileTypeID; }
            set
            {
                if (_recordfileTypeID != value)
                {
                    if (RecordfileType != null && RecordfileType.ID != value)
                    {
                        RecordfileType = null;
                    }
                    _recordfileTypeID = value;
                }
            }
        }
        private int _recordfileTypeID;
    
        public virtual int RootPathID
        {
            get { return _rootPathID; }
            set
            {
                if (_rootPathID != value)
                {
                    if (RootPath != null && RootPath.ID != value)
                    {
                        RootPath = null;
                    }
                    _rootPathID = value;
                }
            }
        }
        private int _rootPathID;
    
        public virtual Nullable<System.Guid> ParentGuid
        {
            get;
            set;
        }
    
        public virtual System.DateTime DateCreated
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
        public virtual ICollection<Clip> Clips
        {
            get
            {
                if (_clips == null)
                {
                    var newCollection = new FixupCollection<Clip>();
                    newCollection.CollectionChanged += FixupClips;
                    _clips = newCollection;
                }
                return _clips;
            }
            set
            {
                if (!ReferenceEquals(_clips, value))
                {
                    var previousValue = _clips as FixupCollection<Clip>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupClips;
                    }
                    _clips = value;
                    var newValue = value as FixupCollection<Clip>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupClips;
                    }
                }
            }
        }
        private ICollection<Clip> _clips;
    
        public virtual RecordfileType RecordfileType
        {
            get { return _recordfileType; }
            set
            {
                if (!ReferenceEquals(_recordfileType, value))
                {
                    var previousValue = _recordfileType;
                    _recordfileType = value;
                    FixupRecordfileType(previousValue);
                }
            }
        }
        private RecordfileType _recordfileType;
    
        public virtual Recording Recording
        {
            get { return _recording; }
            set
            {
                if (!ReferenceEquals(_recording, value))
                {
                    var previousValue = _recording;
                    _recording = value;
                    FixupRecording(previousValue);
                }
            }
        }
        private Recording _recording;
    
        public virtual RootPath RootPath
        {
            get { return _rootPath; }
            set
            {
                if (!ReferenceEquals(_rootPath, value))
                {
                    var previousValue = _rootPath;
                    _rootPath = value;
                    FixupRootPath(previousValue);
                }
            }
        }
        private RootPath _rootPath;

        #endregion
        #region Association Fixup
    
        private void FixupRecordfileType(RecordfileType previousValue)
        {
            if (previousValue != null && previousValue.Recordfiles.Contains(this))
            {
                previousValue.Recordfiles.Remove(this);
            }
    
            if (RecordfileType != null)
            {
                if (!RecordfileType.Recordfiles.Contains(this))
                {
                    RecordfileType.Recordfiles.Add(this);
                }
                if (RecordfileTypeID != RecordfileType.ID)
                {
                    RecordfileTypeID = RecordfileType.ID;
                }
            }
        }
    
        private void FixupRecording(Recording previousValue)
        {
            if (previousValue != null && previousValue.Recordfiles.Contains(this))
            {
                previousValue.Recordfiles.Remove(this);
            }
    
            if (Recording != null)
            {
                if (!Recording.Recordfiles.Contains(this))
                {
                    Recording.Recordfiles.Add(this);
                }
                if (RecordingID != Recording.ID)
                {
                    RecordingID = Recording.ID;
                }
            }
        }
    
        private void FixupRootPath(RootPath previousValue)
        {
            if (RootPath != null)
            {
                if (RootPathID != RootPath.ID)
                {
                    RootPathID = RootPath.ID;
                }
            }
        }
    
        private void FixupClips(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Clip item in e.NewItems)
                {
                    item.Recordfile = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Clip item in e.OldItems)
                {
                    if (ReferenceEquals(item.Recordfile, this))
                    {
                        item.Recordfile = null;
                    }
                }
            }
        }

        #endregion
    }
}
