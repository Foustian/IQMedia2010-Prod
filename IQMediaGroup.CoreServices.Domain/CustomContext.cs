using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.Extensions;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Domain
{
    public partial class IQMediaGroupEntities : ObjectContext
    {
        public List<MoveRecordFile> GetMoveRecordFileByRPID(int p_RPID, DateTime p_FromDate, DateTime p_ToDate, int p_NumRecords)
        {
            var mmRList = new List<MoveRecordFile>();

            DbCommand command = this.CreateStoreCommand("usp_coresvc_RecordFile_SelectByRPID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@RPID", p_RPID));
            command.Parameters.Add(new SqlParameter("@FromDate", p_FromDate));
            command.Parameters.Add(new SqlParameter("@ToDate", p_ToDate));
            command.Parameters.Add(new SqlParameter("@NumRecords", p_NumRecords));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var mmR = new MoveRecordFile();

                    mmR.RecordFileGUID = Guid.Parse(Convert.ToString(reader["Guid"]));
                    mmR.Location = Convert.ToString(reader["StoragePath"]) + Convert.ToString(reader["Location"]);
                    mmR.Status = Convert.ToString(reader["Status"]);

                    mmRList.Add(mmR);
                }
            }

            return mmRList;
        }

        public Int64 InsertMoveMedia(MoveMedia p_mm, out int p_OutStatus)
        {
            Int64 id = 0;
            p_OutStatus = 1;

            DbCommand command = this.CreateStoreCommand("usp_coresvc_MoveMedia_Insert", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@RecordfileGUID", p_mm.RecordFileGUID));
            command.Parameters.Add(new SqlParameter("@OriginRPID", p_mm.OriginRPID));
            command.Parameters.Add(new SqlParameter("@OriginLocation", p_mm.OriginLocation));
            command.Parameters.Add(new SqlParameter("@OriginStatus", p_mm.OriginStatus));
            command.Parameters.Add(new SqlParameter("@OriginSite", p_mm.OriginSite));
            command.Parameters.Add(new SqlParameter("@DestinationRPID", (object)p_mm.DestinationRPID ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationLocation", p_mm.DestinationLocation ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationStatus", p_mm.DestinationStatus ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationSite", p_mm.DestinationSite ?? (object)DBNull.Value));

            var outIDParam = new SqlParameter("@ID", id);
            outIDParam.Direction = ParameterDirection.Output;

            command.Parameters.Add(outIDParam);

            var outStatusParam = new SqlParameter("@OutStatus", p_OutStatus);
            outStatusParam.Direction = ParameterDirection.Output;

            command.Parameters.Add(outStatusParam);

            using (command.Connection.CreateConnectionScope())
            {
                command.ExecuteNonQuery();

                id = Convert.ToInt64(command.Parameters["@ID"].Value);
                p_OutStatus = Convert.ToInt32(command.Parameters["@OutStatus"].Value);
            }

            return id;
        }

        public int UpdateMoveMedia(MoveMedia p_mm, bool p_UpdateOrigin)
        {
            var affectedrecs = 0;

            DbCommand command = this.CreateStoreCommand("usp_coresvc_MoveMedia_Update", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ID", (p_mm.ID > 0 ? (object)p_mm.ID : (object)DBNull.Value)));
            command.Parameters.Add(new SqlParameter("@RecordfileGUID", p_mm.RecordFileGUID));
            command.Parameters.Add(new SqlParameter("@OriginRPID", p_mm.OriginRPID));
            command.Parameters.Add(new SqlParameter("@OriginStatus", p_mm.OriginStatus ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationRPID", (object)p_mm.DestinationRPID ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationLocation", p_mm.DestinationLocation ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationStatus", p_mm.DestinationStatus ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationSite", p_mm.DestinationSite ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@UpdateOrigin", p_UpdateOrigin));

            using (command.Connection.CreateConnectionScope())
            {
                affectedrecs = Convert.ToInt32(command.ExecuteScalar());
            }

            return affectedrecs;
        }

        public List<MoveMedia> GetMoveMedia(GetMoveMediaListInput p_mm)
        {
            var mmList = new List<MoveMedia>();

            DbCommand command = this.CreateStoreCommand("usp_coresvc_MoveMedia_Select", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@OriginSite", p_mm.OriginSite ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationSite", p_mm.DestinationSite ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@OriginStatus", p_mm.OriginStatus ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DestinationStatus", p_mm.DestinationStatus ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@NumRecords", p_mm.NumRecords));

            using (command.Connection.CreateConnectionScope())
            {
                mmList = command.Materialize<MoveMedia>(r => new MoveMedia
                {
                    ID = r.Field<Int64>("ID"),
                    RecordFileGUID = r.Field<Guid>("_RecordFileGUID"),
                    OriginRPID = r.Field<int>("OriginRPID"),
                    OriginStatus = r.Field<string>("OriginStatus"),
                    OriginLocation = r.Field<string>("OriginLocation"),
                    OriginSite = r.Field<string>("OriginSite"),
                    DestinationRPID = r.Field<int?>("DestinationRPID"),
                    DestinationStatus = r.Field<string>("DestinationStatus"),
                    DestinationLocation = r.Field<string>("DestinationLocation"),
                    DestinationSite = r.Field<string>("DestinationSite"),
                    DateCreated = r.Field<DateTime>("DateCreated"),
                    DateModified = r.Field<DateTime?>("DateModified")
                }).ToList();
            }

            return mmList;
        }
    }
}
