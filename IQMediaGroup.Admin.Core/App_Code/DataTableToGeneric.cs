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
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Core.App_Code
{

    public static class DataTableToGeneric
    {

        public static List<T> toList<T>(this DataTable table)
        {
            List<T> list = new List<T>();

            T item;
            Type listItemType = typeof(T);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                item = (T)Activator.CreateInstance(listItemType);
                mapRow(item, table, listItemType, i);
                list.Add(item);
            }
            return list;
        }
        private static void mapRow(object vOb, System.Data.DataTable table, Type type, int row)
        {
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var columnName = table.Columns[col].ColumnName;
                var prop = type.GetProperty(columnName);
                object data = table.Rows[row][col];


                
                if (prop.PropertyType==typeof(Guid) || prop.PropertyType==typeof(Guid?))
                {
                    Guid _Guid = new Guid(data.ToString());
                    prop.SetValue(vOb, _Guid, null);
                }

                else if (prop.PropertyType.Name.ToLower() == "int32")
                {
                    int? _int = null;

                    _int = CommonFunctions.GetIntValue(data.ToString());

                    prop.SetValue(vOb, _int, null);
                }
                else if (prop.PropertyType.Name.ToLower() == "int64")
                {
                    Int64? _int = null;

                    _int = CommonFunctions.GetIntValue(data.ToString());

                    prop.SetValue(vOb, _int, null);
                }
                else
                {
                    if (data != null && !string.IsNullOrEmpty(data.ToString()))
                    {
                        prop.SetValue(vOb, data, null);
                    }
                    else 
                    {
                        prop.SetValue(vOb, string.Empty, null);
                    }
                }
            }
        }

    }
}
