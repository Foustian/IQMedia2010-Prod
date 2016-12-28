using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain.IOS
{
    public class CustomException : Exception
    {
        public string Message { get; set; }

        public CustomException(string p_StrMsg)
        {
            Message = p_StrMsg;
        }
    }
}
