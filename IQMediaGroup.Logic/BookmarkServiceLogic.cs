using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using IQMediaGroup.Domain;
using System.Security.Cryptography;
using System.IO;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public class BookmarkServiceLogic :BaseLogic,ILogic
    {
        public string BookmarkLink(BookMarkInput _BookMarkInput) 
        {
            
            try
            {

                string _BookmarkURL = _BookMarkInput.ClipURL + _BookMarkInput.FileID;

               
                return _BookmarkURL;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
