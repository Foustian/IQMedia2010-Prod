using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQ_NewsController
    {
        List<IQ_News> GetIQNews();
        
    }
}
