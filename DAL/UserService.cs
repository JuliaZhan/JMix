using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserService
    {
        public static SqlConnection connection;
        public static SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    //远程连接数据库命令（前提远程数据库服务器已经配置好允许远程连接）  
                    string strConn = @"Data Source=.;Initial Catalog=test;User ID=sa;Password=1234.com;Persist Security Info=True";

                    //连接本地数据库命令  
                    //string strConn = @"Data Source=.;Initial Catalog=WebKuangjia;Integrated Security=True";  

                    connection = new SqlConnection(strConn);
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
        //执行sql语句,返回被修改行数  
        public static int ExecuteCommand(string commandText, CommandType commandType, SqlParameter[] para)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandText = commandText;
            try
            {
                if (para != null)
                {
                    cmd.Parameters.AddRange(para);
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
            }
        }

        //执行sql语句,返回数据库表  
        public static DataTable GetDataTable(string commandText, CommandType commandType, SqlParameter[] para)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            try
            {
                if (para != null)
                {
                    cmd.Parameters.AddRange(para);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable temp = new DataTable();
                da.Fill(temp);
                return temp;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
            }
        }

        //增加用户  
        public static bool AddAccount(Account account)
        {
            string sql = "insert into test(id,name,password)" + "values(@id,@name,@password)";//sql语句字符串  
            if (account.Id == null)
            {
                account.Id = "";
            }
            if (account.Name == null)
            {
                account.Name = "";
            }
            if (account.Password == null)
            {
                account.Password = "";
            }
            SqlParameter[] para = new SqlParameter[]//存储相应参数的容器  
            {
                new SqlParameter("@name",account.Name),
                new SqlParameter("@id",account.Id),
                new SqlParameter("@password",account.Password),
            };
            int count = ExecuteCommand(sql, CommandType.Text, para);//调用执行sql语句函数  
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //查询数据库表  
        public static DataTable Selecttable()
        {
            string sql = "select * from account";
            return GetDataTable(sql, CommandType.Text, null);
        }

        //删除用户  
        /****************删除用户返回影响行数*****************/
        public static bool DeleteAccountById(string id)
        {
            string sql = "delete from account where id=@id";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@id",id),
            };
            int count = ExecuteCommand(sql, CommandType.Text, para);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /****************删除用户返回表*****************/
        public static DataTable DeleteAccountBySid(int id)
        {
            string sql = "delete from account where id=@id";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@id",id),
            };
            return GetDataTable(sql, CommandType.Text, para);
        }

        //修改用户  
        public static bool ModifyAccount(Account user)
        {
            string sql = "update Student set Sname=@name,Ssex=@sex,Snumber=@number,Sgrade=@grade,Steacher=@teacher where Sid=@id";
            SqlParameter[] para = new SqlParameter[]
             {
                new SqlParameter("@id",user.Id),
                new SqlParameter("@name",user.Name),
                new SqlParameter("@password",user.Password),

             };
            int count = ExecuteCommand(sql, CommandType.Text, para);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //查询用户  
        public static bool QueryStudent(string id)
        {
            string sql = "select * from account where id=@id";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@id",id),
            };
            int count = ExecuteCommand(sql, CommandType.Text, para);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
