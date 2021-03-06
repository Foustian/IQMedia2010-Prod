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
    public partial class Source
    {
        #region Primitive Properties
    
        public virtual System.Guid Guid
        {
            get;
            set;
        }
    
        public virtual string SourceID
        {
            get;
            set;
        }
    
        public virtual string Title
        {
            get;
            set;
        }
    
        public virtual string Logo
        {
            get;
            set;
        }
    
        public virtual string Url
        {
            get;
            set;
        }
    
        public virtual string BroadcastType
        {
            get;
            set;
        }
    
        public virtual string BroadcastLocation
        {
            get;
            set;
        }
    
        public virtual int RetentionDays
        {
            get;
            set;
        }
    
        public virtual bool IsActive
        {
            get;
            set;
        }
    
        public virtual int TimezoneID
        {
            get { return _timezoneID; }
            set
            {
                if (_timezoneID != value)
                {
                    if (Timezone != null && Timezone.ID != value)
                    {
                        Timezone = null;
                    }
                    _timezoneID = value;
                }
            }
        }
        private int _timezoneID;

        #endregion
        #region Navigation Properties
    
        public virtual ICollection<Recording> Recordings
        {
            get
            {
                if (_recordings == null)
                {
                    var newCollection = new FixupCollection<Recording>();
                    newCollection.CollectionChanged += FixupRecordings;
                    _recordings = newCollection;
                }
                return _recordings;
            }
            set
            {
                if (!ReferenceEquals(_recordings, value))
                {
                    var previousValue = _recordings as FixupCollection<Recording>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupRecordings;
                    }
                    _recordings = value;
                    var newValue = value as FixupCollection<Recording>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupRecordings;
                    }
                }
            }
        }
        private ICollection<Recording> _recordings;
    
        public virtual Timezone Timezone
        {
            get { return _timezone; }
            set
            {
                if (!ReferenceEquals(_timezone, value))
                {
                    var previousValue = _timezone;
                    _timezone = value;
                    FixupTimezone(previousValue);
                }
            }
        }
        private Timezone _timezone;

        #endregion
        #region Association Fixup
    
        private void FixupTimezone(Timezone previousValue)
        {
            if (previousValue != null && previousValue.Sources.Contains(this))
            {
                previousValue.Sources.Remove(this);
            }
    
            if (Timezone != null)
            {
                if (!Timezone.Sources.Contains(this))
                {
                    Timezone.Sources.Add(this);
                }
                if (TimezoneID != Timezone.ID)
                {
                    TimezoneID = Timezone.ID;
                }
            }
        }
    
        private void FixupRecordings(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Recording item in e.NewItems)
                {
                    item.Source = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Recording item in e.OldItems)
                {
                    if (ReferenceEquals(item.Source, this))
                    {
                        item.Source = null;
                    }
                }
            }
        }

        #endregion
    }
}
