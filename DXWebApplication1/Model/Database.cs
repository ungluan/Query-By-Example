using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Model
{
    public class Database
    {
        public long DatabaseId { get; set; }
        public String DatabaseName { get; set; }
        public Database( long databaseId, String databaseName)
        {
            DatabaseId = databaseId;
            DatabaseName = databaseName;
        }
    }
}