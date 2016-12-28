using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace IQMedia.Service.TVEyes.RequestDownload.Service
{
    [ServiceContract]
    public interface IRequestDownload
    {
        [OperationContract]
        void WakeupService();
    }
}
