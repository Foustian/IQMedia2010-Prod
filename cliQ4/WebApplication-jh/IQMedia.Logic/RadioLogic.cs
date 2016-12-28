using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Logic.Base;
using IQMedia.Data;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class RadioLogic : ILogic
    {
        public List<string> SelectRadioStations()
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectRadioStations();
        }

        public List<RadioModel> SelectRadioResults(DateTime? FromDate, DateTime? ToDate, string Market, bool IsAsc, int PageNo, int PageSize, ref long SinceID, out long TotalResults)
        {
            RadioDA radioDA = (RadioDA)DataAccessFactory.GetDataAccess(DataAccessType.Radio);
            return radioDA.SelectRadioResults(FromDate, ToDate, Market, IsAsc, PageNo, PageSize, ref SinceID, out TotalResults);
        }
    }
}
