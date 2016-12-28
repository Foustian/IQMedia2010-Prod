using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace IQMediaGroup.Core.App_Code
{  

    public class GenericToDataTable
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        private GenericToDataTable()
        { }
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name=”T”>Custome Class </typeparam>
        /// <param name=”lst”>List Of The Custome Class</param>
        /// <returns> Return the class datatbl </returns>
        public static DataTable ConvertTo<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }

            return tbl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name=”T”>Custome Class</typeparam>
        /// <returns></returns>
        public static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column
                tbl.Columns.Add(prop.Name, prop.PropertyType);
            }
            return tbl;
        }
    }
}
