using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Core.HelperClasses
{
   public class SQLDataType
    {
        public string ParameterName { get; set; }

        public SqlDbType dbType { get; set; }

        public object Value { get; set; }

        public ParameterDirection Direction { get; set; }

        public SQLDataType()
        {

        }

        public SQLDataType(string ParameterName, SqlDbType  dbType, object Value, ParameterDirection Direction)
        {
            this.ParameterName = ParameterName;
            this.dbType = dbType;
            this.Value = Value;
            this.Direction = Direction;
        }
    }
}
