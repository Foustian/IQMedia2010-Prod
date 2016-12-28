using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Factory
{
    public class ModelFactory
    {
        public T CreateObject<T>()
        {
            System.Console.Write("");

            object requiredObject = ObjectFactory.CreateObject<T>();

            return (T)requiredObject;
        }
    }
}
