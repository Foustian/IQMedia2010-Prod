using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace IQMedia.Service.TVEyes.Download.Service
{
    [ServiceContract]
    public interface IDownload
    {
        [OperationContract]
        void WakeupService();
    }
}
