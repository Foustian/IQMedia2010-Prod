using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;
using System.Web.Script.Services;
namespace IQMediaGroup.Admin.WebApplication
{
    /// <summary>
    /// Summary description for SearchTerm
    /// </summary>
    [ScriptService]
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SearchTerm : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        [WebMethod]
        public List<string> GetCompletionList(string prefixText)
        {
            try
            {
                List<string> results = new List<string>();
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = _IClientController.GetClientInfoBySearchTerm(prefixText);
                /*List<String> suggetions = GetSuggestions(prefixText, count);*/
                if (_ListOfClient.Count > 0)
                {
                    /*var ClientNames =
                                        from p in _ListOfClient
                                        select p.ClientName;*/
                    foreach (Client _ClientName in _ListOfClient)
                    {
                        string Suggestion = _ClientName.ClientName;
                        results.Add(Suggestion);
                    }

                }

                return results;

            }
            catch (Exception ex)
            {
                //return "Error:" + ex.Message;
                throw ex;
            }

        }

        [WebMethod]
        public List<string> GetCustomerList(string prefixText)
        {
            try
            {
                List<string> customerresults = new List<string>();
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                List<Customer> _ListOfCustomer = _ICustomerController.GetCustomerInfoBySearchTerm(prefixText);
                if (_ListOfCustomer.Count > 0)
                {
                    /*var ClientNames =
                                        from p in _ListOfClient
                                        select p.ClientName;*/
                    foreach (Customer _Customer in _ListOfCustomer)
                    {
                        string Suggestion = _Customer.Email;
                        customerresults.Add(Suggestion);
                    }

                }

                return customerresults;

            }
            catch (Exception ex)
            {
                //return "Error:" + ex.Message;
                throw ex;
            }

        }
    }
}
