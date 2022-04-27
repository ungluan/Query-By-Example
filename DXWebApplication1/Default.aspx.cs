using DXWebApplication1.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXWebApplication1 {
    public partial class Default : System.Web.UI.Page
    {
        private String connstr = "Data Source=LAPTOP-MDVV6CUJ;Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=true";
        public SqlConnection conn = new SqlConnection();
        public String curDatabase = "";
        public List<String> listTableSelected = new List<String>();
        public List<String> listColumnSelected = new List<String>();
        //public Dictionary<String, List<String>> map = new Dictionary<String, List<String>>();

        protected void Page_Load(object sender, EventArgs e) {
            listTableSelected.Add("");
            listColumnSelected.Add("");
            if (!IsPostBack) {
                conn.ConnectionString = connstr;
                loadDropdownDatabases();
                if (curDatabase != null){
                    loadCheckBoxTables(curDatabase);
                }
                Console.WriteLine(curDatabase);
            }
            else Console.WriteLine(curDatabase);
        }
        public SqlDataReader ExecSqlDataReader(String query)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(query, conn);
            sqlcmd.CommandType = CommandType.Text;

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString =connstr;
                conn.Open();
            }
            try
            {
                myreader = sqlcmd.ExecuteReader();
                return myreader;
            }
            catch (SqlException e)
            {
                conn.Close();
                return null;
            }
        }
        public DataTable ExecSqlDataTable(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        public List<Database> GetDatabases()
        {
            List<Database> databases = new List<Database>();
            String query = "SELECT database_id, name FROM master.sys.databases " +
                "WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'distribution')";
            SqlDataReader myReader = ExecSqlDataReader(query);
            if (myReader.Read())
            {
                do
                {
                    Database db = new Database(myReader.GetInt32(0), myReader.GetString(1));
                    databases.Add(db);
                } while (myReader.Read());
            }
            myReader.Close();
            return databases;
        }
        public void loadDropdownDatabases()
        {
            String query = "SELECT database_id, name FROM master.sys.databases " +
               "WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'distribution')";
            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                curDatabase = dt.Rows[0][1].ToString();
                DropDownListDatabase.DataSource = dt;
                DropDownListDatabase.DataValueField = "database_id";
                DropDownListDatabase.DataTextField = "name";
                DropDownListDatabase.DataBind();
            }
        }
        public void loadCheckBoxTables(String databaseName)
        {
            String query = "use " + databaseName +
                " select object_id as Table_Id, Name as Table_Name from sys.tables\n" +
                "WHERE name != 'sysdiagrams'";
            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = connstr;
                conn.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(dt);
            CheckBoxListTable.DataSource = dt;
            CheckBoxListTable.DataValueField = "Table_Id";
            CheckBoxListTable.DataTextField = "Table_Name";
            CheckBoxListTable.DataBind();
        }
        public List<TableDB> GetTables(String databaseName)
        {
            String query = "use " + databaseName +
                " select object_id as Table_Id, Name as Table_Name from sys.tables\n" +
                "WHERE name != 'sysdiagrams'";
            List<TableDB> tables = new List<TableDB>();

            SqlDataReader myReader = ExecSqlDataReader(query);
            if (myReader.Read())
            {
                do
                {
                    int id = myReader.GetInt32(0);
                    String name = myReader.GetString(1);
                    List<Column> listPrimaryKey = GetListPrimarykey(id);
                    List<ColumnForeignKey> listForeignKey = GetListForeignKey(id);
                    TableDB table = new TableDB(id, name, listPrimaryKey, listForeignKey);
                    tables.Add(table);
                } while (myReader.Read());
            }
            myReader.Close();

            return tables;
        }
        public List<Column> GetListPrimarykey(int tableId)
        {
            String query = "Select referenced_column_id as ColumnId, (SELECT COL_NAME(" + tableId + 
                ", referenced_column_id)) as ColumnName " +
                            "from sys.foreign_key_columns " +
                            "WHERE referenced_object_id = " + tableId;

            List<Column> listPrimaryKey = new List<Column>();

            SqlDataReader myReader = ExecSqlDataReader(query);
            if (myReader.Read())
            {
                do
                {
                    Column primayKey = new Column(myReader.GetInt32(0), myReader.GetString(1));
                    listPrimaryKey.Add(primayKey);
                } while (myReader.Read());
            }
            myReader.Close();

            return listPrimaryKey;
        }
        public List<ColumnForeignKey> GetListForeignKey(int tableId)
        {
            String query = "select referenced_object_id as TableId, " +
                "(select name from sys.tables WHERE object_id = referenced_object_id) as TableName, " +
                "referenced_column_id as ColumnId, " +
                "(SELECT COL_NAME(" + tableId + ", referenced_column_id)) as ColumnName " +
                "from sys.foreign_key_columns " +
                "WHERE parent_object_id = " + tableId;

            List<ColumnForeignKey> listForeignKey = new List<ColumnForeignKey>();

            SqlDataReader myReader = ExecSqlDataReader(query);
            if (myReader.Read())
            {
                do
                {
                    ColumnForeignKey foreignKey = new ColumnForeignKey(
                        myReader.GetInt32(0), myReader.GetString(1),
                        new Column(myReader.GetInt32(2), myReader.GetString(3)));
                    listForeignKey.Add(foreignKey);
                } while (myReader.Read());
            }
            myReader.Close();

            return listForeignKey;
        }

        protected void DropDownListDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCheckBoxTables(DropDownListDatabase.SelectedItem.Text);
        }
        private List<String> getInnerJoinString(List<String> tableSelectedQuery)
        {
            List<String> l = new List<string>();
            List<String> tableHandled = new List<String>();
            tableHandled.Add(tableSelectedQuery[0]);
            for(int i=0; i< tableSelectedQuery.Count; i++)
            {
                for(int j=0; j< tableSelectedQuery.Count; j++)
                {
                    String tableName1 = tableSelectedQuery[i];
                    String tableName2 = tableSelectedQuery[j];
                    String tableId1 = CheckBoxListTable.Items.FindByText(tableName1).Value;
                    String tableId2 = CheckBoxListTable.Items.FindByText(tableName2).Value;
                    if (getRelationship(tableName1, tableId1, tableName2, tableId2).Length > 0)
                    {
                        if (!tableHandled.Contains(tableName1)) l.Add("INNER JOIN " + tableName1 + getRelationship(tableName1, tableId1, tableName2, tableId2));
                        else l.Add("INNER JOIN " + tableName2 + getRelationship(tableName1, tableId1, tableName2, tableId2));
                    }
                }
            }
            return l;
        }
        private String getRelationship(String tableName1,String tableId1,String tableName2, String tableId2)
        {
            String query = " USE "+ DropDownListDatabase.SelectedItem.Text +" SELECT " +
                            "'"+ tableName1 + ".'" +
                            "+(SELECT COL_NAME(referenced_object_id,referenced_column_id)) as ColumnPrimaryKey, " +
                            "'" + tableName2 + ".'" + 
                            "+(SELECT COL_NAME(parent_object_id, parent_column_id)) as ColumnForeignKey " +
                            "from sys.foreign_key_columns " +
                            "where (parent_object_id = "+tableId2+" AND referenced_object_id ="+tableId1+")";
            SqlDataReader myReader = ExecSqlDataReader(query);
            String s = "";
            if (myReader!=null && myReader.Read())
            {
                String col1 = myReader.GetString(0);
                String col2 = myReader.GetString(1);
                s =  " ON " + col1 + "=" + col2;
                myReader.Close();
            }

            return s;
        }
        protected void CheckBoxListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxListTable.Items)
            {
                if (item.Selected) listTableSelected.Add(item.Text);
            }
            initCreateGridView();
            
        }
        private List<String> getColumnFromTable(String tableName)
        {
            List<String> columns = new List<string>();
            String query =  "USE " + DropDownListDatabase.SelectedItem.Text +
                            " SELECT Column_Name " +
                            "FROM INFORMATION_SCHEMA.COLUMNS " +
                            "WHERE TABLE_NAME = N'" + tableName + "'";
            SqlDataReader myReader = ExecSqlDataReader(query);
            if (myReader.Read())
            {
                do
                {
                    columns.Add(myReader.GetString(0));
                } while (myReader.Read());
            }
            myReader.Close();
            return columns;
        }
        private List<String> getColumnNameForGridView(String tableName)
        {
            List<String> columns = new List<string>();
            String query = "USE " + DropDownListDatabase.SelectedItem.Text +
                            " SELECT Column_Name " +
                            "FROM INFORMATION_SCHEMA.COLUMNS " +
                            "WHERE TABLE_NAME = N'" + tableName + "'";
            SqlDataReader myReader = ExecSqlDataReader(query);
            if (myReader.Read())
            {
                do
                {
                    columns.Add(tableName +"."+myReader.GetString(0));
                } while (myReader.Read());
            }
            myReader.Close();
            return columns;
        }

        protected void ButtonQuery_Click1(object sender, EventArgs e)
        {
            // Call table
            //String select = "Select ";
            //String where = "\nWhere ";
            //String groupBy = "\nGroup By ";
            //String having = "\nHaving ";
            //String orderBy = "\nOrder By ";
            //String from = "\n FROM ";
            //String innerjoin = "\n";

            List<String> select = new List<string>();
            List<String> where = new List<string>();
            List<String> groupBy = new List<string>();
            List<String> having = new List<string>();
            List<String> orderBy = new List<string>();
            List<String> from = new List<string>();
            List<String> innerjoin = new List<string>();

            List<String> tableSelectedQuery = new List<string>();
            foreach (GridViewRow row in GridView1.Rows)
            {
                String columnSelected = (row.Cells[0].FindControl("DropDownListColumn") as DropDownList).SelectedValue.ToString();
                String actionSelected = (row.Cells[1].FindControl("DropDownListAction") as DropDownList).SelectedValue.ToString();
                String condition = (row.Cells[2].FindControl("TextBoxWhere") as TextBox).Text.Trim();
                String groupBySelected = (row.Cells[3].FindControl("DropDownGroupBy") as DropDownList).SelectedValue.ToString();
                String havingGroupBy = (row.Cells[4].FindControl("TextBoxHaving") as TextBox).Text.Trim();
                String sort = (row.Cells[5].FindControl("DropDownListSort") as DropDownList).SelectedValue.ToString();
                String alias = (row.Cells[6].FindControl("TextBoxAlias") as TextBox).Text.Trim();

                if (columnSelected.Length == 0 || actionSelected.Length == 0) { continue; }
                select.Add(handleSelect(actionSelected, columnSelected, alias)); // thiếu ,
                if (condition.Length != 0) where.Add(columnSelected + condition); // thiếu AND
                if (groupBySelected.Equals("True"))
                {
                    groupBy.Add(columnSelected); // thiếu ,
                    if (havingGroupBy.Length != 0) having.Add(columnSelected +" "+ havingGroupBy);
                }
                if (sort.Length != 0) orderBy.Add(columnSelected + " "+sort); //  thiếu ,
                if (tableSelectedQuery.Count == 0)
                {
                    tableSelectedQuery.Add(columnSelected.Substring(0, columnSelected.IndexOf(".")));
                }
                else
                {
                    String newTable = columnSelected.Substring(0, columnSelected.IndexOf("."));
                    if (!tableSelectedQuery.Contains(newTable))
                    {
                        tableSelectedQuery.Add(newTable);
                    }
                }
            }
            // X? lý From:
            if (tableSelectedQuery.Count > 1)
            {
                from.Add(tableSelectedQuery[0]);
                from.Add(tableSelectedQuery[1]);
                innerjoin = getInnerJoinString(tableSelectedQuery);
            }
            // Hi?n th? lên form:
            String s = "";
            if (select.Count > 0)
            {
                s += "Select " + String.Join(",", select).ToString()+"\n";
                s += "From "+ tableSelectedQuery[0] +"\n";
                s += String.Join("\n", innerjoin) + "\n";
                if (where.Count > 0) s += "Where " + String.Join(" And ", where) +"\n";
                if(groupBy.Count>0) s += "Group by " + String.Join(",", groupBy) +"\n";
                if (having.Count > 0) s += "Having " + String.Join(" And ", having) + "\n";
                if(orderBy.Count>0) s+= "Order by "+String.Join("," ,orderBy) +"\n";
            }
            TextBoxQuery.Text = s;
            TextBoxQuery.BorderWidth = 1;
            TextBoxQuery.BorderColor = Color.FromArgb(239, 163, 42);
        }
        private String handleSelect(String select, String columnName, String alias)
        {
            switch (select)
            {
                case "Select":
                    {
                        if (alias.Length == 0) return columnName ;
                        return columnName + " As " + alias;
                    }
                case "Count":
                    {
                        if (alias.Length == 0) return "Count(" + columnName + ")";
                        return "Count(" + columnName + ") As " + alias;
                    }
                case "Sum":
                    {
                        if (alias.Length == 0) return "Sum(" + columnName + ")";
                        return "Sum(" + columnName + ") As " + alias;
                    }
                case "Min":
                    {
                        if (alias.Length == 0) return "Min(" + columnName + ")";
                        return "Min(" + columnName + ") As " + alias;
                    }
                case "Max":
                    {
                        if (alias.Length == 0) return "Max(" + columnName + ")" ;
                        return "Max(" + columnName + ") As " + alias ;
                    }
                case "Avg":
                    {
                        if (alias.Length == 0) return "Avg(" + columnName + ")" ;
                        return "Avg(" + columnName + ") As " + alias ;
                    }
                default: return "";
            }
        }

        private void initCreateGridView()
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < 12; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            listColumnSelected.Clear();
            listColumnSelected.Add("");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < listTableSelected.Count; i++)
                {
                    listColumnSelected.AddRange(getColumnNameForGridView(listTableSelected[i]));
                }
                bindingDropDownInGridView(e, "DropDownListColumn", listColumnSelected);
            }
        }
        private void bindingDropDownInGridView(GridViewRowEventArgs e, String idDropDown, List<String> data)
        {
            Control ctrl = e.Row.FindControl(idDropDown);
            if (ctrl != null)
            {
                DropDownList dd = ctrl as DropDownList;
                //Binding the Dropdown with Dummy data.
                dd.DataSource = data;
                dd.DataBind();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ButtonReport_Click(object sender, EventArgs e)
        {
            if(TextBoxQuery.Text.Trim().Length > 0)
            {
                try
                {
                    Session["title"] = TextBoxTitle.Text;
                    Session["query"] = TextBoxQuery.Text;
                    Session["database"] = DropDownListDatabase.SelectedItem.Text;
                    Response.Redirect("Viewer.aspx");
                    Server.Execute("Viewer.aspx");
                }
                catch (Exception ex) { }
            }
            else
            {
                TextBoxQuery.BorderWidth = 2;
                TextBoxQuery.BorderColor = Color.Red;
            }
        }
    }
}