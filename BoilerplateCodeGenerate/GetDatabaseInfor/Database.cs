namespace GetDatabaseInfor
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    public class Database
    {
        public static List<string> GetDatabaseList()
        {
            List<string> list = new List<string>();
            // Open connection to the database
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                con.Open();
                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (!dr[0].ToString().Contains("master") && !dr[0].ToString().Contains("tempdb")
                                    && !dr[0].ToString().Contains("model") && !dr[0].ToString().Contains("msdb"))
                            {
                                list.Add(dr[0].ToString());
                            }
                        }
                    }
                }
            }
            return list.OrderBy(e => e).ToList();
        }

        /// <summary>
        /// Lấy hết table của db đã chọn
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static List<string> ListTables(string databaseName)
        {
            List<string> tables = new List<string>();
            using (SqlConnection con = new SqlConnection(Database.GetConnectionString(databaseName)))
            {
                con.Open();
                DataTable dt = con.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    string tablename = (string)row[2];
                    if (!tablename.Trim().Contains("Abp") && !tablename.Trim().Contains("EF")
                            && !tablename.Trim().Contains("sys"))
                    {
                        tables.Add(tablename);
                    }
                }
            }
            return tables.OrderBy(e => e).ToList();
        }

        /// <summary>
        /// Get danh sách các cột trong bảng
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static List<DataRow> GetListColumnNames(string DatabaseName, string TableName)
        {
            // Danh sách các cột không lấy
            List<string> rejectColumnNameList = new List<string>() { "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "TenantId" };
            using (SqlConnection conn = new SqlConnection(Database.GetConnectionString(DatabaseName)))
            {
                string[] restrictions = new string[4] { null, null, TableName, null };
                conn.Open();
                return conn.GetSchema("Columns", restrictions).AsEnumerable()
                           .Where(e => !rejectColumnNameList.Contains(e.ItemArray[3].ToString())).ToList();
            }
        }

        /// <summary>
        /// Tạo ra thuộc tính
        /// </summary>
        /// <param name="Column"></param>
        /// <returns></returns>
        public static string GetProperty(object[] Column)
        {
            StringBuilder sb = new StringBuilder();
            string colName = Column[3].ToString();
            string colType = Column[7].ToString();
            string allowNull = Column[6].ToString();
            sb.Append("        public virtual ");
            if (colType == "int")
            {
                sb.Append("int");
            }
            else if (colType == "bigint")
            {
                sb.Append("long");
            }
            else if (colType == "bit")
            {
                sb.Append("bool");
            }
            else if (colType == "float")
            {
                sb.Append("double");
            }
            else if (colType.Contains("date"))
            {
                sb.Append("DateTime");
            }
            else if (colType.Contains("nvarchar") || colType.Contains("nchar"))
            {
                sb.Append("string");
            }
            if (allowNull == "YES" && !colType.Contains("nvarchar") && !colType.Contains("nchar"))
            {
                sb.Append("?");
            }
            sb.Append(" " + colName);
            sb.Append(" { get; set; }");
            return sb.ToString();
        }

        private static string GetConnectionString(string databaseName = "")
        {
            string conString = "server=localhost;";
            if (!string.IsNullOrWhiteSpace(databaseName))
            {
                conString += "Database = " + databaseName + ";";
            }
            conString += "Trusted_Connection=True;";
            return conString;
        }
    }
}
