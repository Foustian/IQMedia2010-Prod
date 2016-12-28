using System.Web;
using IQMedia.Services.SMS.Logic;

namespace IQMedia.Services.SMS.Commands
{
    public class CTSMSResult : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            var ctSMSResultLgc = (CTSMSResultLogic)LogicFactory.GetLogic(LogicType.CTSMSResult);
            var result = ctSMSResultLgc.InsertCTSMSResult();
        }

    }
}