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

namespace IQMediaGroup.Domain
{
    public partial class ClipDownload
    {
        #region Primitive Properties
    
        public virtual long IQ_ClipDownload_Key
        {
            get;
            set;
        }
    
        public virtual System.Guid ClipID
        {
            get;
            set;
        }
    
        public virtual System.Guid CustomerGUID
        {
            get;
            set;
        }
    
        public virtual byte ClipDownloadStatus
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> ClipDLRequestDateTime
        {
            get;
            set;
        }
    
        public virtual string ClipDLFormat
        {
            get;
            set;
        }
    
        public virtual string ClipFileLocation
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> ClipDownLoadedDateTime
        {
            get;
            set;
        }
    
        public virtual string CreatedBy
        {
            get;
            set;
        }
    
        public virtual string ModifiedBy
        {
            get;
            set;
        }
    
        public virtual System.DateTime CreatedDate
        {
            get;
            set;
        }
    
        public virtual System.DateTime ModifiedDate
        {
            get;
            set;
        }
    
        public virtual bool IsActive
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> CCDownloadStatus
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> CCDownloadedDateTime
        {
            get;
            set;
        }

        #endregion
    }
}