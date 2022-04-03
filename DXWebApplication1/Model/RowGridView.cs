using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Model
{
    public class RowGridView
    {
        public List<String> Tables { get; set; }
        public List<String> Fields { get; set; }
        public List<String> Selects { get; set; }
        public String Where { get; set; }
        public String Having { get; set; }
        public List<String> Sort { get; set; }
        public String Alias { get; set; }

        public RowGridView(List<String> tables, List<String> fields, List<String> selects,
            String where, String having, List<String> sort, String alias)
        {
            Tables = tables;
            Fields = fields;
            Selects = selects;
            Where = where;
            Having = having;
            Sort = sort;
            Alias = alias;
        }

    }
}