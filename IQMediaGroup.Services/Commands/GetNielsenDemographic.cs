using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetNielsenDemographic : ICommand
    {

        public String _Format { get; private set; }

        public GetNielsenDemographic(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var output = new NielsenDemographicOutput();
            CLogger.Debug("GetNielsenDemographic Request Started");

            try
            {
                var nlsnDgfInput = IQMediaGroup.Services.Util.CommonFunctions.InitializeRequest<NielsenDemographicInput>(p_HttpRequest, _Format);                

                if (nlsnDgfInput.ClientGuid == Guid.Empty)
                {
                    throw new CustomException("Client cannot be empty");
                }

                if (nlsnDgfInput.IQCCKeyList!=null && nlsnDgfInput.IQCCKeyList.Count>0)
                {
                    int cnt = nlsnDgfInput.IQCCKeyList.Where(iqcckey => string.IsNullOrWhiteSpace(iqcckey.IQCCKey) || string.IsNullOrWhiteSpace(iqcckey.IQ_Dma_Num) || iqcckey.IQStartPoint <= 0).Count();

                    if (cnt > 0)
                    {
                        throw new CustomException("Invalid Input");
                    }
                }

                if (nlsnDgfInput.GUIDList!=null && nlsnDgfInput.GUIDList.Count > 0)
                {
                    int cnt = nlsnDgfInput.GUIDList.Where(guid => (guid.GUID == Guid.Empty) || string.IsNullOrWhiteSpace(guid.IQ_Dma_Num) || guid.IQStartPoint <= 0).Count();

                    if (cnt > 0)
                    {
                        throw new CustomException("Invalid Input");
                    }
                }
                
                if (nlsnDgfInput.IQCCKeyList!=null && nlsnDgfInput.IQCCKeyList.Count >0)
                {
                    nlsnDgfInput.IsGUID = false;                    
                }
                else if (nlsnDgfInput.GUIDList!=null && nlsnDgfInput.GUIDList.Count >0)
                {
                    nlsnDgfInput.IsGUID = true;
                }
                else
                {
                    throw new CustomException("Invalid Request");
                }

                var nlsnLgc = (NielSenDataLogic)LogicFactory.GetLogic(LogicType.NielSen);
                List<NielsenDemographicData> nlsnData = nlsnLgc.GetNielsenDemographicByList(nlsnDgfInput);

                output.NielsenDemographicDataList = nlsnData;
                output.Status = 0;
                output.Message = "Success";
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

            CLogger.Debug("GetNielsenDemographic Request Ended");
            IQMediaGroup.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, output, _Format);
        }
    }
}