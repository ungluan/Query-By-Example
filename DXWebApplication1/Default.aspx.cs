using DXWebApplication1.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXWebApplication1 {
    public partial class Default : System.Web.UI.Page {
        private String connstr = "Data Source=LAPTOP-MDVV6CUJ;Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=true";
        public SqlConnection conn = new SqlConnection();
        public String curDatabase = "";
        public List<String> listTableSelected = new List<String>();
        public List<String> listColumnSelected = new List<String>();
        public Dictionary<String, List<String>> map = new Dictionary<String, List<String>>();

        protected void Page_Load(object sender, EventArgs e) {
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
            String query = "Select referenced_column_id as ColumnId, (SELECT COL_NAME(" + tableId + ", referenced_column_id)) as ColumnName " +
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
            curDatabase = DropDownListDatabase.SelectedItem.Text;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Call table
        }

        protected void CheckBoxListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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

        protected void Button1_Click1(object sender, EventArgs e)
        {
            listTableSelected.Clear();
            foreach (ListItem item in CheckBoxListTable.Items)
            {
                if (item.Selected) listTableSelected.Add(item.Text);
            }

            listTableSelected.ForEach(tableName => map.Add(tableName, getColumnFromTable(tableName)));
            // Th?c hi?n load GridView
            if (GridView1.Rows.Count == 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Column0"));
                dt.Columns.Add(new DataColumn("Column1", typeof(string)));
                dt.Columns.Add(new DataColumn("Column2", typeof(string)));
                dt.Columns.Add(new DataColumn("Column3", typeof(string)));
                dt.Columns.Add(new DataColumn("Column4", typeof(string)));
                dt.Columns.Add(new DataColumn("Column5", typeof(string)));
                dt.Columns.Add(new DataColumn("Column6", typeof(string)));
                dr = dt.NewRow();
                List<String> cl1 = new List<String>();
                cl1.Add("2");
                cl1.Add("1");
                cl1.Add("1");

                dr["Column0"] = listTableSelected;
                dr["Column1"] = string.Empty;
                dr["Column2"] = string.Empty;
                dr["Column3"] = string.Empty;
                dr["Column4"] = string.Empty;
                dr["Column5"] = string.Empty;
                dr["Column6"] = string.Empty;

                dt.Rows.Add(dr);

                //Store the DataTable in ViewState

                ViewState["CurrentTable"] = dt;



                GridView1.DataSource = dt;

                GridView1.DataBind();

            }
    }
    }
}
