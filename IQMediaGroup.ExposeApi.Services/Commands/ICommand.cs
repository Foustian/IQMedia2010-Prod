using System.Web;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Function that is called to run the command after the constructor is invoked.
        /// </summary>
        void Execute(HttpRequest request, HttpResponse response);
    }
}
