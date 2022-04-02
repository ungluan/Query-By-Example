using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Model
{
    public class TableDB
    {
        public long TableId { get; set; }
        public String TableName { get; set; }

        public List<Column> ListPrimaryKey { get; set; }
        public List<ColumnForeignKey> ListForeignKey { get; set; }

        public TableDB(long tableId, string tableName, List<Column> listPrimaryKey, 
            List<ColumnForeignKey> listColumnForeignKey)
        {
            TableId = tableId;
            TableName = tableName;
            ListPrimaryKey = listPrimaryKey;
            ListForeignKey = listColumnForeignKey;
        }
    }
}