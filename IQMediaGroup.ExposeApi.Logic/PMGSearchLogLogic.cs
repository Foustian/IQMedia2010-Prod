using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Data.Objects;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class PMGSearchLogLogic : BaseLogic, ILogic
    {
        public void InsertPMGSearchLog(string _Terms, int _PageNumber, int _PageSize, int _MaxHighlights, DateTime? _StartDate, DateTime? _EndDate,
                                        string _Response,string Title120,
                                        string Appearing, string IQ_Time_Zone, Int64 CustomerID, List<string> p_DMA_Num, List<string> p_Class_Num, List<string> p_Affil_Num, string _SearchType)
        {
            try
            {
                string _FileContent = string.Empty;
                _FileContent = "<PMGRequest>";
                _FileContent += "<Terms>" + _Terms + "</Terms>";
                _FileContent += "<PageNumber>" + _PageNumber + "</PageNumber>";
                _FileContent += "<PageSize>" + _PageSize + "</PageSize>";
                _FileContent += "<MaxHighlights>" + _MaxHighlights + "</MaxHighlights>";
                //_FileContent += "<IQCCKeyList>" + _GUIDList + "</IQCCKeyList>";
                if (_StartDate.HasValue)
                {
                    _FileContent += "<StartDate>" + _StartDate + "</StartDate>";
                }
                else
                {
                    _FileContent += "<StartDate></StartDate>";
                }             

                if (!string.IsNullOrEmpty(IQ_Time_Zone))
                {
                    _FileContent += "<IQ_Time_Zone>" + IQ_Time_Zone + "</IQ_Time_Zone>";
                }
                else
                {
                    _FileContent += "<IQ_Time_Zone />";
                }               

                if (!string.IsNullOrEmpty(Title120))
                {
                    _FileContent += "<Title120>" + Title120 + "</Title120>";
                }
                else
                {
                    _FileContent += "<Title120 />";
                }

                if (!string.IsNullOrEmpty(Appearing))
                {
                    _FileContent += "<Appearing>" + Appearing + "</Appearing>";
                }
                else
                {
                    _FileContent += "<Appearing />";
                }

                if (p_DMA_Num != null && p_DMA_Num.Count() > 0)
                {
                    _FileContent += "<IQ_Dma_Num>" + string.Join(",",p_DMA_Num) + "</IQ_Dma_Num>";
                }
                else
                {
                    _FileContent += "<IQ_Dma_Num />";
                }

                if (p_Class_Num != null && p_Class_Num.Count() >0)
                {
                    _FileContent += "<IQ_Class_Num>" + string.Join(",", p_Class_Num) + "</IQ_Class_Num>";
                }
                else
                {
                    _FileContent += "<IQ_Class_Num />";
                }
                if (p_Affil_Num != null && p_Affil_Num.Count() > 0)
                {
                    _FileContent += "<Station_Affil_Num>" + string.Join(",", p_Affil_Num) + "</Station_Affil_Num>";
                }
                else
                {
                    _FileContent += "<Station_Affil />";
                }

                if (_EndDate.HasValue)
                {
                    _FileContent += "<EndDate>" + _EndDate + "</EndDate>";
                }
                else
                {
                    _FileContent += "<EndDate></EndDate>";
                }


                _FileContent += "</PMGRequest>";

                _FileContent = _FileContent.Replace("&", "&amp;");

                string _Result = string.Empty;

                PMGSearchLog PMGSearchLog = new PMGSearchLog();

                PMGSearchLog.SearchType = _SearchType;
                PMGSearchLog.RequestXML = _FileContent;
                PMGSearchLog.ErrorResponseXML = _Response;
                PMGSearchLog.CreatedDate = DateTime.Now;
                PMGSearchLog.ModifiedDate = DateTime.Now;
                PMGSearchLog.IsActive = true;
                PMGSearchLog.CreatedBy = "Service";
                PMGSearchLog.ModifiedBy = "Service";
                PMGSearchLog.CustomerID = Convert.ToInt32(CustomerID);

                //Context.InsertPMGSearchLog(PMGSearchLog.CustomerID, PMGSearchLog.SearchType, PMGSearchLog.RequestXML, PMGSearchLog.ErrorResponseXML, PMGSearchLogKey);

                Context.PMGSearchLogs.AddObject(PMGSearchLog);

                Context.SaveChanges();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
