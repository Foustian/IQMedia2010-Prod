using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;


namespace IQMediaGroup.Admin.Controller.Factory
{
    /// <summary>
    /// ControllerFactory class to create object of controller class
    /// </summary>
    public class ControllerFactory
    {
        /// <summary>
        /// This Method creates object of particular controller type.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <returns>Object of particular class</returns>
        public T CreateObject<T>()
        {
            object requiredObject = ObjectFactory.CreateObject<T>();

            return (T)requiredObject;
        }
    }
}
