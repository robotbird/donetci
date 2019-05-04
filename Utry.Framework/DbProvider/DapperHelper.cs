using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Utry.Framework.Configuration;

namespace Utry.Framework.DbProvider
{
    public class DapperHelper
    {
        //access 数据库连接
       // private static string _mdbpath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection);
       // public static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _mdbpath;

        public static string ConnectionString = "";
        //连接数据库字符串。
        private readonly string sqlconnection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //获取Sql Server的连接数据库对象。SqlConnection

        public OleDbConnection OpenConnectionAccess()
        {
             OleDbConnection conn = new OleDbConnection(ConnectionString);
             conn.Open();
             return conn;
        }

        //public OleDbConnection OpenSqliteConnection()
        //{
        //    SQLiteConnection conn = new SQLiteConnection(ConnectionString);
        //    conn.Open();
        //    return conn;
        //}
      
        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// 获取分页Sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="colList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderBy"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetPageSql(string tableName, string colName, string colList, int pageSize, int pageIndex, int orderBy, string condition)
        {
            string temp = string.Empty;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(condition))
            {
                condition = " 1=1 ";
            }

            //降序
            if (orderBy == 1)
            {
                temp = "select top {0} {1} from {2} where {5} and {3} <(select min(pk) from ( select top {4} {3} as pk from {2} where {5} order by {3} desc) t) order by {3} desc";
                sql = string.Format(temp, pageSize, colList, tableName, colName, pageSize * (pageIndex - 1), condition);
            }
            //降序
            if (orderBy == 0)
            {
                temp = "select top {0} {1} from {2} where {5} and {3} >(select max(pk) from ( select top {4} {3} as pk from {2} where {5} order by {3} asc) t) order by {3} asc";
                sql = string.Format(temp, pageSize, colList, tableName, colName, pageSize * (pageIndex - 1), condition);
            }
            //第一页
            if (pageIndex == 1)
            {
                temp = "select top {0} {1} from {2} where {3} order by {4} {5}";
                sql = string.Format(temp, pageSize, colList, tableName, condition, colName, orderBy == 1 ? "desc" : "asc");
            }

            return sql;
        }

    }
}
