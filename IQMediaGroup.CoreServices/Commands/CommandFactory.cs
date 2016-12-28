using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using IQMediaGroup.CoreServices.Config.Sections.Mappings;

namespace IQMediaGroup.CoreServices.Commands
{
    public static class CommandFactory
    {
        public static ICommand Create(UrlMapping urlMapping, NameValueCollection param)
        {
            ICommand command = new NullCommand();

            if (urlMapping == null)
                return command;

            //NOTE: Web.Config parameters must match order in code
            var parameters = new List<object>();
            foreach (var p in urlMapping.ActionClass.Parameters)
            {
                if (param[p.Key] == null)
                    parameters.Add(new NullParameter());
                //Gotta do a little extra work for Guids...
                else if (p.Type == "System.Guid")
                {
                    //NOTE: Gotta love that .NET 4 finally includes a TryParse for Guids :-)
                    Guid guid;
                    if (Guid.TryParse(param[p.Key], out guid)) parameters.Add(guid);
                    //If it didn't work, we pass null (and our code should expect a Guid? not a Guid)
                    else parameters.Add(null);
                }
                else
                {
                    //Try to convert and insert the value, fail to null if conversion fails...
                    try { parameters.Add(Convert.ChangeType(param[p.Key], Type.GetType(p.Type))); }
                    catch (Exception) { parameters.Add(null); }
                }
            }


            var t = Type.GetType(urlMapping.ActionClass.Type);
            var constInfo = t.GetConstructors();
            foreach (var constructor in constInfo)
            {
                //If the length of the parameters don't match, skip this constructor...
                if (constructor.GetParameters().Length != parameters.Count) continue;

                //Parameter count must match, so invoke the constructor...
                command = (ICommand)constructor.Invoke(parameters.ToArray());
                break;
            }

            return command;
        }
    }

    /// <summary>
    /// An empty class to represent a null parameter which means 
    /// a parameter wasn't passed at all. This is different from 'null'
    /// which means the parameter WAS passed with a null value...
    /// </summary>
    public class NullParameter { }
}