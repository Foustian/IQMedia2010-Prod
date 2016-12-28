using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IQMediaGroup.TVEyes.Commands
{
    public class NullCommand : ICommand
    {
        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {
            response.Output.Write("<p style='font-weight:bold;'>Error<p>"
                + "<p>The request could not be completed due to insufficient information.</p>");
        }

        #endregion
    }
}
