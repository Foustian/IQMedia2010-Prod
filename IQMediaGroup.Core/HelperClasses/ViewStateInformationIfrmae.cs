using System;

namespace IQMediaGroup.Core.HelperClasses
{
     [Serializable]
    public class ViewStateInformationIframe
    {       

        public Guid? ClientGUID { get; set; }

        public Guid? CustomerGUID { get; set; }
    }
}