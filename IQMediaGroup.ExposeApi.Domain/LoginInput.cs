using System;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    
    public class LoginInput
    {
        public string UserID { get; set; }

        public string Password { get; set; }

        public string SessionID { get; set; }      
    }
}