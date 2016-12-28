using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Serializers;
using System.IO;

namespace IQMediaGroup.CoreServices.Commands
{
    public class NullCommand : ICommand
    {
        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {
            //StreamReader _obj  = new StreamReader(@"E:\All Projects\IQMedia\obama_clip_03012012_102710.wmv.xml");
            //string _inputXMLString = _obj.ReadToEnd();
            //UGCXml _objIngestionData = new UGCXml();
            //_objIngestionData = (UGCXml)Serializer.DeserialiazeXml(_inputXMLString, _objIngestionData);            


            response.Output.Write("<p style='font-weight:bold;'>Error<p>"
                + "<p>The request could not be completed due to insufficient information.</p>");
        }   

        


        
        #endregion

        
    }
}