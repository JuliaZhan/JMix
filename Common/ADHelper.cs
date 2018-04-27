using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 概述：AD域操作
    /// 文件：Common.ADHelper
    /// 作者：詹詹    创建时间：2018/4/27 11:31:50
    /// 描述：
    ///    > add description for ADHelper
    ///    修改历史：
    ///
    /// <summary>
    public class ADHelper
    {
        /// <summary>
        /// 根据域名查询该用户是否域用户
        /// </summary>
        /// <param name="searchUser">域名</param>
        /// <param name="firstname">名</param>
        /// <param name="lastname">姓</param>
        /// <returns></returns>
        public static bool HasUser(string searchUser, out string firstname, out string lastname)
        {
            bool isExist = false;
            Log.i(string.Format("获取当前用户信息:{0}", searchUser));
            DirectoryEntry entry = null;
            string _firstname = "";
            string _lastname = "";
            try
            {
                string domain = System.Configuration.ConfigurationManager.AppSettings["ADdomain"];//获取域
                var CrmUserName = System.Configuration.ConfigurationManager.AppSettings["CrmUserName"];
                var CrmPassword = System.Configuration.ConfigurationManager.AppSettings["CrmPassword"];
                entry = new DirectoryEntry(string.Format("LDAP://{0}", domain), CrmUserName, CrmPassword, AuthenticationTypes.Secure);
                if (entry != null)
                {
                    DirectorySearcher mySearcher = new DirectorySearcher(entry);
                    //mySearcher.Filter= ("(&(|(cn=" + searchUser + ")))");
                    mySearcher.Filter = ("(&(objectClass=user)(sAMAccountName=" + searchUser + "))");
                    SearchResult searchResult = mySearcher.FindOne();
                    if (searchResult != null)
                    {
                        isExist = true;
                        Log.i("获取当前用户是域用户");
                        _firstname = searchResult.Properties["sn"][0].ToString();
                        _lastname = searchResult.Properties["givenname"][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            firstname = _firstname;
            lastname = _lastname;
            return isExist;
        }
    }
}
