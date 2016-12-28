using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller;
using IQMediaGroup.Controller.Interface;
using System.IO;
using System.Xml;
using System.Configuration;

namespace IQMediaGroup.IQAgent_Fetch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(CommonConstants.CAStarted);

                string _FilePath = null;

                if (args.Length > 0)
                {
                    _FilePath = args[0];
                }
                else
                {
                    _FilePath = @"E:\IQMedia\Data\IQAgent.xml";
                   // throw new MyException(CommonConstants.MsgFileNotExist);
                }

                int _NoOfRecordsInserted = 0;

                ControllerFactory _ControllerFactory = new ControllerFactory();
                ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();                

                _NoOfRecordsInserted = _ISearchRequestController.SearchRawMediaFromPMGSearch(_FilePath);

                Console.WriteLine(_NoOfRecordsInserted.ToString() + CommonConstants.Space + CommonConstants.InsertedRecords);
                Console.WriteLine(CommonConstants.CACompleted);
            }
            catch (System.Data.SqlClient.SqlException)
            {
                Console.WriteLine("Database Error encountered");
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("Webexception occurs");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timeout occurs");
            }
            catch (MyException _MyException)
            {
                Console.WriteLine(_MyException._StrMsg);
            }
            catch (Exception _Exception)
            {
                Console.WriteLine(_Exception.Message + CommonConstants.UnderScore + _Exception.StackTrace);
            }
        }
    }
}
