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
    public partial class PlayLog
    {
        #region Primitive Properties
    
        public virtual int ID
        {
            get;
            set;
        }
    
        public virtual System.Guid AssetGuid
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
    
        public virtual Nullable<System.Guid> UserGuid
        {
            get;
            set;
        }

        #endregion
    }
}