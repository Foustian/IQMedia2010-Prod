﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace IQMedia.TVEyes.Domain
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class IQMediaGroupEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new IQMediaGroupEntities object using the connection string found in the 'IQMediaGroupEntities' section of the application configuration file.
        /// </summary>
        public IQMediaGroupEntities() : base("name=IQMediaGroupEntities", "IQMediaGroupEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new IQMediaGroupEntities object.
        /// </summary>
        public IQMediaGroupEntities(string connectionString) : base(connectionString, "IQMediaGroupEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new IQMediaGroupEntities object.
        /// </summary>
        public IQMediaGroupEntities(EntityConnection connection) : base(connection, "IQMediaGroupEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Function Imports
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="iD">No Metadata Documentation available.</param>
        public ObjectResult<RootPath> GetRootPathByID(Nullable<global::System.Int64> iD)
        {
            ObjectParameter iDParameter;
            if (iD.HasValue)
            {
                iDParameter = new ObjectParameter("ID", iD);
            }
            else
            {
                iDParameter = new ObjectParameter("ID", typeof(global::System.Int64));
            }
    
            return base.ExecuteFunction<RootPath>("GetRootPathByID", iDParameter);
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="iD">No Metadata Documentation available.</param>
        /// <param name="status">No Metadata Documentation available.</param>
        /// <param name="mediaUrl">No Metadata Documentation available.</param>
        /// <param name="packgeUrl">No Metadata Documentation available.</param>
        public ObjectResult<Nullable<global::System.Int32>> ArchiveTVEyesUpdateDownloadStatus(Nullable<global::System.Int64> iD, global::System.String status, global::System.String mediaUrl, global::System.String packgeUrl)
        {
            ObjectParameter iDParameter;
            if (iD.HasValue)
            {
                iDParameter = new ObjectParameter("ID", iD);
            }
            else
            {
                iDParameter = new ObjectParameter("ID", typeof(global::System.Int64));
            }
    
            ObjectParameter statusParameter;
            if (status != null)
            {
                statusParameter = new ObjectParameter("Status", status);
            }
            else
            {
                statusParameter = new ObjectParameter("Status", typeof(global::System.String));
            }
    
            ObjectParameter mediaUrlParameter;
            if (mediaUrl != null)
            {
                mediaUrlParameter = new ObjectParameter("MediaUrl", mediaUrl);
            }
            else
            {
                mediaUrlParameter = new ObjectParameter("MediaUrl", typeof(global::System.String));
            }
    
            ObjectParameter packgeUrlParameter;
            if (packgeUrl != null)
            {
                packgeUrlParameter = new ObjectParameter("PackgeUrl", packgeUrl);
            }
            else
            {
                packgeUrlParameter = new ObjectParameter("PackgeUrl", typeof(global::System.String));
            }
    
            return base.ExecuteFunction<Nullable<global::System.Int32>>("ArchiveTVEyesUpdateDownloadStatus", iDParameter, statusParameter, mediaUrlParameter, packgeUrlParameter);
        }

        #endregion
    }
    

    #endregion
    
    #region ComplexTypes
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmComplexTypeAttribute(NamespaceName="IQMediaGroupModel", Name="RootPath")]
    [DataContractAttribute(IsReference=true)]
    [Serializable()]
    public partial class RootPath : ComplexObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new RootPath object.
        /// </summary>
        /// <param name="storagePath">Initial value of the StoragePath property.</param>
        /// <param name="streamSuffixPath">Initial value of the StreamSuffixPath property.</param>
        public static RootPath CreateRootPath(global::System.String storagePath, global::System.String streamSuffixPath)
        {
            RootPath rootPath = new RootPath();
            rootPath.StoragePath = storagePath;
            rootPath.StreamSuffixPath = streamSuffixPath;
            return rootPath;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String StoragePath
        {
            get
            {
                return _StoragePath;
            }
            set
            {
                OnStoragePathChanging(value);
                ReportPropertyChanging("StoragePath");
                _StoragePath = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("StoragePath");
                OnStoragePathChanged();
            }
        }
        private global::System.String _StoragePath;
        partial void OnStoragePathChanging(global::System.String value);
        partial void OnStoragePathChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String StreamSuffixPath
        {
            get
            {
                return _StreamSuffixPath;
            }
            set
            {
                OnStreamSuffixPathChanging(value);
                ReportPropertyChanging("StreamSuffixPath");
                _StreamSuffixPath = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("StreamSuffixPath");
                OnStreamSuffixPathChanged();
            }
        }
        private global::System.String _StreamSuffixPath;
        partial void OnStreamSuffixPathChanging(global::System.String value);
        partial void OnStreamSuffixPathChanged();

        #endregion
    }

    #endregion
    
}
