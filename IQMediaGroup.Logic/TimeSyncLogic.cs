using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Json;

namespace IQMediaGroup.Logic
{
    public class TimeSyncLogic : BaseLogic, ILogic
    {
        public InsertClipTimeSyncOutput InsertClipTimeSync(InsertClipTimeSyncInput p_InsertClipTimeSyncInput)
        {
            try
            {
                InsertClipTimeSyncOutput insertClipTimeSyncOutput = new InsertClipTimeSyncOutput();

                List<TimeSyncData> lstTimeSyncData = Context.GetIQTimeSyncDataByClipGuid(p_InsertClipTimeSyncInput.ClipGuid.Value).ToList();

                XDocument xDoc = new XDocument(new XElement("list"));
                foreach(TimeSyncData item in lstTimeSyncData)
                {
                    IQTimeSyncModel iQTimeSyncModel = new IQTimeSyncModel();
                    iQTimeSyncModel = (IQTimeSyncModel)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Data, iQTimeSyncModel.GetType());


                    IQTimeSyncModel iQClipTimeSyncModel = new IQTimeSyncModel();
                    iQClipTimeSyncModel.data = iQTimeSyncModel.data.Where(a => a.S >= p_InsertClipTimeSyncInput.StartOffset && a.S <= p_InsertClipTimeSyncInput.EndOffset).ToList();

                    xDoc.Root.Add(new XElement("item", new XElement("Type", item.C_TypeID), new XElement("Data", Newtonsoft.Json.JsonConvert.SerializeObject(iQClipTimeSyncModel))));
                }

                var rowsAffected = Context.InsertIQClipTimeSyncData(p_InsertClipTimeSyncInput.ClipGuid, xDoc.ToString()).FirstOrDefault();

                if (rowsAffected > 0)
                {
                    insertClipTimeSyncOutput.Message = "Success";
                    insertClipTimeSyncOutput.Status = 0;
                }
                else
                {
                    insertClipTimeSyncOutput.Message = "Failed to insert in database";
                    insertClipTimeSyncOutput.Status = 1;
                }

                return insertClipTimeSyncOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        

        
    }


}
