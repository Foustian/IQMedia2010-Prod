using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using IQMedia.Common.Util;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class RecordfileLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the record file.
        /// </summary>
        /// <param name="guid">The RecordFile GUID.</param>
        /// <returns></returns>
        public Recordfile GetRecordfile(Guid guid)
        {
            return Context.Recordfiles.SingleOrDefault(rf => rf.Guid.Equals(guid));
        }

        public Recordfile GetTextRecordfile(Recordfile rFile)
        {
            return Context.Recordfiles.FirstOrDefault
                (rf => (rf.RecordingID == rFile.RecordingID) && rf.RecordfileTypeID == RecordfileType.TEXT.ID);
        }

        public Recordfile GetLocalRecordfile(Recordfile rFile)
        {
            var rf = rFile;

            while (rf != null && rf.ParentGuid != null && !rf.ParentGuid.Equals("00000000-0000-0000-0000-000000000000"))
            {
                var temp = GetRecordfile(rf.ParentGuid.Value);
                if (temp != null) rf = temp;
            }
            return rf;

        }

        /// <summary>
        /// Inserts a collection of Recordfiles and rolls-back on any errors.
        /// </summary>
        /// <param name="recordfiles">The recordfiles.</param>
        /// <returns>
        ///     <c>true</c> if the transaction completed; otherwise, <c>false</c>.
        /// </returns>
        public bool InsertAllOrRollback(IEnumerable<Recordfile> recordfiles)
        {
            var success = false;

            using(var transaction = new TransactionScope())
            {
                try
                {
                    foreach (var recordfile in recordfiles)
                    {
                        if (recordfile.Guid.Equals(Guid.Empty))
                        {
                            recordfile.Guid = Guid.NewGuid();
                            //NOTE: We have to makee sure the navigation properties are 'null' because if not,
                            //when we try to save, it will throw an error about multiple instances of IEntityChangeTraker
                            recordfile.Recording = null;
                            recordfile.RootPath = null;
                            recordfile.RecordfileType = null;
                            Context.Recordfiles.AddObject(recordfile);
                        }
                        else
                        {
                            //gotta do an update...
                            //NOTE: This is annoyingly tedious because we have to "transcode" the object
                            var rfile = Context.Recordfiles.SingleOrDefault(rf => rf.Guid.Equals(recordfile.Guid));
                            rfile.EndOffset = recordfile.EndOffset;
                            rfile.LastModified = DateTime.Now;
                            rfile.Location = recordfile.Location;
                            rfile.ParentGuid = recordfile.ParentGuid;
                            rfile.RecordfileTypeID = recordfile.RecordfileTypeID;
                            rfile.RecordingID = recordfile.RecordingID;
                            rfile.RootPathID = recordfile.RootPathID;
                            rfile.StartOffset = recordfile.StartOffset;
                            rfile.Status = recordfile.Status;
                        }
                        Context.SaveChanges(SaveOptions.DetectChangesBeforeSave);
                    }
                    transaction.Complete();
                    success = true;
                }
                catch(Exception ex)
                {
                    Logger.Error("An error occurred while trying to save multiple recordfiles. The transaction has been rolled back.", ex);
                }
                
                if(success) Context.AcceptAllChanges();
                return success;
            }
        }

    }
}
