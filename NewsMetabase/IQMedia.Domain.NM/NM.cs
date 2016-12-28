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

namespace IQMedia.Domain.NM
{
    public partial class NM
    {
        #region Primitive Properties
    
        public virtual string ArticleID
        {
            get;
            set;
        }
    
        public virtual Nullable<int> RootPathID
        {
            get { return _rootPathID; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_rootPathID != value)
                    {
                        if (RootPath != null && RootPath.ID != value)
                        {
                            RootPath = null;
                        }
                        _rootPathID = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private Nullable<int> _rootPathID;
    
        public virtual string Location
        {
            get;
            set;
        }
    
        public virtual string Url
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> harvest_time
        {
            get;
            set;
        }
    
        public virtual string Status
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
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
    
        private bool _settingFK = false;
    
        private void FixupRootPath(RootPath previousValue)
        {
            if (previousValue != null && previousValue.NMs.Contains(this))
            {
                previousValue.NMs.Remove(this);
            }
    
            if (RootPath != null)
            {
                if (!RootPath.NMs.Contains(this))
                {
                    RootPath.NMs.Add(this);
                }
                if (RootPathID != RootPath.ID)
                {
                    RootPathID = RootPath.ID;
                }
            }
            else if (!_settingFK)
            {
                RootPathID = null;
            }
        }

        #endregion
    }
}
