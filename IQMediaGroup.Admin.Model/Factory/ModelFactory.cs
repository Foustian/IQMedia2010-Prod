using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Factory
{
    public class ModelFactory
    {
        public T CreateObject<T>()
        {
            object requiredObject = ObjectFactory.CreateObject<T>();

            return (T)requiredObject;
        }
    }
}
