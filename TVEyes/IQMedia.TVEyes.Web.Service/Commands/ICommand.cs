using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IQMedia.TVEyes.Web.Service.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Function that is called to run the command after the constructor is invoked.
        /// </summary>
        void Execute(HttpRequest request, HttpResponse response);
    }
}
