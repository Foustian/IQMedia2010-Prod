using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Data.Objects;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class StatskedprogLogic : BaseLogic, ILogic
    {
        public StatskedprogData GetStatskedprogData(Guid p_ClientGUID)
        {
            try
            {
                StatskedprogData _StatskedprogData = new StatskedprogData();

                bool isAllDma, isAllStationAffil, isAllClass;

                var sspData = Context.GetStatskedprogDataByClientGUID(p_ClientGUID, out isAllDma, out isAllStationAffil, out isAllClass);

                _StatskedprogData.DmaList = (List<Dma>)sspData["IQ_Dma"];
                _StatskedprogData.AffiliateList = (List<Affiliate>)sspData["Station_Affil"];
                _StatskedprogData.ProgramCategoryList = (List<Class>)sspData["IQ_Class"];
                _StatskedprogData.RegionList = (List<Region>)sspData["IQ_Region"];
                _StatskedprogData.CountryList = (List<Country>)sspData["IQ_Country"];
                _StatskedprogData.StationList = (List<Station>)sspData["IQ_Station"];

                _StatskedprogData.IsAllDma = isAllDma;
                _StatskedprogData.IsAllAffiliate = isAllStationAffil;

                return _StatskedprogData;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }

        }


    }
}
