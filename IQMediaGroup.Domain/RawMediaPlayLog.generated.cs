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
    public partial class RawMediaPlayLog
    {
        #region Primitive Properties
    
        public virtual long ID
        {
            get;
            set;
        }
    
        public virtual System.Guid C_AssetGuid
        {
            get;
            set;
        }
    
        public virtual System.DateTime PlayDate
        {
            get;
            set;
        }
    
        public virtual string IPAddress
        {
            get;
            set;
        }
    
        public virtual string Referrer
        {
            get;
            set;
        }
    
        public virtual Nullable<System.Guid> C_UserGuid
        {
            get;
            set;
        }
    
        public virtual string Device
        {
            get;
            set;
        }
    
        public virtual string DeviceOS
        {
            get;
            set;
        }
    
        public virtual Nullable<long> IPAddDecimal
        {
            get;
            set;
        }
    
        public virtual Nullable<short> SecondsPlayed
        {
            get;
            set;
        }

        #endregion
    }
}
