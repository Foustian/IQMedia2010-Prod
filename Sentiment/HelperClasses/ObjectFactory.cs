using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sentiment.HelperClasses
{
    public static class ObjectFactory
    {
        public static T CreateObject<T>()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            object requiredObject = null;

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    // If it does not implement the required Interface, skip it
                    if (type.GetInterface(typeof(T).FullName) == null)
                    {
                        continue;
                    }

                    requiredObject = Activator.CreateInstance(type);
                    break;
                }
            }

            return (T)requiredObject;
        }
    }
}
