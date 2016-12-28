using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Common;
using System.Data;
using Microsoft.Data.Extensions;
using System.Data.SqlClient;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Domain
{
    public partial class IQMediaGroupISVCEntities : ObjectContext
    {
        public Dictionary<string, object> GetIQLicenseDetailByClientGUID(Guid p_ClientGUID)
        {

            DbCommand command = this.CreateStoreCommand("usp_iqsvc_IQClient_CustomSettings_CheckIQLicenseByCustomerID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGUID", p_ClientGUID));
            Dictionary<string, object> licenseDetail = new Dictionary<string, object>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                { 
                    licenseDetail.Add("IQLicense",Convert.ToString(reader["IQLicense"]));
                    licenseDetail.Add("ClientID", Convert.ToInt64(reader["ClientID"]));
                    licenseDetail.Add("CustomerID", Convert.ToInt64(reader["CustomerID"]));
                }
            }

            return licenseDetail;
        }

        public Dictionary<string, string> GetStationAffiliateNCallSign(string p_StationID)
        {

            DbCommand command = this.CreateStoreCommand("usp_iqsvc_IQ_Station_SelectByStationID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@StationID", p_StationID));
            Dictionary<string, string> stationDetail = new Dictionary<string, string>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    stationDetail.Add("Affiliate", Convert.ToString(reader["Affiliate"]));
                    stationDetail.Add("CallSign", Convert.ToString(reader["CallSign"]));
                }
            }

            return stationDetail;
        }

        public List<NielsenDemographicData> GetNielsenDemographicDataByGUID(Guid p_ClientGUID,string p_NielsenInput)
        {

            DbCommand command = this.CreateStoreCommand("usp_iqsvc_SelectNielSenDemographicAgeDataByRecordFileGuidList", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@GuidList", p_NielsenInput));
            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGUID));

            var nlsnDgfDataList = new List<NielsenDemographicData>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var nlsnData = new NielsenDemographicData();

                    nlsnData.AF18_20 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_18_20"]);
                    nlsnData.AF21_24 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_21_24"]);
                    nlsnData.AF25_34 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_25_34"]);
                    nlsnData.AF35_49 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_35_49"]);
                    nlsnData.AF50_54 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_50_54"]);
                    nlsnData.AF55_64 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_55_64"]);
                    nlsnData.AF65 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_ABOVE_65"]);

                    nlsnData.AM18_20 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_18_20"]);
                    nlsnData.AM21_24 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_21_24"]);
                    nlsnData.AM25_34 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_25_34"]);
                    nlsnData.AM35_49 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_35_49"]);
                    nlsnData.AM50_54 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_50_54"]);
                    nlsnData.AM55_64 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_55_64"]);
                    nlsnData.AM65 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_ABOVE_65"]);

                    nlsnData.SQAD_SHAREVALUE = Convert.ToString(reader["SQAD_SHAREVALUE"]);

                    nlsnData.GUID = Guid.Parse(Convert.ToString(reader["Guid"]));
                    nlsnData.IQ_CC_Key = Convert.ToString(reader["IQ_CC_Key"]);

                    nlsnDgfDataList.Add(nlsnData);
                }
            }

            return nlsnDgfDataList;
        }

        public List<NielsenDemographicData> GetNielsenDemographicDataByIQCCKey(Guid p_ClientGUID, string p_NielsenInput)
        {

            DbCommand command = this.CreateStoreCommand("usp_iqsvc_SelectNielSenDemographicAgeData_SelectByIQCCKeyList", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@IQCCKeyList", p_NielsenInput));
            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGUID));

            var nlsnDgfDataList = new List<NielsenDemographicData>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var nlsnData = new NielsenDemographicData();

                    nlsnData.AF18_20 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_18_20"]);
                    nlsnData.AF21_24 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_21_24"]);
                    nlsnData.AF25_34 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_25_34"]);
                    nlsnData.AF35_49 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_35_49"]);
                    nlsnData.AF50_54 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_50_54"]);
                    nlsnData.AF55_64 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_55_64"]);
                    nlsnData.AF65 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["FEMALE_AUDIENCE_ABOVE_65"]);

                    nlsnData.AM18_20 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_18_20"]);
                    nlsnData.AM21_24 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_21_24"]);
                    nlsnData.AM25_34 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_25_34"]);
                    nlsnData.AM35_49 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_35_49"]);
                    nlsnData.AM50_54 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_50_54"]);
                    nlsnData.AM55_64 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_55_64"]);
                    nlsnData.AM65 = CommonFunctions.CheckDBNullNReturnValue<Decimal>(reader["MALE_AUDIENCE_ABOVE_65"]);

                    nlsnData.SQAD_SHAREVALUE = Convert.ToString(reader["SQAD_SHAREVALUE"]) + (Convert.ToBoolean(reader["IsActualNielsen"])?"(A)":"(E)");
                    
                    nlsnData.IQ_CC_Key = Convert.ToString(reader["IQ_CC_Key"]);

                    nlsnDgfDataList.Add(nlsnData);
                }
            }

            return nlsnDgfDataList;
        }

        public List<CompeteDemographicData> GetCompeteDemographicData(Guid p_ClientGUID, string p_CompeteInput, string p_SubMediaType)
        {

            DbCommand command = this.CreateStoreCommand("usp_iqsvc_IQ_CompeteAll_Demographic_Select", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("@CompeteURLXml", p_CompeteInput));
            command.Parameters.Add(new SqlParameter("@SubMediaType", p_SubMediaType));

            var cmptDgfDataList = new List<CompeteDemographicData>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var cmptData = new CompeteDemographicData();

                    cmptData.AF18_24 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["FEMALE_AUDIENCE_18_24"]);
                    cmptData.AF25_34 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["FEMALE_AUDIENCE_25_34"]);
                    cmptData.AF35_44 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["FEMALE_AUDIENCE_35_44"]);
                    cmptData.AF45_54 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["FEMALE_AUDIENCE_45_54"]);
                    cmptData.AF55_64 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["FEMALE_AUDIENCE_55_64"]);
                    cmptData.AF65 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["FEMALE_AUDIENCE_ABOVE_65"]);

                    cmptData.AM18_24 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["MALE_AUDIENCE_18_24"]);
                    cmptData.AM25_34 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["MALE_AUDIENCE_25_34"]);
                    cmptData.AM35_44 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["MALE_AUDIENCE_35_44"]);
                    cmptData.AM45_54 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["MALE_AUDIENCE_45_54"]);
                    cmptData.AM55_64 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["MALE_AUDIENCE_55_64"]);
                    cmptData.AM65 = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["MALE_AUDIENCE_ABOVE_65"]);

                    cmptData.c_uniq_visitor = CommonFunctions.CheckDBNullNReturnValue<Int64?>(reader["c_uniq_visitor"]);
                    cmptData.CompeteURL = Convert.ToString(reader["CompeteURL"]);
                    cmptData.IQ_AdShare_Value = CommonFunctions.CheckDBNullNReturnValue<Decimal?>(reader["IQ_AdShare_Value"]);
                    cmptData.IsCompeteAll = Convert.ToBoolean(reader["IsCompeteAll"]);
                    cmptData.IsUrlFound = Convert.ToBoolean(reader["IsUrlFound"]);

                    cmptDgfDataList.Add(cmptData);
                }
            }

            return cmptDgfDataList;
        }
    }
}
