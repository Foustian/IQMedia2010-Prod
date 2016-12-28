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
    public partial class Customer
    {
        #region Primitive Properties
    
        public virtual long CustomerKey
        {
            get;
            set;
        }
    
        public virtual string FirstName
        {
            get;
            set;
        }
    
        public virtual string LastName
        {
            get;
            set;
        }
    
        public virtual string Email
        {
            get;
            set;
        }
    
        public virtual string CustomerPassword
        {
            get;
            set;
        }
    
        public virtual string ContactNo
        {
            get;
            set;
        }
    
        public virtual string CustomerComment
        {
            get;
            set;
        }
    
        public virtual long ClientID
        {
            get;
            set;
        }
    
        public virtual System.Guid CustomerGUID
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
    
        public virtual Nullable<System.DateTime> CreatedDate
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> ModifiedDate
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> IsActive
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> MultiLogin
        {
            get;
            set;
        }
    
        public virtual string DefaultPage
        {
            get;
            set;
        }
    
        public virtual string LoginID
        {
            get;
            set;
        }
    
        public virtual Nullable<long> MasterCustomerID
        {
            get;
            set;
        }
    
        public virtual string UDID
        {
            get;
            set;
        }
    
        public virtual Nullable<bool> HasMobileRegd
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> DateMobileRegd
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> PasswordAttempts
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> LastPwdChangedDate
        {
            get;
            set;
        }
    
        public virtual Nullable<byte> RsetPwdEmailCount
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> LastPwdRsetDate
        {
            get;
            set;
        }
    
        public virtual string AnewstipUserID
        {
            get;
            set;
        }

        #endregion
    }
}