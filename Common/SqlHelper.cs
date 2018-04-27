using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    /// <summary>
    /// 概述：sql server 数据库操作
    /// 文件：Common.SqlHelper
    /// 作者：詹詹    创建时间：2018/4/27 11:48:16
    /// 描述：
    ///    > add description for SqlHelper
    ///    修改历史：
    ///
    /// <summary>
    public abstract class SqlHelper
    {
        //根据xml文件获取数据库连接
        public static readonly string conStr = GetConnectString();
        public static string GetConnectString()
        {
            string configXml = "c:\\Config\\config.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(configXml);
            XmlNode xmlNode = doc.SelectSingleNode("/ExtensionConfig/ConnectionString/ConnectionString");
            return xmlNode.InnerText;
        }
        //根据appconfig获取数据库连接
        //public static readonly string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        /// <summary>
        /// 执行一条返回结果集的SqlCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数
        /// </summary>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(conStr);

            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">有效的Sql语句</param>
        /// <returns>返回DataReader</returns>
        public static SqlDataReader ExcuteReader(string sql)
        {
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>
        /// 通过SQL语句来获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataBySql(string sql)
        {
            try
            {
                SqlConnection conn = new SqlConnection(conStr);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
