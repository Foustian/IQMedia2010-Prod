using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Logic
{
    public class FiveMinStagingLogic : BaseLogic,ILogic
    {
        #region Five Min Staging Media Core Services

        public InsertFiveMinStagingOutput InsertFiveMinStaging(string iqcckey)
        {
            try
            {
                var insertFiveMinStagingOutput = new InsertFiveMinStagingOutput();
                var isInserted =  Context.InsertFiveMinStaging(iqcckey).FirstOrDefault();
                if (isInserted > 0)
                {
                    insertFiveMinStagingOutput.status = 0;
                    insertFiveMinStagingOutput.message = "Record inserted successfully.";
                }
                else
                {
                    insertFiveMinStagingOutput.status = 1;
                    insertFiveMinStagingOutput.message = "Record with iqcckey already inserted.";
                }
                return insertFiveMinStagingOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while inserting FiveMinStaging iqcckey:" + iqcckey, ex);
                throw ex;
            }
        }

        public UpdateFiveMinStagingOutput UpdateFiveMinStaging(Guid recordFileGUID, int lastMediaSeg, string mediaFilename, string mediaStatus, string iqcckey)
        {
            try
            {
                var updateFiveMinStagingOutput = new UpdateFiveMinStagingOutput();
                var recordAffected = Context.UpdateFiveMinStaging(iqcckey,recordFileGUID,lastMediaSeg,mediaStatus,mediaFilename).FirstOrDefault();
                if (recordAffected > 0)
                {
                    updateFiveMinStagingOutput.status = 0;
                    updateFiveMinStagingOutput.message = "Record updated successfully.";
                }
                else
                {
                    updateFiveMinStagingOutput.status = 1;
                    updateFiveMinStagingOutput.message = "Record couldn't updated";
                }
                

                return updateFiveMinStagingOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while updating FiveMinStaging iqcckey:"+iqcckey, ex);
                throw ex;
            }
        }

        public FiveMinStagingOutput GetFiveMinStaging(string iqcckey)
        {
            try
            {
                var fiveMinStagingOutput = new FiveMinStagingOutput();
                var listOfFiveMinStaging = Context.GetFiveMinStagingByIQCCKey(iqcckey).FirstOrDefault();
                //var listOfFiveMinStaging = result.Count() > 0 ? result. : null;
                if (listOfFiveMinStaging != null)
                {
                    fiveMinStagingOutput.status = 0;
                    fiveMinStagingOutput.message = "Record fetched successfully";
                    fiveMinStagingOutput.fiveMinStaging = listOfFiveMinStaging;

                }
                else
                {
                    fiveMinStagingOutput.status = 0;
                    fiveMinStagingOutput.message = "No record found";
                }
                return fiveMinStagingOutput;

            }
            catch(Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching FiveMinStaging iqcckey:" + iqcckey, ex);
                throw ex;
            }
        }

        #endregion

        #region Five Min Staging CC Core Services

        public InsertFiveMinStagingCCOutput InsertFiveMinStagingwithCC(string iqcckey)
        {
            try
            {
                var insertFiveMinStagingCCOutput = new InsertFiveMinStagingCCOutput();
                var isInserted = Context.InsertIQFiveMinStagingWithCC(iqcckey).FirstOrDefault();
                if (isInserted > 0)
                {
                    insertFiveMinStagingCCOutput.status = 0;
                    insertFiveMinStagingCCOutput.message = "Record inserted successfully.";
                }
                else
                {
                    insertFiveMinStagingCCOutput.status = 1;
                    insertFiveMinStagingCCOutput.message = "Record with iqcckey already inserted.";
                }
                return insertFiveMinStagingCCOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while inserting FiveMinStagingwithCC iqcckey:" + iqcckey, ex);
                throw ex;
            }


        }

        public FiveMinStagingCCOutput GetFiveMinStagingCC(string iqcckey)
        {
            try
            {
                var fiveMinStagingCCOutput = new FiveMinStagingCCOutput();
                var listOfFiveMinStagingcc = Context.GetFiveMinStagingCCByIQCCKey(iqcckey).FirstOrDefault();
                //var listOfFiveMinStaging = result.Count() > 0 ? result. : null;
                if (listOfFiveMinStagingcc != null)
                {
                    fiveMinStagingCCOutput.status = 0;
                    fiveMinStagingCCOutput.message = "Record fetched successfully";
                    fiveMinStagingCCOutput.fiveMinStagingCC = listOfFiveMinStagingcc;

                }
                else
                {
                    fiveMinStagingCCOutput.status = 0;
                    fiveMinStagingCCOutput.message = "No record found";
                }
                return fiveMinStagingCCOutput;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching FiveMinStaging iqcckey:" + iqcckey, ex);
                throw ex;
            }
        }

        public UpdateFiveMinStagingCCOutput UpdateFiveMinStagingCC(Guid recordFileGUID, int lastCCTextSegment, string ccTxtFilename, string ccTxtStatus, string iqcckey)
        {
            try
            {
                var updateFiveMinStagingCCOutput = new UpdateFiveMinStagingCCOutput();
                var recordAffected = Context.UpdateFiveMinStagingCC(iqcckey, recordFileGUID, lastCCTextSegment, ccTxtFilename, ccTxtStatus).FirstOrDefault();
                if (recordAffected > 0)
                {
                    updateFiveMinStagingCCOutput.status = 0;
                    updateFiveMinStagingCCOutput.message = "Record updated successfully.";
                }
                else
                {
                    updateFiveMinStagingCCOutput.status = 1;
                    updateFiveMinStagingCCOutput.message = "Record couldn't updated";
                }
                return updateFiveMinStagingCCOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while updating FiveMinStagingCC iqcckey:" + iqcckey, ex);
                throw ex;
            }
        }

        #endregion
    }
}
