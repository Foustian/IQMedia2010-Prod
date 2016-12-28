using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using System.IO;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Implementation;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.IQAgent_Notification
{
    class Program
    {
        private readonly static ControllerFactory _ControllerFactory = new ControllerFactory();
        static void Main(string[] args)
        {
            try
            {
                string _FilePath = null;
                if (args.Length > 0)
                {
                    _FilePath = args[0];
                }
                else
                {
                   // _FilePath = "D:\\vishal\\Sites\\NET\\Main\\IQMediaGroup2010\\IQMediaGroup.IQAgent_Notification\\IQAgentNotificationConfig.xml";
                
                }

                IIQNotificationTrackingController _IQNotificationTrackingController = _ControllerFactory.CreateObject<IIQNotificationTrackingController>();
                int NoofNotificationSent = _IQNotificationTrackingController.SendNotifications(_FilePath);

                Console.WriteLine(NoofNotificationSent.ToString() + " Notification(s) Sent Successfully....");

                // Notify user that process is End
                Console.WriteLine("Process Ended....");
            }
            catch (MyException _MyException)
            {
                Console.WriteLine(_MyException._StrMsg);                
            }
            catch (Exception _Exception)
            {
                Console.WriteLine(_Exception.Message);                
            }
        }

        
    }
}
