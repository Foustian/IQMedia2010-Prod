using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Logic
{
    public class SourceLogic : BaseLogic,ILogic
    {
        public SourceGuidOutput GetSourceGUIDBySourceID(string _sourceID)
        {
            try
            {
                var sourceGuidOutput = new SourceGuidOutput();
                var sourceGUID = Context.GetSourceGUIDBySourceID(_sourceID).FirstOrDefault();
                if (sourceGUID != null)
                {
                    sourceGuidOutput.message = "SourceGuid fetched successfully.";
                    sourceGuidOutput.status = 0;
                    sourceGuidOutput.sourceGuid = sourceGUID;
                }
                else
                {
                    sourceGuidOutput.message = "No SourceGuid found for SourceID:"+_sourceID;
                    sourceGuidOutput.status = 0;
                }

                return sourceGuidOutput;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching SourceGuid for SourceID : " + _sourceID, ex);
                throw ex;

            }
            
        }

    }
}
