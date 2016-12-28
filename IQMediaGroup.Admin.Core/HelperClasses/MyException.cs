using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    public class MyException : Exception
    {
        public string _StrMsg = string.Empty;

        public MyException(string p_StrMsg)
        {            
            _StrMsg = p_StrMsg;
        }
    }
}
