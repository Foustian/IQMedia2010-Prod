using System;
using System.Web;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using IQMediaGroup.Services.Util;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;

namespace IQMediaGroup.Services.Commands
{
    public class GetCompeteDemographic : ICommand
    {
        public String _Format { get; private set; }

        public GetCompeteDemographic(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var output = new CompeteDemographicOutput();

            CLogger.Debug("GetCompeteDemographic Request Started");

            try
            {
                var cmptDgfInput = IQMediaGroup.Services.Util.CommonFunctions.InitializeRequest<CompeteDemographicInput>(p_HttpRequest, _Format);

                if (cmptDgfInput.ClientGuid == Guid.Empty)
                {
                    throw new CustomException("Client cannot be empty");
                }

                if (cmptDgfInput.CompeteUrlSet != null && cmptDgfInput.CompeteUrlSet.Count  > 0)
                {
                    int cnt = cmptDgfInput.CompeteUrlSet.Where(url => string.IsNullOrWhiteSpace(url)).Count();

                    if (cnt > 0)
                    {
                        throw new CustomException("Invalid Input");    
                    }

                    cmptDgfInput.CompeteUrlSet = cmptDgfInput.CompeteUrlSet.Select(u => u.Trim()).Distinct().ToList();
                }
                else
                {
                    throw new CustomException("Invalid Input");
                }

                CompeteDataLogic cmptLgc = (CompeteDataLogic)LogicFactory.GetLogic(LogicType.Compete);

                output.CompeteDemographicDataList = cmptLgc.GetCompeteDemographic(cmptDgfInput.ClientGuid, cmptDgfInput.CompeteUrlSet, cmptDgfInput.SubMediaType);

                output.Message = "Success";
                output.Status = 0;
            }
            catch (CustomException ex)
            {
                output.Status = -1;
                output.Message = ex.Message;

                CLogger.Error(ex);
            }
            catch (Exception ex)
            {
                output.Status = -2;
                output.Message = "An error occurred, please try again.";

                CLogger.Error(ex);
            }

            CLogger.Debug("GetCompeteDemographic Request Ended");
            IQMediaGroup.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, output, _Format);
        }
    }
}