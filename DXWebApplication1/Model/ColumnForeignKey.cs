using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Model
{
    public class ColumnForeignKey
    {
        public long TableId { get; set; }
        public String TableName { get; set; }

        public Column ForeignKey { get; set; }

        public ColumnForeignKey(long tableId, string tableName, Column foreignKey)
        {
            TableId = tableId;
            TableName = tableName;
            ForeignKey = foreignKey;
        }

    }
}