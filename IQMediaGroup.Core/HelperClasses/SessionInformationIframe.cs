using System;

namespace IQMediaGroup.Core.HelperClasses
{
    public class SessionInformationIframe
    {
        public Guid? ClientGUID { get; set; }

        public Guid? CustomerGUID { get; set; }

        public bool? IsRedirect { get; set; }

        public bool? IsActivePlayerLogo { get; set; }

        public string PlayerLogoImage { get; set; }
    }
}