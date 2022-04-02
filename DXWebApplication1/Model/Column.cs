using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Model
{
    public class Column
    {
        public long ColumnId { get; set; }
        public String ColumnName { get; set; }

        public Column(long columnId, string columnName)
        {
            ColumnId = columnId;
            ColumnName = columnName;
        }

    }
}