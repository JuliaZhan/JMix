using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class UserManage
    {
        public static bool add(Account account)
        {
            return UserService.AddAccount(account);
        }
        public static bool delete(string id)
        {
            return UserService.DeleteAccountById(id);
        }
        public static bool xiugai(string number)
        {
            return UserService.QueryStudent(number);
        }
        public static bool modify(Account account)
        {
            return UserService.ModifyAccount(account);
        }
        public static bool select(string number)
        {
            return UserService.QueryStudent(number);
        }
        public static DataTable table()
        {
            return UserService.Selecttable();
        }
        public static DataTable deletebyid(int id)
        {
            return UserService.DeleteAccountBySid(id);
        }
    }
}
